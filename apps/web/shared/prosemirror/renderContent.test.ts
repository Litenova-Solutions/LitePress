import { describe, expect, it } from "vitest";
import { renderProseMirrorToHtml, excerptFromContent } from "./renderContent";

describe("renderContent", () => {
  it("renders plain text as paragraph html", () => {
    const html = renderProseMirrorToHtml("Hello world");
    expect(html).toContain("Hello world");
    expect(html).toContain("<p>");
  });

  it("renders prosemirror json", () => {
    const json = JSON.stringify({
      type: "doc",
      content: [{ type: "paragraph", content: [{ type: "text", text: "JSON body" }] }],
    });
    const html = renderProseMirrorToHtml(json);
    expect(html).toContain("JSON body");
  });

  it("builds excerpt from content", () => {
    const excerpt = excerptFromContent("Short post text");
    expect(excerpt).toBe("Short post text");
  });
});
