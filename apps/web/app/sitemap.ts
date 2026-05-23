import type { MetadataRoute } from "next";
import { getApiClient } from "@/lib/api/client";
import { env } from "@/lib/env";

export const dynamic = "force-dynamic";

export default async function sitemap(): Promise<MetadataRoute.Sitemap> {
  const siteUrl = env.siteUrl;

  const staticRoutes: MetadataRoute.Sitemap = [
    { url: siteUrl, lastModified: new Date(), changeFrequency: "daily", priority: 1 },
    { url: siteUrl + "/tags", lastModified: new Date(), changeFrequency: "weekly", priority: 0.8 },
  ];

  try {
    const client = await getApiClient({ revalidate: 3600 });

    const [postsResult, tagsResult] = await Promise.all([
      client.GET("/api/posts", { params: { query: { page: 1, pageSize: 500 } } }),
      client.GET("/api/tags"),
    ]);

    const posts = postsResult.data?.items ?? [];
    const tags = tagsResult.data ?? [];

    const postRoutes: MetadataRoute.Sitemap = posts.map((post) => ({
      url: siteUrl + "/" + post.slug,
      lastModified: post.publishedAt ? new Date(post.publishedAt) : new Date(),
      changeFrequency: "weekly" as const,
      priority: 0.9,
    }));

    const tagRoutes: MetadataRoute.Sitemap = tags.map((tag) => ({
      url: siteUrl + "/tags/" + tag.slug,
      lastModified: new Date(),
      changeFrequency: "weekly" as const,
      priority: 0.7,
    }));

    return [...staticRoutes, ...postRoutes, ...tagRoutes];
  } catch {
    return staticRoutes;
  }
}
