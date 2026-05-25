import type { components } from "@litepress/api-types";
import Link from "next/link";
import { Badge } from "@/components/ui/badge";
import { Card, CardContent } from "@/components/ui/card";

export type TagSummary = components["schemas"]["TagResult"];

interface TagsIndexProps {
  tags: TagSummary[];
}

export function TagsIndex({ tags }: TagsIndexProps) {
  return (
    <section className="space-y-8">
      <div>
        <h1 className="font-heading text-3xl font-semibold tracking-tight">Tags</h1>
        <p className="mt-1 text-sm text-muted-foreground">Browse posts by topic.</p>
      </div>

      {tags.length === 0 ? (
        <Card>
          <CardContent className="py-12 text-center text-muted-foreground">No tags yet.</CardContent>
        </Card>
      ) : (
        <ul className="flex flex-wrap gap-3">
          {tags.map((tag) => (
            <li key={tag.tagId}>
              <Link href={"/tags/" + tag.slug}>
                <Badge variant="secondary" className="px-3 py-1 text-sm">
                  {tag.name}
                  <span className="ml-1 text-muted-foreground">({tag.postCount})</span>
                </Badge>
              </Link>
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}
