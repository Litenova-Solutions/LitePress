import type { components } from "@litepress/api-types";
import Link from "next/link";
import { Badge } from "@/components/ui/badge";
import { buttonVariants } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { cn } from "@/lib/utils";

export type PostSummary = components["schemas"]["PostSummaryResult"];

interface PostListProps {
  posts: PostSummary[];
  heading: string;
  page: number;
  hasNextPage: boolean;
  tagSlug?: string;
  /** Base path for pagination links. Defaults to `/` (home). Use `/tags/{slug}` on tag routes. */
  paginationBase?: string;
}

function buildPageHref(basePath: string, page: number, tagSlug?: string): string {
  const params = new URLSearchParams();
  if (page > 1) {
    params.set("page", String(page));
  }
  if (tagSlug && basePath === "/") {
    params.set("tag", tagSlug);
  }
  const query = params.toString();
  return query ? basePath + "?" + query : basePath;
}

export function PostList({
  posts,
  heading,
  page,
  hasNextPage,
  tagSlug,
  paginationBase = "/",
}: PostListProps) {
  const emptyMessage =
    tagSlug || paginationBase.startsWith("/tags/")
      ? "No posts with this tag."
      : "No posts yet.";

  return (
    <section className="space-y-8">
      <div>
        <h1 className="font-heading text-3xl font-semibold tracking-tight">{heading}</h1>
        <p className="mt-1 text-sm text-muted-foreground">Published posts from LitePress.</p>
      </div>

      <div className="space-y-4">
        {posts.map((post) => (
          <Card key={post.postId}>
            <CardHeader>
              <CardTitle className="text-xl">
                <Link href={"/" + post.slug} className="hover:underline">
                  {post.title}
                </Link>
              </CardTitle>
              {post.excerpt && <CardDescription>{post.excerpt}</CardDescription>}
            </CardHeader>
            <CardContent className="space-y-3">
              {post.tags && post.tags.length > 0 && (
                <div className="flex flex-wrap gap-2">
                  {post.tags.map((tag) => (
                    <Link key={tag.tagId} href={"/tags/" + tag.slug}>
                      <Badge variant="secondary">{tag.name}</Badge>
                    </Link>
                  ))}
                </div>
              )}
              {post.publishedAt && (
                <time dateTime={post.publishedAt} className="block text-sm text-muted-foreground">
                  {new Date(post.publishedAt).toLocaleDateString()}
                </time>
              )}
            </CardContent>
          </Card>
        ))}
      </div>

      {posts.length === 0 && (
        <Card>
          <CardContent className="py-12 text-center text-muted-foreground">{emptyMessage}</CardContent>
        </Card>
      )}

      <nav className="flex justify-between" aria-label="Pagination">
        {page > 1 ? (
          <Link
            href={buildPageHref(paginationBase, page - 1, tagSlug)}
            className={buttonVariants({ variant: "outline" })}
          >
            Previous
          </Link>
        ) : (
          <span />
        )}
        {hasNextPage && (
          <Link
            href={buildPageHref(paginationBase, page + 1, tagSlug)}
            className={cn(buttonVariants({ variant: "outline" }), "ml-auto")}
          >
            Next
          </Link>
        )}
      </nav>
    </section>
  );
}
