"use client";
// Rename requires browser prompt for the new name.

import { toast } from "sonner";
import { Button } from "@/components/ui/button";

interface RenameTagButtonProps {
  tagId: string;
  tagName: string;
}

export function RenameTagButton({ tagId, tagName }: RenameTagButtonProps) {
  async function handleRename() {
    const newName = prompt(`Rename tag "${tagName}" to:`, tagName);
    if (!newName || newName.trim() === tagName) {
      return;
    }

    const res = await fetch("/api-proxy/tags/" + tagId, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ name: newName.trim() }),
    });

    if (!res.ok) {
      toast.error(await res.text());
      return;
    }

    toast.success("Tag renamed");
    window.location.reload();
  }

  return (
    <Button type="button" variant="outline" size="sm" onClick={handleRename}>
      Rename
    </Button>
  );
}
