import type { Metadata } from "next";
import { notFound } from "next/navigation";
import { PostList } from "@/domain/posts/list-published-posts/PostList";
import { env } from "@/lib/env";

export const dynamic = "force-dynamic";

export async function generateMetadata({
  params,
}: {
  params: Promise<{ slug: string }>;
}): Promise<Metadata> {
  const { slug } = await params;
  const title = `Posts tagged: ${slug}`;
  const description = `Published posts tagged with ${slug}.`;
  const canonical = env.siteUrl + "/tags/" + slug;

  return {
    title,
    description,
    alternates: { canonical },
    openGraph: { title, description, url: canonical, type: "website" },
    twitter: { card: "summary", title, description },
    robots: { index: true, follow: true },
  };
}

export default async function TagPage({
  params,
}: {
  params: Promise<{ slug: string }>;
}) {
  const { slug } = await params;

  const res = await fetch(
    env.API_URL + "/api/posts?tag=" + slug + "&page=1&pageSize=20",
    { next: { tags: ["posts", "tag-" + slug], revalidate: 3600 } }
  );

  if (!res.ok) {
    notFound();
  }

  const data = await res.json();

  return (
    <PostList
      posts={data.items}
      heading={"Posts tagged: " + slug}
      page={1}
      hasNextPage={data.items.length === 20}
      tagSlug={slug}
    />
  );
}
