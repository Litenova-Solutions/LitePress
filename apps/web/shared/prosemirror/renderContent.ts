import StarterKit from "@tiptap/starter-kit";
import { generateHTML } from "@tiptap/html";

const extensions = [StarterKit];

function parseContentJson(content: string): object {
  const trimmed = content.trim();
  if (trimmed.startsWith("{")) {
    return JSON.parse(trimmed) as object;
  }

  return {
    type: "doc",
    content: [
      {
        type: "paragraph",
        content: [{ type: "text", text: content }],
      },
    ],
  };
}

export function renderProseMirrorToHtml(content: string): string {
  if (!content.trim()) {
    return "";
  }

  const doc = parseContentJson(content);
  return generateHTML(doc, extensions);
}

export function excerptFromContent(content: string, maxLength = 160): string {
  const html = renderProseMirrorToHtml(content);
  const text = html.replace(/<[^>]+>/g, " ").replace(/\s+/g, " ").trim();
  if (text.length <= maxLength) {
    return text;
  }
  return text.slice(0, maxLength - 1).trimEnd() + "…";
}
