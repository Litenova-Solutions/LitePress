import type { Metadata } from "next";
import { TagsIndex } from "@/domain/tags/list-tags/TagsIndex";
import { getApiClient } from "@/lib/api/client";
import { env } from "@/lib/env";

export const dynamic = "force-dynamic";

export const metadata: Metadata = {
  title: "Tags",
  description: "Browse posts by tag.",
  alternates: { canonical: env.siteUrl + "/tags" },
  openGraph: {
    title: "Tags",
    description: "Browse posts by tag.",
    url: env.siteUrl + "/tags",
    type: "website",
  },
  twitter: {
    card: "summary",
    title: "Tags",
    description: "Browse posts by tag.",
  },
  robots: { index: true, follow: true },
};

export default async function TagsPage() {
  const client = await getApiClient({ tags: ["tags"], revalidate: 3600 });
  const { data, error } = await client.GET("/api/tags");

  if (error || !data) {
    throw new Error("Failed to load tags");
  }

  return <TagsIndex tags={data} />;
}
