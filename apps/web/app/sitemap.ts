import type { MetadataRoute } from "next";
import { env } from "@/lib/env";

interface PostSummary {
  slug: string;
  publishedAt?: string;
}

interface TagSummary {
  slug: string;
}

async function fetchJson<T>(url: string): Promise<T | null> {
  try {
    const res = await fetch(url, { next: { revalidate: 3600 } });
    if (!res.ok) {
      return null;
    }
    return (await res.json()) as T;
  } catch {
    return null;
  }
}

export default async function sitemap(): Promise<MetadataRoute.Sitemap> {
  const siteUrl = env.siteUrl;

  const staticRoutes: MetadataRoute.Sitemap = [
    { url: siteUrl, lastModified: new Date(), changeFrequency: "daily", priority: 1 },
    { url: siteUrl + "/tags", lastModified: new Date(), changeFrequency: "weekly", priority: 0.8 },
  ];

  const [postsData, tags] = await Promise.all([
    fetchJson<{ items: PostSummary[] }>(env.API_URL + "/api/posts?page=1&pageSize=500"),
    fetchJson<TagSummary[]>(env.API_URL + "/api/tags"),
  ]);

  const posts = postsData?.items ?? [];

  const postRoutes: MetadataRoute.Sitemap = posts.map((post) => ({
    url: siteUrl + "/" + post.slug,
    lastModified: post.publishedAt ? new Date(post.publishedAt) : new Date(),
    changeFrequency: "weekly" as const,
    priority: 0.9,
  }));

  const tagRoutes: MetadataRoute.Sitemap = (tags ?? []).map((tag) => ({
    url: siteUrl + "/tags/" + tag.slug,
    lastModified: new Date(),
    changeFrequency: "weekly" as const,
    priority: 0.7,
  }));

  return [...staticRoutes, ...postRoutes, ...tagRoutes];
}
