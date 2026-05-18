export function PostCard({ title, excerpt }: { title: string; excerpt: string }) { return <article className="rounded border p-4"><h2>{title}</h2><p>{excerpt}</p></article>; }
