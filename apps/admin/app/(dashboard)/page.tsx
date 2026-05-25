import Link from "next/link";
import { env } from "@/lib/env";
import { getApiClient } from "@/lib/api/client";
import { buttonVariants } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";

export default async function DashboardPage() {
  const client = await getApiClient();

  const [postsResult, tagsResult] = await Promise.all([
    client.GET("/api/posts", { params: { query: { page: 1, pageSize: 500 } } }),
    client.GET("/api/tags"),
  ]);

  if (postsResult.error || !postsResult.data) {
    throw new Error("Failed to load posts");
  }
  if (tagsResult.error || !tagsResult.data) {
    throw new Error("Failed to load tags");
  }

  const posts = postsResult.data;
  const tags = tagsResult.data;

  const draftCount = posts.items.filter((p) => p.postState === "Draft").length;
  const publishedCount = posts.items.filter((p) => p.postState === "Published").length;
  const archivedCount = posts.items.filter((p) => p.postState === "Archived").length;

  const stats = [
    { label: "Total posts", value: posts.totalCount, description: "All posts in the system" },
    { label: "Published", value: publishedCount, description: "Live on the public site" },
    { label: "Drafts", value: draftCount, description: "Work in progress" },
    { label: "Tags", value: tags.length, description: "Topic labels" },
  ] as const;

  return (
    <section className="space-y-8">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <div>
          <h1 className="font-heading text-3xl font-semibold tracking-tight">Dashboard</h1>
          <p className="text-sm text-muted-foreground">Overview of your blog content.</p>
        </div>
        <Link href="/posts/new" className={buttonVariants()}>
          New post
        </Link>
      </div>

      <div className="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
        {stats.map((stat) => (
          <Card key={stat.label}>
            <CardHeader>
              <CardDescription>{stat.label}</CardDescription>
              <CardTitle className="text-3xl tabular-nums">{stat.value}</CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-sm text-muted-foreground">{stat.description}</p>
            </CardContent>
          </Card>
        ))}
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Environment</CardTitle>
          <CardDescription>Local development connection details.</CardDescription>
        </CardHeader>
        <CardContent className="space-y-1 text-sm text-muted-foreground">
          <p>Archived posts: {archivedCount}</p>
          <p>API: {env.API_URL}</p>
        </CardContent>
      </Card>
    </section>
  );
}
