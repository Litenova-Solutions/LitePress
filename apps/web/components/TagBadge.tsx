import Link from "next/link";
export function TagBadge({ tag }: { tag: string }) { return <Link href={`/tags/${tag}`} className="rounded border px-2 py-1 text-sm">#{tag}</Link>; }
