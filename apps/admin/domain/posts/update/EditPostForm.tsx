"use client";
// Post editing requires client-side TipTap and api-proxy mutations.

import { useState, useEffect, use } from "react";
import { useRouter } from "next/navigation";
import { TipTapEditor } from "../create/TipTapEditor";

interface Post {
  postId: string;
  title: string;
  slug: string;
  content: string;
  excerpt?: string;
  coverImageUrl?: string;
  state: string;
  tags: Array<{ tagId: string; tagName: string; tagSlug: string }>;
}

interface TagOption {
  tagId: string;
  name: string;
}

interface EditPostFormProps {
  params: Promise<{ id: string }>;
}

export function EditPostForm({ params }: EditPostFormProps) {
  const { id } = use(params);
  const router = useRouter();
  const [post, setPost] = useState<Post | null>(null);
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
    ]).then(([postData, tagsData]: [Post, TagOption[]]) => {
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
      router.refresh();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Unknown error");
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
        router.push("/posts");
      } else {
        const res = await fetch("/api-proxy/posts/" + id + "/" + action, {
          method: "POST",
        });
        if (!res.ok) {
          throw new Error(await res.text());
        }
        router.refresh();
        window.location.reload();
      }
    } catch (err) {
      alert(err instanceof Error ? err.message : "Unknown error");
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
      alert(await res.text());
      return;
    }
    const updated = (await fetch("/api-proxy/posts/" + id).then((r) =>
      r.json()
    )) as Post;
    setPost(updated);
  }

  if (!post) {
    return <div className="text-gray-500">Loading...</div>;
  }

  const isDraft = post.state === "Draft";
  const isPublished = post.state === "Published";
  const isArchived = post.state === "Archived";
  const assignedTagIds = new Set(post.tags?.map((t) => t.tagId) ?? []);

  return (
    <section>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-bold">Edit Post</h1>
        <div className="flex gap-2">
          {isDraft && (
            <button
              type="button"
              onClick={() => handleAction("publish")}
              className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700 text-sm"
            >
              Publish
            </button>
          )}
          {isPublished && (
            <button
              type="button"
              onClick={() => handleAction("archive")}
              className="bg-gray-600 text-white px-4 py-2 rounded hover:bg-gray-700 text-sm"
            >
              Archive
            </button>
          )}
          {(isDraft || isArchived) && (
            <button
              type="button"
              onClick={() => handleAction("delete")}
              className="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700 text-sm"
            >
              Delete
            </button>
          )}
        </div>
      </div>
      <div className="mb-4">
        <span
          className={
            "inline-block px-2 py-1 rounded text-xs font-semibold " +
            (post.state === "Published"
              ? "bg-green-100 text-green-800"
              : post.state === "Archived"
                ? "bg-gray-100 text-gray-600"
                : "bg-yellow-100 text-yellow-800")
          }
        >
          {post.state}
        </span>
      </div>
      <form onSubmit={handleUpdate} className="max-w-2xl space-y-4">
        {error && <div className="bg-red-50 text-red-600 p-3 rounded">{error}</div>}
        <div>
          <label className="block text-sm font-medium mb-1">Title *</label>
          <input
            required
            value={form.title}
            onChange={(e) => setForm((f) => ({ ...f, title: e.target.value }))}
            className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Content *</label>
          <TipTapEditor
            value={form.content}
            onChange={(content) => setForm((f) => ({ ...f, content }))}
          />
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Excerpt</label>
          <textarea
            rows={3}
            value={form.excerpt}
            onChange={(e) => setForm((f) => ({ ...f, excerpt: e.target.value }))}
            className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Cover Image URL</label>
          <input
            type="url"
            value={form.coverImageUrl}
            onChange={(e) => setForm((f) => ({ ...f, coverImageUrl: e.target.value }))}
            className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        {isDraft && (
          <button
            type="submit"
            disabled={loading}
            className="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700 disabled:opacity-50"
          >
            {loading ? "Saving..." : "Save Changes"}
          </button>
        )}
      </form>
      {isDraft && allTags.length > 0 && (
        <div className="mt-8 max-w-2xl">
          <h2 className="text-lg font-semibold mb-3">Tags</h2>
          <div className="flex flex-wrap gap-2">
            {allTags.map((tag) => {
              const assigned = assignedTagIds.has(tag.tagId);
              return (
                <button
                  key={tag.tagId}
                  type="button"
                  onClick={() => toggleTag(tag.tagId, assigned)}
                  className={
                    "text-sm px-3 py-1 rounded border " +
                    (assigned
                      ? "bg-blue-600 text-white border-blue-600"
                      : "bg-white text-gray-700 border-gray-300 hover:border-blue-400")
                  }
                >
                  {tag.name}
                </button>
              );
            })}
          </div>
        </div>
      )}
    </section>
  );
}
