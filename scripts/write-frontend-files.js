const fs = require('fs');
const path = require('path');

const adminBase = 'C:/Projects/Blog/apps/admin';
const webBase = 'C:/Projects/Blog/apps/web';

function write(filePath, content) {
  const dir = path.dirname(filePath);
  if (!fs.existsSync(dir)) fs.mkdirSync(dir, { recursive: true });
  fs.writeFileSync(filePath, content, 'utf8');
  console.log('Written:', filePath.replace('C:/Projects/Blog/', ''));
}

// ===== ADMIN PAGES =====

write(`${adminBase}/app/(dashboard)/posts/[id]/page.tsx`, `
"use client";

import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import { use } from "react";

interface Post {
  postId: string;
  title: string;
  slug: string;
  content: string;
  excerpt?: string;
  coverImageUrl?: string;
  state: string;
}

export default function EditPostPage({ params }: { params: Promise<{ id: string }> }) {
  const { id } = use(params);
  const router = useRouter();
  const [post, setPost] = useState<Post | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [form, setForm] = useState({ title: "", content: "", excerpt: "", coverImageUrl: "" });

  useEffect(() => {
    fetch("/api-proxy/posts/" + id)
      .then(r => r.json())
      .then((data: Post) => {
        setPost(data);
        setForm({
          title: data.title,
          content: data.content,
          excerpt: data.excerpt || "",
          coverImageUrl: data.coverImageUrl || "",
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
      if (!res.ok) throw new Error(await res.text());
      router.refresh();
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : "Unknown error");
    } finally {
      setLoading(false);
    }
  }

  async function handleAction(action: "publish" | "archive" | "delete") {
    if (!confirm("Are you sure?")) return;
    try {
      if (action === "delete") {
        const res = await fetch("/api-proxy/posts/" + id, { method: "DELETE" });
        if (!res.ok) throw new Error(await res.text());
        router.push("/posts");
      } else {
        const res = await fetch("/api-proxy/posts/" + id + "/" + action, { method: "POST" });
        if (!res.ok) throw new Error(await res.text());
        router.refresh();
      }
    } catch (err: unknown) {
      alert(err instanceof Error ? err.message : "Unknown error");
    }
  }

  if (!post) return <div className="text-gray-500">Loading...</div>;

  const isDraft = post.state === "Draft";
  const isPublished = post.state === "Published";
  const isArchived = post.state === "Archived";

  return (
    <section>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-bold">Edit Post</h1>
        <div className="flex gap-2">
          {isDraft && (
            <button onClick={() => handleAction("publish")}
              className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700 text-sm">
              Publish
            </button>
          )}
          {isPublished && (
            <button onClick={() => handleAction("archive")}
              className="bg-gray-600 text-white px-4 py-2 rounded hover:bg-gray-700 text-sm">
              Archive
            </button>
          )}
          {(isDraft || isArchived) && (
            <button onClick={() => handleAction("delete")}
              className="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700 text-sm">
              Delete
            </button>
          )}
        </div>
      </div>
      <div className="mb-4">
        <span className={"inline-block px-2 py-1 rounded text-xs font-semibold " + (
          post.state === "Published" ? "bg-green-100 text-green-800" :
          post.state === "Archived" ? "bg-gray-100 text-gray-600" :
          "bg-yellow-100 text-yellow-800"
        )}>{post.state}</span>
      </div>
      <form onSubmit={handleUpdate} className="max-w-2xl space-y-4">
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
          <input type="url" value={form.coverImageUrl}
            onChange={e => setForm(f => ({ ...f, coverImageUrl: e.target.value }))}
            className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500" />
        </div>
        {isDraft && (
          <button type="submit" disabled={loading}
            className="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700 disabled:opacity-50">
            {loading ? "Saving..." : "Save Changes"}
          </button>
        )}
      </form>
    </section>
  );
}
`.trim());

write(`${adminBase}/app/(dashboard)/tags/page.tsx`, `
import { apiGet, apiPost, apiPut, apiDelete } from "../../../lib/api";

interface Tag {
  tagId: string;
  name: string;
  slug: string;
  postCount: number;
}

async function createTag(formData: FormData) {
  "use server";
  const name = formData.get("name") as string;
  await apiPost("/api/tags", { name });
}

async function deleteTag(tagId: string) {
  "use server";
  await apiDelete("/api/tags/" + tagId);
}

export default async function TagsPage() {
  const tags = await apiGet<Tag[]>("/api/tags");

  return (
    <section>
      <h1 className="text-2xl font-bold mb-6">Tags</h1>
      <div className="max-w-2xl">
        <form action={createTag} className="flex gap-2 mb-6">
          <input name="name" required placeholder="Tag name"
            className="flex-1 border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500" />
          <button type="submit"
            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">
            Add Tag
          </button>
        </form>
        <div className="bg-white rounded shadow overflow-hidden">
          <table className="w-full">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-4 py-3 text-left text-sm font-semibold text-gray-600">Name</th>
                <th className="px-4 py-3 text-left text-sm font-semibold text-gray-600">Slug</th>
                <th className="px-4 py-3 text-left text-sm font-semibold text-gray-600">Posts</th>
                <th className="px-4 py-3 text-left text-sm font-semibold text-gray-600">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100">
              {tags.map((tag) => (
                <tr key={tag.tagId} className="hover:bg-gray-50">
                  <td className="px-4 py-3 font-medium">{tag.name}</td>
                  <td className="px-4 py-3 text-gray-500 text-sm">{tag.slug}</td>
                  <td className="px-4 py-3 text-sm">{tag.postCount}</td>
                  <td className="px-4 py-3">
                    <form action={deleteTag.bind(null, tag.tagId)}>
                      <button type="submit"
                        className="text-red-600 hover:underline text-sm"
                        onClick={(e) => { if (!confirm("Delete tag?")) e.preventDefault(); }}>
                        Delete
                      </button>
                    </form>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          {tags.length === 0 && (
            <p className="text-center text-gray-500 py-8">No tags yet.</p>
          )}
        </div>
      </div>
    </section>
  );
}
`.trim());

// ===== ADMIN API PROXY =====
// Server Actions for client-side pages need a proxy to attach auth headers
write(`${adminBase}/app/api-proxy/[...path]/route.ts`, `
import { NextRequest, NextResponse } from "next/server";
import { auth } from "../../../auth";
import { SignJWT } from "jose";

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";
const apiSecret = new TextEncoder().encode(
  process.env.API_JWT_SECRET ?? "dev-secret-key-must-be-at-least-32-characters-long!"
);

async function createApiToken(sub: string, name: string): Promise<string> {
  return new SignJWT({ sub, name })
    .setProtectedHeader({ alg: "HS256" })
    .setIssuedAt()
    .setExpirationTime("1h")
    .sign(apiSecret);
}

export async function GET(req: NextRequest, { params }: { params: Promise<{ path: string[] }> }) {
  return proxyRequest(req, await params, "GET");
}
export async function POST(req: NextRequest, { params }: { params: Promise<{ path: string[] }> }) {
  return proxyRequest(req, await params, "POST");
}
export async function PUT(req: NextRequest, { params }: { params: Promise<{ path: string[] }> }) {
  return proxyRequest(req, await params, "PUT");
}
export async function DELETE(req: NextRequest, { params }: { params: Promise<{ path: string[] }> }) {
  return proxyRequest(req, await params, "DELETE");
}

async function proxyRequest(req: NextRequest, params: { path: string[] }, method: string) {
  const session = await auth();
  const headers: HeadersInit = { "Content-Type": "application/json" };

  if (session?.githubId) {
    const token = await createApiToken(session.githubId, session.user?.name ?? session.githubId);
    headers["Authorization"] = "Bearer " + token;
  }

  const apiPath = "/api/" + params.path.join("/");
  const url = API_URL + apiPath + (req.nextUrl.search || "");

  const body = method !== "GET" && method !== "DELETE"
    ? await req.text()
    : undefined;

  const res = await fetch(url, { method, headers, body });
  const data = await res.text();

  return new NextResponse(data, {
    status: res.status,
    headers: { "Content-Type": res.headers.get("Content-Type") || "application/json" },
  });
}
`.trim());

// ===== WEB APP PAGES =====
write(`${webBase}/app/page.tsx`, `
import Link from "next/link";

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";

interface PostSummary {
  postId: string;
  title: string;
  slug: string;
  excerpt?: string;
  publishedAt?: string;
  tags: Array<{ tagId: string; tagName: string; tagSlug: string }>;
}

interface PagedResult {
  items: PostSummary[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

export default async function HomePage({
  searchParams,
}: {
  searchParams: Promise<{ page?: string; tag?: string }>;
}) {
  const sp = await searchParams;
  const page = parseInt(sp.page || "1");
  const tag = sp.tag;

  const url = tag
    ? API_URL + "/api/posts?tag=" + tag + "&page=" + page + "&pageSize=10"
    : API_URL + "/api/posts?page=" + page + "&pageSize=10";

  const data = await fetch(url, { next: { tags: ["posts"], revalidate: 3600 } }).then(r => r.json()) as PagedResult;

  return (
    <main className="max-w-3xl mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-8">{tag ? "Posts tagged: " + tag : "Latest Posts"}</h1>
      <div className="space-y-8">
        {data.items.map((post) => (
          <article key={post.postId} className="border-b pb-8">
            <h2 className="text-xl font-semibold mb-2">
              <Link href={"/" + post.slug} className="hover:text-blue-600">
                {post.title}
              </Link>
            </h2>
            {post.excerpt && <p className="text-gray-600 mb-3">{post.excerpt}</p>}
            <div className="flex gap-2 flex-wrap">
              {post.tags?.map((t) => (
                <Link key={t.tagId} href={"/?tag=" + t.tagSlug}
                  className="text-xs bg-gray-100 text-gray-600 px-2 py-1 rounded hover:bg-gray-200">
                  {t.tagName}
                </Link>
              ))}
            </div>
            {post.publishedAt && (
              <p className="text-sm text-gray-400 mt-2">
                {new Date(post.publishedAt).toLocaleDateString()}
              </p>
            )}
          </article>
        ))}
      </div>
      {data.items.length === 0 && (
        <p className="text-gray-500 text-center py-12">No posts yet.</p>
      )}
      <div className="flex justify-between mt-8">
        {page > 1 && (
          <Link href={"/?page=" + (page - 1) + (tag ? "&tag=" + tag : "")}
            className="text-blue-600 hover:underline">
            ← Previous
          </Link>
        )}
        {data.items.length === 10 && (
          <Link href={"/?page=" + (page + 1) + (tag ? "&tag=" + tag : "")}
            className="text-blue-600 hover:underline ml-auto">
            Next →
          </Link>
        )}
      </div>
    </main>
  );
}
`.trim());

write(`${webBase}/app/[slug]/page.tsx`, `
import { notFound } from "next/navigation";

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";

interface PostDetail {
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

export default async function PostPage({ params }: { params: Promise<{ slug: string }> }) {
  const { slug } = await params;

  const res = await fetch(API_URL + "/api/posts/" + slug, {
    next: { tags: ["posts"], revalidate: 3600 },
  });

  if (res.status === 404) notFound();
  if (!res.ok) throw new Error("Failed to load post");

  const post = await res.json() as PostDetail;

  if (post.state !== "Published") notFound();

  return (
    <main className="max-w-3xl mx-auto px-4 py-8">
      <article>
        {post.coverImageUrl && (
          <img src={post.coverImageUrl} alt={post.title}
            className="w-full h-64 object-cover rounded mb-8" />
        )}
        <h1 className="text-3xl font-bold mb-4">{post.title}</h1>
        <div className="flex gap-4 text-sm text-gray-500 mb-6">
          <span>{post.authorName}</span>
          {post.publishedAt && <span>{new Date(post.publishedAt).toLocaleDateString()}</span>}
        </div>
        <div
          className="prose prose-lg max-w-none"
          dangerouslySetInnerHTML={{ __html: post.content }}
        />
        {post.tags?.length > 0 && (
          <div className="flex gap-2 mt-8 pt-8 border-t">
            {post.tags.map((t) => (
              <a key={t.tagId} href={"/?tag=" + t.tagSlug}
                className="text-xs bg-gray-100 text-gray-600 px-2 py-1 rounded hover:bg-gray-200">
                {t.tagName}
              </a>
            ))}
          </div>
        )}
      </article>
    </main>
  );
}
`.trim());

write(`${webBase}/app/tags/[slug]/page.tsx`, `
import Link from "next/link";
import { notFound } from "next/navigation";

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";

export default async function TagPage({ params }: { params: Promise<{ slug: string }> }) {
  const { slug } = await params;

  const res = await fetch(API_URL + "/api/posts?tag=" + slug + "&page=1&pageSize=20", {
    next: { tags: ["posts"], revalidate: 3600 },
  });

  if (!res.ok) notFound();
  const data = await res.json();

  return (
    <main className="max-w-3xl mx-auto px-4 py-8">
      <h1 className="text-2xl font-bold mb-6">Posts tagged: {slug}</h1>
      <div className="space-y-6">
        {data.items.map((post: { postId: string; title: string; slug: string; excerpt?: string }) => (
          <article key={post.postId} className="border-b pb-6">
            <h2 className="text-lg font-semibold">
              <Link href={"/" + post.slug} className="hover:text-blue-600">{post.title}</Link>
            </h2>
            {post.excerpt && <p className="text-gray-600 mt-1">{post.excerpt}</p>}
          </article>
        ))}
      </div>
      {data.items.length === 0 && (
        <p className="text-gray-500 text-center py-12">No posts with this tag.</p>
      )}
    </main>
  );
}
`.trim());

write(`${webBase}/lib/api.ts`, `
const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";

export async function apiGet<T>(path: string, tags?: string[]): Promise<T> {
  const res = await fetch(API_URL + path, {
    next: { tags: tags || ["default"], revalidate: 3600 },
  });
  if (!res.ok) throw new Error("API error " + res.status);
  return res.json() as Promise<T>;
}
`.trim());

console.log('All files written successfully!');
