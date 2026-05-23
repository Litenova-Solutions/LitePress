import { renderProseMirrorToHtml } from "@/shared/prosemirror/renderContent";
import { GiscusComments } from "./GiscusComments";

export interface PostDetail {
  postId: string;
  title: string;
  slug: string;
  content: string;
  excerpt?: string;
  coverImageUrl?: string;
  state: string;
  publishedAt?: string;
  authorName: string;
  tags: Array<{ tagId: string; tagName: string; tagSlug: string }>;
}

interface PostArticleProps {
  post: PostDetail;
}

export function PostArticle({ post }: PostArticleProps) {
  const html = renderProseMirrorToHtml(post.content);

  return (
    <article>
      {post.coverImageUrl && (
        <img
          src={post.coverImageUrl}
          alt={post.title}
          className="w-full h-64 object-cover rounded mb-8"
          fetchPriority="high"
        />
      )}
      <h1 className="text-3xl font-bold mb-4">{post.title}</h1>
      <div className="flex gap-4 text-sm text-gray-500 mb-6">
        <span>{post.authorName}</span>
        {post.publishedAt && (
          <time dateTime={post.publishedAt}>
            {new Date(post.publishedAt).toLocaleDateString()}
          </time>
        )}
      </div>
      <div
        className="prose prose-lg max-w-none"
        dangerouslySetInnerHTML={{ __html: html }}
      />
      {post.tags?.length > 0 && (
        <div className="flex gap-2 mt-8 pt-8 border-t">
          {post.tags.map((tag) => (
            <a
              key={tag.tagId}
              href={"/tags/" + tag.tagSlug}
              className="text-xs bg-gray-100 text-gray-600 px-2 py-1 rounded hover:bg-gray-200"
            >
              {tag.tagName}
            </a>
          ))}
        </div>
      )}
      <GiscusComments slug={post.slug} />
    </article>
  );
}
