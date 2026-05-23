"use client";
// Rename requires browser prompt for the new name.

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
      alert(await res.text());
      return;
    }

    window.location.reload();
  }

  return (
    <button
      type="button"
      onClick={handleRename}
      className="text-blue-600 hover:underline text-sm mr-3"
    >
      Rename
    </button>
  );
}
