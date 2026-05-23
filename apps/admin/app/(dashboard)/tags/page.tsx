import { z } from "zod";
import { getApiClient } from "@/lib/api/client";
import { DeleteTagButton } from "@/domain/tags/delete/DeleteTagButton";
import { RenameTagButton } from "@/domain/tags/rename/RenameTagButton";

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
    <section>
      <h1 className="text-2xl font-bold mb-6">Tags</h1>
      <div className="max-w-2xl">
        <form action={createTag} className="flex gap-2 mb-6">
          <input
            name="name"
            required
            placeholder="Tag name"
            className="flex-1 border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          <button
            type="submit"
            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
          >
            Add Tag
          </button>
        </form>
        <div className="bg-white rounded shadow overflow-hidden">
          <table className="w-full">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-4 py-3 text-left text-sm font-semibold text-gray-600">Name</th>
                <th className="px-4 py-3 text-left text-sm font-semibold text-gray-600">Slug</th>
                <th className="px-4 py-3 text-left text-sm font-semibold text-gray-600">Posts</th>
                <th className="px-4 py-3 text-left text-sm font-semibold text-gray-600">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100">
              {data.map((tag) => (
                <tr key={tag.tagId} className="hover:bg-gray-50">
                  <td className="px-4 py-3 font-medium">{tag.name}</td>
                  <td className="px-4 py-3 text-gray-500 text-sm">{tag.slug}</td>
                  <td className="px-4 py-3 text-sm">{tag.postCount}</td>
                  <td className="px-4 py-3">
                    <RenameTagButton tagId={tag.tagId} tagName={tag.name} />
                    <DeleteTagButton tagId={tag.tagId} tagName={tag.name} />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          {data.length === 0 && (
            <p className="text-center text-gray-500 py-8">No tags yet.</p>
          )}
        </div>
      </div>
    </section>
  );
}
