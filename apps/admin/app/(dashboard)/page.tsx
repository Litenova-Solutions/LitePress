import { env } from "@/lib/env";
import { getApiClient } from "@/lib/api/client";

export default async function DashboardPage() {
  const client = await getApiClient();

  const [postsResult, tagsResult] = await Promise.all([
    client.GET("/api/posts", { params: { query: { page: 1, pageSize: 500 } } }),
    client.GET("/api/tags"),
  ]);

  if (postsResult.error || !postsResult.data) {
    throw new Error("Failed to load posts");
  }
  if (tagsResult.error || !tagsResult.data) {
    throw new Error("Failed to load tags");
  }

  const posts = postsResult.data;
  const tags = tagsResult.data;

  const draftCount = posts.items.filter((p) => p.postState === "Draft").length;
  const publishedCount = posts.items.filter((p) => p.postState === "Published").length;
  const archivedCount = posts.items.filter((p) => p.postState === "Archived").length;

  return (
    <section>
      <h1 className="text-2xl font-bold mb-6">Dashboard</h1>
      <div className="grid grid-cols-1 sm:grid-cols-4 gap-4 mb-8">
        <div className="bg-white rounded shadow p-4">
          <p className="text-sm text-gray-500">Total posts</p>
          <p className="text-2xl font-bold">{posts.totalCount}</p>
        </div>
        <div className="bg-white rounded shadow p-4">
          <p className="text-sm text-gray-500">Published</p>
          <p className="text-2xl font-bold text-green-700">{publishedCount}</p>
        </div>
        <div className="bg-white rounded shadow p-4">
          <p className="text-sm text-gray-500">Drafts</p>
          <p className="text-2xl font-bold text-yellow-700">{draftCount}</p>
        </div>
        <div className="bg-white rounded shadow p-4">
          <p className="text-sm text-gray-500">Tags</p>
          <p className="text-2xl font-bold">{tags.length}</p>
        </div>
      </div>
      <p className="text-sm text-gray-500">
        Archived posts: {archivedCount}. API: {env.API_URL}
      </p>
    </section>
  );
}
