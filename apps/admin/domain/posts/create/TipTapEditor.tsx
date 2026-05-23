"use client";
// TipTap requires DOM APIs for editing.

import { useEditor, EditorContent } from "@tiptap/react";
import StarterKit from "@tiptap/starter-kit";
import CodeBlockLowlight from "@tiptap/extension-code-block-lowlight";
import { common, createLowlight } from "lowlight";
import { useEffect } from "react";

const lowlight = createLowlight(common);

interface TipTapEditorProps {
  value: string;
  onChange: (json: string) => void;
}

function parseInitialContent(value: string): object {
  const trimmed = value.trim();
  if (!trimmed) {
    return { type: "doc", content: [{ type: "paragraph" }] };
  }
  if (trimmed.startsWith("{")) {
    return JSON.parse(trimmed) as object;
  }
  return {
    type: "doc",
    content: [{ type: "paragraph", content: [{ type: "text", text: value }] }],
  };
}

export function TipTapEditor({ value, onChange }: TipTapEditorProps) {
  const editor = useEditor({
    extensions: [StarterKit, CodeBlockLowlight.configure({ lowlight })],
    content: parseInitialContent(value),
    onUpdate: ({ editor: currentEditor }) => {
      onChange(JSON.stringify(currentEditor.getJSON()));
    },
  });

  useEffect(() => {
    if (!editor) {
      return;
    }
    const current = JSON.stringify(editor.getJSON());
    if (value && value !== current) {
      editor.commands.setContent(parseInitialContent(value));
    }
  }, [editor, value]);

  return (
    <div className="border rounded min-h-64 p-3 prose max-w-none focus-within:ring-2 focus-within:ring-blue-500">
      <EditorContent editor={editor} />
    </div>
  );
}
