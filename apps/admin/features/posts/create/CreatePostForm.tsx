"use client";
// Form mutations require client interactivity and api-proxy calls.

import { useState } from "react";
import { useRouter } from "next/navigation";
import { toast } from "sonner";
import { TipTapEditor } from "./TipTapEditor";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { readApiErrorMessage } from "@/lib/api/errors";

export function CreatePostForm() {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [form, setForm] = useState({
    title: "",
    content: "",
    excerpt: "",
    coverImageUrl: "",
  });

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);
    setError(null);
    try {
      const res = await fetch("/api-proxy/posts", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          title: form.title,
          content: form.content,
          excerpt: form.excerpt || null,
          coverImageUrl: form.coverImageUrl || null,
          tagIds: [],
        }),
      });
      if (!res.ok) {
        throw new Error(await readApiErrorMessage(res));
      }
      const data = (await res.json()) as { postId: string };
      toast.success("Post created");
      router.push("/posts/" + data.postId);
    } catch (err) {
      const message = err instanceof Error ? err.message : "Unknown error";
      setError(message);
      toast.error("Failed to create post");
    } finally {
      setLoading(false);
    }
  }

  return (
    <section className="mx-auto max-w-2xl space-y-6">
      <div>
        <h1 className="font-heading text-3xl font-semibold tracking-tight">New post</h1>
        <p className="text-sm text-muted-foreground">Create a draft with title and content.</p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Post details</CardTitle>
          <CardDescription>Slug is generated from the title when you save.</CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit} className="space-y-5">
            {error && (
              <Alert variant="destructive">
                <AlertTitle>Could not create post</AlertTitle>
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

            <Button type="submit" disabled={loading || !form.content.trim()}>
              {loading ? "Creating..." : "Create post"}
            </Button>
          </form>
        </CardContent>
      </Card>
    </section>
  );
}
