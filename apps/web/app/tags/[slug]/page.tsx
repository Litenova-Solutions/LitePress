import Link from "next/link";
import { notFound } from "next/navigation";

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";

export default async function TagPage({ params }: { params: Promise<{ slug: string }> }) {
  const { slug } = await params;

  const res = await fetch(API_URL + "/api/posts?tag=" + slug + "&page=1&pageSize=20", {
    next: { tags: ["posts"], revalidate: 3600 },
  });

  if (!res.ok) notFound();
  const data = await res.json();

  return (
    <main className="max-w-3xl mx-auto px-4 py-8">
      <h1 className="text-2xl font-bold mb-6">Posts tagged: {slug}</h1>
      <div className="space-y-6">
        {data.items.map((post: { postId: string; title: string; slug: string; excerpt?: string }) => (
          <article key={post.postId} className="border-b pb-6">
            <h2 className="text-lg font-semibold">
              <Link href={"/" + post.slug} className="hover:text-blue-600">{post.title}</Link>
            </h2>
            {post.excerpt && <p className="text-gray-600 mt-1">{post.excerpt}</p>}
          </article>
        ))}
      </div>
      {data.items.length === 0 && (
        <p className="text-gray-500 text-center py-12">No posts with this tag.</p>
      )}
    </main>
  );
}