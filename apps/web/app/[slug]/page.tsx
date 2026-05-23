import type { Metadata } from "next";
import { notFound } from "next/navigation";
import { PostArticle, type PostDetail } from "@/domain/posts/view-post-by-slug/PostArticle";
import { excerptFromContent } from "@/shared/prosemirror/renderContent";
import { env } from "@/lib/env";

export const dynamic = "force-dynamic";

async function fetchPost(slug: string): Promise<PostDetail | null> {
  const res = await fetch(env.API_URL + "/api/posts/" + slug, {
    next: { tags: ["posts", slug], revalidate: 3600 },
  });

  if (res.status === 404) {
    return null;
  }
  if (!res.ok) {
    throw new Error("Failed to load post");
  }

  return res.json() as Promise<PostDetail>;
}

export async function generateMetadata({
  params,
}: {
  params: Promise<{ slug: string }>;
}): Promise<Metadata> {
  const { slug } = await params;
  const post = await fetchPost(slug);
  if (!post || post.state !== "Published") {
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
      publishedTime: post.publishedAt,
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

  if (!post || post.state !== "Published") {
    notFound();
  }

  const jsonLd = {
    "@context": "https://schema.org",
    "@type": "BlogPosting",
    headline: post.title,
    datePublished: post.publishedAt,
    author: { "@type": "Person", name: post.authorName },
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
