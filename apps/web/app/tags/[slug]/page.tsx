import type { Metadata } from "next";
import { notFound } from "next/navigation";
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
}: {
  params: Promise<{ slug: string }>;
}) {
  const { slug } = await params;
  const client = await getApiClient({
    tags: ["posts", "tag-" + slug],
    revalidate: 3600,
  });

  const { data, error, response } = await client.GET("/api/posts", {
    params: {
      query: {
        tag: slug,
        page: 1,
        pageSize: 20,
      },
    },
  });

  if (response.status === 404 || error || !data) {
    notFound();
  }

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
