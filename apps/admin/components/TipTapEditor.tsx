"use client";
import { useEditor, EditorContent } from "@tiptap/react";
import StarterKit from "@tiptap/starter-kit";
import CodeBlockLowlight from "@tiptap/extension-code-block-lowlight";
import { common, createLowlight } from "lowlight";
const lowlight = createLowlight(common);
export function TipTapEditor(){ const editor = useEditor({extensions:[StarterKit, CodeBlockLowlight.configure({ lowlight })], content:"<p>Write your post...</p>"}); return <EditorContent editor={editor} />; }
