import Link from "next/link";
export function Sidebar(){return <aside className="w-52 border-r p-4"><nav className="grid gap-2"><Link href="/">Dashboard</Link><Link href="/posts">Posts</Link><Link href="/tags">Tags</Link></nav></aside>}
