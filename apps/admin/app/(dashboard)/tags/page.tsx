import { z } from "zod";
import { getApiClient } from "@/lib/api/client";
import { DeleteTagButton } from "@/features/tags/delete/DeleteTagButton";
import { RenameTagButton } from "@/features/tags/rename/RenameTagButton";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";

const createTagSchema = z.object({
  name: z.string().trim().min(1, "Tag name is required").max(50, "Tag name too long"),
});

async function createTag(formData: FormData) {
  "use server";
  const parsed = createTagSchema.safeParse({ name: formData.get("name") });
  if (!parsed.success) {
    throw new Error(parsed.error.errors[0]?.message ?? "Invalid tag name");
  }

  const client = await getApiClient();
  const { error } = await client.POST("/api/tags", {
    body: { name: parsed.data.name },
  });

  if (error) {
    throw new Error("Failed to create tag");
  }
}

export default async function TagsPage() {
  const client = await getApiClient();
  const { data, error } = await client.GET("/api/tags");

  if (error || !data) {
    throw new Error("Failed to load tags");
  }

  return (
    <section className="space-y-6">
      <div>
        <h1 className="font-heading text-3xl font-semibold tracking-tight">Tags</h1>
        <p className="text-sm text-muted-foreground">Create and manage topic labels for posts.</p>
      </div>

      <Card className="max-w-3xl">
        <CardHeader>
          <CardTitle>Add tag</CardTitle>
        </CardHeader>
        <CardContent>
          <form action={createTag} className="flex flex-col gap-3 sm:flex-row">
            <Input name="name" required placeholder="Tag name" className="flex-1" />
            <Button type="submit">Add tag</Button>
          </form>
        </CardContent>
      </Card>

      <Card className="max-w-3xl">
        <CardHeader>
          <CardTitle>All tags</CardTitle>
        </CardHeader>
        <CardContent className="px-0 pb-0">
          {data.length === 0 ? (
            <p className="px-6 pb-6 text-center text-sm text-muted-foreground">No tags yet.</p>
          ) : (
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Name</TableHead>
                  <TableHead>Slug</TableHead>
                  <TableHead>Posts</TableHead>
                  <TableHead className="text-right">Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {data.map((tag) => (
                  <TableRow key={tag.tagId}>
                    <TableCell className="font-medium">{tag.name}</TableCell>
                    <TableCell className="text-muted-foreground">{tag.slug}</TableCell>
                    <TableCell>{tag.postCount}</TableCell>
                    <TableCell className="text-right">
                      <div className="flex justify-end gap-2">
                        <RenameTagButton tagId={tag.tagId} tagName={tag.name} />
                        <DeleteTagButton tagId={tag.tagId} tagName={tag.name} />
                      </div>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          )}
        </CardContent>
      </Card>
    </section>
  );
}
