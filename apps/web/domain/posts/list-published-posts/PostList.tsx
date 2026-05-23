import type { components } from "@litenova/api-types";
import Link from "next/link";

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
    <section>
      <h1 className="text-3xl font-bold mb-8">{heading}</h1>
      <div className="space-y-8">
        {posts.map((post) => (
          <article key={post.postId} className="border-b pb-8">
            <h2 className="text-xl font-semibold mb-2">
              <Link href={"/" + post.slug} className="hover:text-blue-600">
                {post.title}
              </Link>
            </h2>
            {post.excerpt && <p className="text-gray-600 mb-3">{post.excerpt}</p>}
            <div className="flex gap-2 flex-wrap">
              {post.tags?.map((tag) => (
                <Link
                  key={tag.tagId}
                  href={"/tags/" + tag.slug}
                  className="text-xs bg-gray-100 text-gray-600 px-2 py-1 rounded hover:bg-gray-200"
                >
                  {tag.name}
                </Link>
              ))}
            </div>
            {post.publishedAt && (
              <time dateTime={post.publishedAt} className="text-sm text-gray-400 mt-2 block">
                {new Date(post.publishedAt).toLocaleDateString()}
              </time>
            )}
          </article>
        ))}
      </div>
      {posts.length === 0 && (
        <p className="text-gray-500 text-center py-12">No posts yet.</p>
      )}
      <nav className="flex justify-between mt-8" aria-label="Pagination">
        {page > 1 && (
          <Link
            href={"/?page=" + (page - 1) + tagQuery}
            className="text-blue-600 hover:underline"
          >
            Previous
          </Link>
        )}
        {hasNextPage && (
          <Link
            href={"/?page=" + (page + 1) + tagQuery}
            className="text-blue-600 hover:underline ml-auto"
          >
            Next
          </Link>
        )}
      </nav>
    </section>
  );
}
