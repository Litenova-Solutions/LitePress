import Link from "next/link";

export interface TagSummary {
  tagId: string;
  name: string;
  slug: string;
  postCount: number;
}

interface TagsIndexProps {
  tags: TagSummary[];
}

export function TagsIndex({ tags }: TagsIndexProps) {
  return (
    <section>
      <h1 className="text-3xl font-bold mb-8">Tags</h1>
      <ul className="flex flex-wrap gap-3">
        {tags.map((tag) => (
          <li key={tag.tagId}>
            <Link
              href={"/tags/" + tag.slug}
              className="inline-block bg-gray-100 text-gray-700 px-3 py-1 rounded hover:bg-gray-200"
            >
              {tag.name}
              <span className="text-gray-400 ml-1">({tag.postCount})</span>
            </Link>
          </li>
        ))}
      </ul>
      {tags.length === 0 && (
        <p className="text-gray-500 text-center py-12">No tags yet.</p>
      )}
    </section>
  );
}
