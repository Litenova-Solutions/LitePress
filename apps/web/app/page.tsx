import Link from "next/link";

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";

interface PostSummary {
  postId: string;
  title: string;
  slug: string;
  excerpt?: string;
  publishedAt?: string;
  tags: Array<{ tagId: string; tagName: string; tagSlug: string }>;
}

interface PagedResult {
  items: PostSummary[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

export default async function HomePage({
  searchParams,
}: {
  searchParams: Promise<{ page?: string; tag?: string }>;
}) {
  const sp = await searchParams;
  const page = parseInt(sp.page || "1");
  const tag = sp.tag;

  const url = tag
    ? API_URL + "/api/posts?tag=" + tag + "&page=" + page + "&pageSize=10"
    : API_URL + "/api/posts?page=" + page + "&pageSize=10";

  const data = await fetch(url, { next: { tags: ["posts"], revalidate: 3600 } }).then(r => r.json()) as PagedResult;

  return (
    <main className="max-w-3xl mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-8">{tag ? "Posts tagged: " + tag : "Latest Posts"}</h1>
      <div className="space-y-8">
        {data.items.map((post) => (
          <article key={post.postId} className="border-b pb-8">
            <h2 className="text-xl font-semibold mb-2">
              <Link href={"/" + post.slug} className="hover:text-blue-600">
                {post.title}
              </Link>
            </h2>
            {post.excerpt && <p className="text-gray-600 mb-3">{post.excerpt}</p>}
            <div className="flex gap-2 flex-wrap">
              {post.tags?.map((t) => (
                <Link key={t.tagId} href={"/?tag=" + t.tagSlug}
                  className="text-xs bg-gray-100 text-gray-600 px-2 py-1 rounded hover:bg-gray-200">
                  {t.tagName}
                </Link>
              ))}
            </div>
            {post.publishedAt && (
              <p className="text-sm text-gray-400 mt-2">
                {new Date(post.publishedAt).toLocaleDateString()}
              </p>
            )}
          </article>
        ))}
      </div>
      {data.items.length === 0 && (
        <p className="text-gray-500 text-center py-12">No posts yet.</p>
      )}
      <div className="flex justify-between mt-8">
        {page > 1 && (
          <Link href={"/?page=" + (page - 1) + (tag ? "&tag=" + tag : "")}
            className="text-blue-600 hover:underline">
            ← Previous
          </Link>
        )}
        {data.items.length === 10 && (
          <Link href={"/?page=" + (page + 1) + (tag ? "&tag=" + tag : "")}
            className="text-blue-600 hover:underline ml-auto">
            Next →
          </Link>
        )}
      </div>
    </main>
  );
}