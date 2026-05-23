import { env } from "@/lib/env";
import { apiGet } from "@/lib/api";

interface PostSummary {
  postId: string;
  title: string;
  state: string;
}

interface TagSummary {
  tagId: string;
  name: string;
}

interface PagedPosts {
  totalCount: number;
  items: PostSummary[];
}

export default async function DashboardPage() {
  const [posts, tags] = await Promise.all([
    apiGet<PagedPosts>("/api/posts?page=1&pageSize=500"),
    apiGet<TagSummary[]>("/api/tags"),
  ]);

  const draftCount = posts.items.filter((p) => p.state === "Draft").length;
  const publishedCount = posts.items.filter((p) => p.state === "Published").length;
  const archivedCount = posts.items.filter((p) => p.state === "Archived").length;

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
