"use client";
// Giscus embed requires browser APIs and third-party script loading.

import Giscus from "@giscus/react";
import { env } from "@/lib/env";

interface GiscusCommentsProps {
  slug: string;
}

export function GiscusComments({ slug }: GiscusCommentsProps) {
  const repo = env.NEXT_PUBLIC_GISCUS_REPO;
  const repoId = env.NEXT_PUBLIC_GISCUS_REPO_ID;
  const categoryId = env.NEXT_PUBLIC_GISCUS_CATEGORY_ID;

  if (!repo || !repoId || !categoryId) {
    return null;
  }

  return (
    <section aria-label="Comments" className="mt-12 pt-8 border-t">
      <Giscus
        id={"comments-" + slug}
        repo={repo as `${string}/${string}`}
        repoId={repoId}
        category="Announcements"
        categoryId={categoryId}
        mapping="pathname"
        term={"/" + slug}
        reactionsEnabled="1"
        emitMetadata="0"
        inputPosition="top"
        theme="light"
        lang="en"
        loading="lazy"
      />
    </section>
  );
}
