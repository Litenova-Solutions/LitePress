"use client";
// Post editing requires client-side TipTap and api-proxy mutations.

import type { components } from "@litepress/api-types";
import { useState, useEffect, use } from "react";
import { useRouter } from "next/navigation";
import { toast } from "sonner";
import { TipTapEditor } from "../create/TipTapEditor";
import { PostStatusBadge } from "@/components/PostStatusBadge";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Badge } from "@/components/ui/badge";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { cn } from "@/lib/utils";

type PostDetail = components["schemas"]["PostDetailResult"];
type TagOption = components["schemas"]["TagResult"];

interface EditPostFormProps {
  params: Promise<{ id: string }>;
}

export function EditPostForm({ params }: EditPostFormProps) {
  const { id } = use(params);
  const router = useRouter();
  const [post, setPost] = useState<PostDetail | null>(null);
  const [allTags, setAllTags] = useState<TagOption[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [form, setForm] = useState({
    title: "",
    content: "",
    excerpt: "",
    coverImageUrl: "",
  });

  useEffect(() => {
    Promise.all([
      fetch("/api-proxy/posts/" + id).then((r) => r.json()),
      fetch("/api-proxy/tags").then((r) => r.json()),
    ]).then(([postData, tagsData]: [PostDetail, TagOption[]]) => {
      setPost(postData);
      setAllTags(tagsData);
      setForm({
        title: postData.title,
        content: postData.content,
        excerpt: postData.excerpt || "",
        coverImageUrl: postData.coverImageUrl || "",
      });
    });
  }, [id]);

  async function handleUpdate(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);
    setError(null);
    try {
      const res = await fetch("/api-proxy/posts/" + id, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(form),
      });
      if (!res.ok) {
        throw new Error(await res.text());
      }
      toast.success("Post saved");
      router.refresh();
    } catch (err) {
      const message = err instanceof Error ? err.message : "Unknown error";
      setError(message);
      toast.error("Failed to save post");
    } finally {
      setLoading(false);
    }
  }

  async function handleAction(action: "publish" | "archive" | "delete") {
    if (!confirm("Are you sure?")) {
      return;
    }
    try {
      if (action === "delete") {
        const res = await fetch("/api-proxy/posts/" + id, { method: "DELETE" });
        if (!res.ok) {
          throw new Error(await res.text());
        }
        toast.success("Post deleted");
        router.push("/posts");
      } else {
        const res = await fetch("/api-proxy/posts/" + id + "/" + action, {
          method: "POST",
        });
        if (!res.ok) {
          throw new Error(await res.text());
        }
        toast.success(action === "publish" ? "Post published" : "Post archived");
        router.refresh();
        window.location.reload();
      }
    } catch (err) {
      toast.error(err instanceof Error ? err.message : "Action failed");
    }
  }

  async function toggleTag(tagId: string, assigned: boolean) {
    const method = assigned ? "DELETE" : "POST";
    const path = assigned
      ? "/api-proxy/posts/" + id + "/tags/" + tagId
      : "/api-proxy/posts/" + id + "/tags";
    const res = await fetch(path, {
      method,
      headers: assigned ? undefined : { "Content-Type": "application/json" },
      body: assigned ? undefined : JSON.stringify({ tagId }),
    });
    if (!res.ok) {
      toast.error(await res.text());
      return;
    }
    const updated = (await fetch("/api-proxy/posts/" + id).then((r) =>
      r.json()
    )) as PostDetail;
    setPost(updated);
  }

  if (!post) {
    return <p className="text-sm text-muted-foreground">Loading...</p>;
  }

  const isDraft = post.postState === "Draft";
  const isPublished = post.postState === "Published";
  const isArchived = post.postState === "Archived";
  const assignedTagIds = new Set(post.tags?.map((t) => t.tagId) ?? []);

  return (
    <section className="mx-auto max-w-2xl space-y-6">
      <div className="flex flex-wrap items-start justify-between gap-4">
        <div className="space-y-2">
          <h1 className="font-heading text-3xl font-semibold tracking-tight">Edit post</h1>
          <PostStatusBadge status={post.postState} />
        </div>
        <div className="flex flex-wrap gap-2">
          {isDraft && (
            <Button type="button" onClick={() => handleAction("publish")}>
              Publish
            </Button>
          )}
          {isPublished && (
            <Button type="button" variant="secondary" onClick={() => handleAction("archive")}>
              Archive
            </Button>
          )}
          {(isDraft || isArchived) && (
            <Button type="button" variant="destructive" onClick={() => handleAction("delete")}>
              Delete
            </Button>
          )}
        </div>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Post details</CardTitle>
          <CardDescription>Update title, content, and metadata.</CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleUpdate} className="space-y-5">
            {error && (
              <Alert variant="destructive">
                <AlertTitle>Could not save post</AlertTitle>
                <AlertDescription>{error}</AlertDescription>
              </Alert>
            )}

            <div className="space-y-2">
              <Label htmlFor="title">Title</Label>
              <Input
                id="title"
                required
                value={form.title}
                onChange={(e) => setForm((f) => ({ ...f, title: e.target.value }))}
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="content">Content</Label>
              <TipTapEditor
                value={form.content}
                onChange={(content) => setForm((f) => ({ ...f, content }))}
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="excerpt">Excerpt</Label>
              <Textarea
                id="excerpt"
                rows={3}
                value={form.excerpt}
                onChange={(e) => setForm((f) => ({ ...f, excerpt: e.target.value }))}
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="coverImageUrl">Cover image URL</Label>
              <Input
                id="coverImageUrl"
                type="url"
                value={form.coverImageUrl}
                onChange={(e) => setForm((f) => ({ ...f, coverImageUrl: e.target.value }))}
              />
            </div>

            {isDraft && (
              <Button type="submit" disabled={loading}>
                {loading ? "Saving..." : "Save changes"}
              </Button>
            )}
          </form>
        </CardContent>
      </Card>

      {isDraft && allTags.length > 0 && (
        <Card>
          <CardHeader>
            <CardTitle>Tags</CardTitle>
            <CardDescription>Assign tags before publishing.</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="flex flex-wrap gap-2">
              {allTags.map((tag) => {
                const assigned = assignedTagIds.has(tag.tagId);
                return (
                  <button
                    key={tag.tagId}
                    type="button"
                    onClick={() => toggleTag(tag.tagId, assigned)}
                    className="rounded-full focus-visible:outline-none focus-visible:ring-3 focus-visible:ring-ring/50"
                  >
                    <Badge
                      variant={assigned ? "default" : "outline"}
                      className={cn("cursor-pointer", assigned && "ring-1 ring-primary/30")}
                    >
                      {tag.name}
                    </Badge>
                  </button>
                );
              })}
            </div>
          </CardContent>
        </Card>
      )}
    </section>
  );
}
