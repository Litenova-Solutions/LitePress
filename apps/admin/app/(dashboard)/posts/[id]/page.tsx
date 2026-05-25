import { EditPostForm } from "@/features/posts/update/EditPostForm";

export default function EditPostPage({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  return <EditPostForm params={params} />;
}
