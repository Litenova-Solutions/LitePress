"use client";
// Delete confirmation requires browser confirm dialog.

import { toast } from "sonner";
import { Button } from "@/components/ui/button";

interface DeleteTagButtonProps {
  tagId: string;
  tagName: string;
}

export function DeleteTagButton({ tagId, tagName }: DeleteTagButtonProps) {
  async function handleDelete() {
    if (!confirm(`Delete tag "${tagName}"?`)) {
      return;
    }
    const res = await fetch("/api-proxy/tags/" + tagId, { method: "DELETE" });
    if (!res.ok) {
      toast.error(await res.text());
      return;
    }
    toast.success("Tag deleted");
    window.location.reload();
  }

  return (
    <Button type="button" variant="destructive" size="sm" onClick={handleDelete}>
      Delete
    </Button>
  );
}
