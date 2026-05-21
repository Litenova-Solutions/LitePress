import Link from "next/link";
import { apiGet } from "../../../lib/api";

interface PostSummary {
  postId: string;
  title: string;
  slug: string;
  state: string;
  createdAt: string;
  publishedAt?: string;
}

interface PagedResult {
  items: PostSummary[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

export default async function PostsPage() {
  const data = await apiGet<PagedResult>("/api/posts?page=1&pageSize=50");

  return (
    <section>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-bold">Posts</h1>
        <Link href="/posts/new" className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">
          New Post
        </Link>
      </div>
      <div className="bg-white rounded shadow overflow-hidden">
        <table className="w-full">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-4 py-3 text-left text-sm font-semibold text-gray-600">Title</th>
              <th className="px-4 py-3 text-left text-sm font-semibold text-gray-600">Slug</th>
              <th className="px-4 py-3 text-left text-sm font-semibold text-gray-600">Status</th>
              <th className="px-4 py-3 text-left text-sm font-semibold text-gray-600">Created</th>
              <th className="px-4 py-3 text-left text-sm font-semibold text-gray-600">Actions</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-100">
            {data.items.map((post) => (
              <tr key={post.postId} className="hover:bg-gray-50">
                <td className="px-4 py-3 font-medium">{post.title}</td>
                <td className="px-4 py-3 text-gray-500 text-sm">{post.slug}</td>
                <td className="px-4 py-3">
                  <span className={`inline-block px-2 py-1 rounded text-xs font-semibold ${
                    post.state === "Published" ? "bg-green-100 text-green-800" :
                    post.state === "Archived" ? "bg-gray-100 text-gray-600" :
                    "bg-yellow-100 text-yellow-800"
                  }`}>
                    {post.state}
                  </span>
                </td>
                <td className="px-4 py-3 text-sm text-gray-500">
                  {new Date(post.createdAt).toLocaleDateString()}
                </td>
                <td className="px-4 py-3">
                  <Link href={`/posts/${post.postId}`} className="text-blue-600 hover:underline text-sm">
                    Edit
                  </Link>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        {data.items.length === 0 && (
          <p className="text-center text-gray-500 py-8">No posts yet. Create your first post!</p>
        )}
      </div>
    </section>
  );
}