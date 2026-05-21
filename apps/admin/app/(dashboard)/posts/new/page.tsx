"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";

interface Tag { tagId: string; name: string; }

export default function NewPostPage() {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [form, setForm] = useState({ title: "", content: "", excerpt: "", coverImageUrl: "" });

  async function handleSubmit(e) {
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
      if (!res.ok) throw new Error(await res.text());
      const data = await res.json();
      router.push("/posts/" + data.postId);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Unknown error");
    } finally {
      setLoading(false);
    }
  }

  return (
    <section>
      <h1 className="text-2xl font-bold mb-6">New Post</h1>
      <form onSubmit={handleSubmit} className="max-w-2xl space-y-4">
        {error && <div className="bg-red-50 text-red-600 p-3 rounded">{error}</div>}
        <div>
          <label className="block text-sm font-medium mb-1">Title *</label>
          <input required value={form.title} onChange={e => setForm(f => ({ ...f, title: e.target.value }))}
            className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500" />
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Content *</label>
          <textarea required rows={14} value={form.content}
            onChange={e => setForm(f => ({ ...f, content: e.target.value }))}
            className="w-full border rounded px-3 py-2 font-mono text-sm focus:outline-none focus:ring-2 focus:ring-blue-500" />
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Excerpt</label>
          <textarea rows={3} value={form.excerpt} onChange={e => setForm(f => ({ ...f, excerpt: e.target.value }))}
            className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500" />
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Cover Image URL</label>
          <input type="url" value={form.coverImageUrl} onChange={e => setForm(f => ({ ...f, coverImageUrl: e.target.value }))}
            className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500" />
        </div>
        <button type="submit" disabled={loading}
          className="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700 disabled:opacity-50">
          {loading ? "Creating..." : "Create Post"}
        </button>
      </form>
    </section>
  );
}