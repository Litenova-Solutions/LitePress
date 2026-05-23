import { apiGet, apiPost } from "@/lib/api";
import { DeleteTagButton } from "@/domain/tags/delete/DeleteTagButton";

interface Tag {
  tagId: string;
  name: string;
  slug: string;
  postCount: number;
}

async function createTag(formData: FormData) {
  "use server";
  const name = formData.get("name") as string;
  await apiPost("/api/tags", { name });
}

export default async function TagsPage() {
  const tags = await apiGet<Tag[]>("/api/tags");

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
              {tags.map((tag) => (
                <tr key={tag.tagId} className="hover:bg-gray-50">
                  <td className="px-4 py-3 font-medium">{tag.name}</td>
                  <td className="px-4 py-3 text-gray-500 text-sm">{tag.slug}</td>
                  <td className="px-4 py-3 text-sm">{tag.postCount}</td>
                  <td className="px-4 py-3">
                    <DeleteTagButton tagId={tag.tagId} tagName={tag.name} />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          {tags.length === 0 && (
            <p className="text-center text-gray-500 py-8">No tags yet.</p>
          )}
        </div>
      </div>
    </section>
  );
}
