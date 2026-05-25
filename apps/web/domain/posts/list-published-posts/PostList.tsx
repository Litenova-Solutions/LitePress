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
}

export function PostList({ posts, heading, page, hasNextPage, tagSlug }: PostListProps) {
  const tagQuery = tagSlug ? "&tag=" + tagSlug : "";

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
          <CardContent className="py-12 text-center text-muted-foreground">No posts yet.</CardContent>
        </Card>
      )}

      <nav className="flex justify-between" aria-label="Pagination">
        {page > 1 ? (
          <Link href={"/?page=" + (page - 1) + tagQuery} className={buttonVariants({ variant: "outline" })}>
            Previous
          </Link>
        ) : (
          <span />
        )}
        {hasNextPage && (
          <Link
            href={"/?page=" + (page + 1) + tagQuery}
            className={cn(buttonVariants({ variant: "outline" }), "ml-auto")}
          >
            Next
          </Link>
        )}
      </nav>
    </section>
  );
}
