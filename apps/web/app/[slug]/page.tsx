import { GiscusComments } from "../../components/GiscusComments";
export default async function PostPage({ params }: { params: Promise<{ slug: string }> }) { const { slug } = await params; return <article><h1>{slug}</h1><p>Post content rendered from TipTap JSON.</p><GiscusComments /></article>; }
