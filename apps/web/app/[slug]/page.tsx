import { notFound } from "next/navigation";

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";

interface PostDetail {
  postId: string;
  title: string;
  slug: string;
  content: string;
  excerpt?: string;
  coverImageUrl?: string;
  state: string;
  publishedAt?: string;
  authorName: string;
  tags: Array<{ tagId: string; tagName: string; tagSlug: string }>;
}

export default async function PostPage({ params }: { params: Promise<{ slug: string }> }) {
  const { slug } = await params;

  const res = await fetch(API_URL + "/api/posts/" + slug, {
    next: { tags: ["posts"], revalidate: 3600 },
  });

  if (res.status === 404) notFound();
  if (!res.ok) throw new Error("Failed to load post");

  const post = await res.json() as PostDetail;

  if (post.state !== "Published") notFound();

  return (
    <main className="max-w-3xl mx-auto px-4 py-8">
      <article>
        {post.coverImageUrl && (
          <img src={post.coverImageUrl} alt={post.title}
            className="w-full h-64 object-cover rounded mb-8" />
        )}
        <h1 className="text-3xl font-bold mb-4">{post.title}</h1>
        <div className="flex gap-4 text-sm text-gray-500 mb-6">
          <span>{post.authorName}</span>
          {post.publishedAt && <span>{new Date(post.publishedAt).toLocaleDateString()}</span>}
        </div>
        <div
          className="prose prose-lg max-w-none"
          dangerouslySetInnerHTML={{ __html: post.content }}
        />
        {post.tags?.length > 0 && (
          <div className="flex gap-2 mt-8 pt-8 border-t">
            {post.tags.map((t) => (
              <a key={t.tagId} href={"/?tag=" + t.tagSlug}
                className="text-xs bg-gray-100 text-gray-600 px-2 py-1 rounded hover:bg-gray-200">
                {t.tagName}
              </a>
            ))}
          </div>
        )}
      </article>
    </main>
  );
}