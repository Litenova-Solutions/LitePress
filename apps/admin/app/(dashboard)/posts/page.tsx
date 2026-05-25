import Link from "next/link";
import { getApiClient } from "@/lib/api/client";
import { buttonVariants } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { PostStatusBadge } from "@/components/PostStatusBadge";

export default async function PostsPage() {
  const client = await getApiClient();
  const { data, error } = await client.GET("/api/posts", {
    params: { query: { page: 1, pageSize: 50 } },
  });

  if (error || !data) {
    throw new Error("Failed to load posts");
  }

  return (
    <section className="space-y-6">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <div>
          <h1 className="font-heading text-3xl font-semibold tracking-tight">Posts</h1>
          <p className="text-sm text-muted-foreground">Manage drafts, published posts, and archives.</p>
        </div>
        <Link href="/posts/new" className={buttonVariants()}>
          New post
        </Link>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>All posts</CardTitle>
        </CardHeader>
        <CardContent className="px-0 pb-0">
          {data.items.length === 0 ? (
            <p className="px-6 pb-6 text-center text-sm text-muted-foreground">
              No posts yet. Create your first post.
            </p>
          ) : (
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Title</TableHead>
                  <TableHead>Slug</TableHead>
                  <TableHead>Status</TableHead>
                  <TableHead>Created</TableHead>
                  <TableHead className="text-right">Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {data.items.map((post) => (
                  <TableRow key={post.postId}>
                    <TableCell className="font-medium">{post.title}</TableCell>
                    <TableCell className="text-muted-foreground">{post.slug}</TableCell>
                    <TableCell>
                      <PostStatusBadge status={post.postState} />
                    </TableCell>
                    <TableCell className="text-muted-foreground">
                      {new Date(post.createdAt).toLocaleDateString()}
                    </TableCell>
                    <TableCell className="text-right">
                      <Link
                        href={`/posts/${post.postId}`}
                        className={buttonVariants({ variant: "ghost", size: "sm" })}
                      >
                        Edit
                      </Link>
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
