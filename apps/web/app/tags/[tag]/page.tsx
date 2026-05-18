export default async function TagPage({ params }: { params: Promise<{ tag: string }> }) { const { tag } = await params; return <section><h1>Posts tagged: {tag}</h1></section>; }
