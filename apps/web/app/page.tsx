import type { Metadata } from "next";
import { PostList } from "@/features/posts/list-published-posts/PostList";
import { getApiClient } from "@/lib/api/client";
import { env } from "@/lib/env";

export const dynamic = "force-dynamic";

export async function generateMetadata({
  searchParams,
}: {
  searchParams: Promise<{ tag?: string }>;
}): Promise<Metadata> {
  const sp = await searchParams;
  const title = sp.tag ? `Posts tagged: ${sp.tag}` : "Latest Posts";
  const description = "Published posts from LitePress.";
  const canonical = env.siteUrl + (sp.tag ? `/?tag=${sp.tag}` : "/");

  return {
    title,
    description,
    alternates: { canonical },
    openGraph: { title, description, url: canonical, type: "website" },
    twitter: { card: "summary", title, description },
    robots: { index: true, follow: true },
  };
}

export default async function HomePage({
  searchParams,
}: {
  searchParams: Promise<{ page?: string; tag?: string }>;
}) {
  const sp = await searchParams;
  const page = parseInt(sp.page || "1", 10);
  const tag = sp.tag;

  const client = await getApiClient({ tags: ["posts"], revalidate: 3600 });
  const { data, error } = await client.GET("/api/posts", {
    params: {
      query: {
        page,
        pageSize: 10,
        tag,
      },
    },
  });

  if (error || !data) {
    throw new Error("Failed to load posts");
  }

  const heading = tag ? "Posts tagged: " + tag : "Latest Posts";

  return (
    <PostList
      posts={data.items}
      heading={heading}
      page={page}
      hasNextPage={data.items.length === 10}
      tagSlug={tag}
    />
  );
}
