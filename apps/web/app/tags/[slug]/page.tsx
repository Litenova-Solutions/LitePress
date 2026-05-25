import type { Metadata } from "next";
import { PostList } from "@/features/posts/list-published-posts/PostList";
import { getApiClient } from "@/lib/api/client";
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
  searchParams,
}: {
  params: Promise<{ slug: string }>;
  searchParams: Promise<{ page?: string }>;
}) {
  const { slug } = await params;
  const sp = await searchParams;
  const page = parseInt(sp.page || "1", 10);

  const client = await getApiClient({
    tags: ["posts", "tag-" + slug],
    revalidate: 3600,
  });

  const result = await client.GET("/api/posts", {
    params: {
      query: {
        tag: slug,
        page,
        pageSize: 10,
      },
    },
  });

  if (result.error) {
    throw new Error("Failed to load posts");
  }

  const items = result.data?.items ?? [];

  return (
    <PostList
      posts={items}
      heading={"Posts tagged: " + slug}
      page={page}
      hasNextPage={items.length === 10}
      paginationBase={"/tags/" + slug}
    />
  );
}
