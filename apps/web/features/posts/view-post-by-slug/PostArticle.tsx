import type { components } from "@litepress/api-types";
import Link from "next/link";
import { Badge } from "@/components/ui/badge";
import { Card, CardContent } from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import { renderProseMirrorToHtml } from "@/shared/prosemirror/renderContent";
import { GiscusComments } from "./GiscusComments";

export type PostDetail = components["schemas"]["PostDetailResult"];

interface PostArticleProps {
  post: PostDetail;
}

export function PostArticle({ post }: PostArticleProps) {
  const html = renderProseMirrorToHtml(post.content);

  return (
    <article className="space-y-8">
      {post.coverImageUrl && (
        <img
          src={post.coverImageUrl}
          alt={post.title}
          className="h-64 w-full rounded-xl object-cover ring-1 ring-foreground/10"
          fetchPriority="high"
        />
      )}

      <header className="space-y-4">
        <h1 className="font-heading text-4xl font-semibold tracking-tight">{post.title}</h1>
        <div className="flex flex-wrap gap-4 text-sm text-muted-foreground">
          <span>{post.authorDisplayName}</span>
          {post.publishedAt && (
            <time dateTime={post.publishedAt}>
              {new Date(post.publishedAt).toLocaleDateString()}
            </time>
          )}
        </div>
      </header>

      <Card>
        <CardContent className="pt-6">
          <div
            className="prose prose-lg max-w-none dark:prose-invert"
            dangerouslySetInnerHTML={{ __html: html }}
          />
        </CardContent>
      </Card>

      {post.tags?.length > 0 && (
        <div className="flex flex-wrap gap-2">
          {post.tags.map((tag) => (
            <Link key={tag.tagId} href={"/tags/" + tag.slug}>
              <Badge variant="outline">{tag.name}</Badge>
            </Link>
          ))}
        </div>
      )}

      <Separator />
      <GiscusComments slug={post.slug} />
    </article>
  );
}
