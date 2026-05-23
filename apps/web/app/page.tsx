import type { Metadata } from "next";
import { PostList } from "@/domain/posts/list-published-posts/PostList";
import { env } from "@/lib/env";

export const dynamic = "force-dynamic";

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

export async function generateMetadata({
  searchParams,
}: {
  searchParams: Promise<{ tag?: string }>;
}): Promise<Metadata> {
  const sp = await searchParams;
  const title = sp.tag ? `Posts tagged: ${sp.tag}` : "Latest Posts";
  const description = "Published posts from LiteNova Blog.";
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

  const url = tag
    ? env.API_URL + "/api/posts?tag=" + tag + "&page=" + page + "&pageSize=10"
    : env.API_URL + "/api/posts?page=" + page + "&pageSize=10";

  const data = (await fetch(url, {
    next: { tags: ["posts"], revalidate: 3600 },
  }).then((r) => r.json())) as PagedResult;

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
