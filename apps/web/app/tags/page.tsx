import type { Metadata } from "next";
import { TagsIndex } from "@/domain/tags/list-tags/TagsIndex";
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

interface TagSummary {
  tagId: string;
  name: string;
  slug: string;
  postCount: number;
}

export default async function TagsPage() {
  const tags = (await fetch(env.API_URL + "/api/tags", {
    next: { tags: ["tags"], revalidate: 3600 },
  }).then((r) => r.json())) as TagSummary[];

  return <TagsIndex tags={tags} />;
}
