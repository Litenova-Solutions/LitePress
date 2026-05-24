import type { Metadata } from "next";
import type { components } from "@litepress/api-types";
import { notFound } from "next/navigation";
import { PostArticle } from "@/domain/posts/view-post-by-slug/PostArticle";
import { excerptFromContent } from "@/shared/prosemirror/renderContent";
import { getApiClient } from "@/lib/api/client";
import { env } from "@/lib/env";

export const dynamic = "force-dynamic";

type PostDetail = components["schemas"]["PostDetailResult"];

async function fetchPost(slug: string): Promise<PostDetail | null> {
  const client = await getApiClient({ tags: ["posts", slug], revalidate: 3600 });
  const { data, error, response } = await client.GET("/api/posts/{slug}", {
    params: { path: { slug } },
  });

  if (response.status === 404) {
    return null;
  }
  if (error || !data) {
    throw new Error("Failed to load post");
  }

  return data;
}

export async function generateMetadata({
  params,
}: {
  params: Promise<{ slug: string }>;
}): Promise<Metadata> {
  const { slug } = await params;
  const post = await fetchPost(slug);
  if (!post || post.postState !== "Published") {
    return { title: "Not Found" };
  }

  const description = post.excerpt || excerptFromContent(post.content);
  const canonical = env.siteUrl + "/" + post.slug;

  return {
    title: post.title,
    description,
    alternates: { canonical },
    openGraph: {
      title: post.title,
      description,
      url: canonical,
      type: "article",
      publishedTime: post.publishedAt ?? undefined,
      images: post.coverImageUrl ? [{ url: post.coverImageUrl }] : undefined,
    },
    twitter: {
      card: "summary_large_image",
      title: post.title,
      description,
      images: post.coverImageUrl ? [post.coverImageUrl] : undefined,
    },
    robots: { index: true, follow: true },
  };
}

export default async function PostPage({
  params,
}: {
  params: Promise<{ slug: string }>;
}) {
  const { slug } = await params;
  const post = await fetchPost(slug);

  if (!post || post.postState !== "Published") {
    notFound();
  }

  const jsonLd = {
    "@context": "https://schema.org",
    "@type": "BlogPosting",
    headline: post.title,
    datePublished: post.publishedAt,
    author: { "@type": "Person", name: post.authorDisplayName },
    description: post.excerpt || excerptFromContent(post.content),
    mainEntityOfPage: env.siteUrl + "/" + post.slug,
  };

  return (
    <>
      <script
        type="application/ld+json"
        dangerouslySetInnerHTML={{ __html: JSON.stringify(jsonLd) }}
      />
      <PostArticle post={post} />
    </>
  );
}
