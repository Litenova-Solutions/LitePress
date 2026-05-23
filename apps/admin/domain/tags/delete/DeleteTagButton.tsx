"use client";
// Delete confirmation requires browser confirm dialog.

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
      alert(await res.text());
      return;
    }
    window.location.reload();
  }

  return (
    <button
      type="button"
      onClick={handleDelete}
      className="text-red-600 hover:underline text-sm"
    >
      Delete
    </button>
  );
}
