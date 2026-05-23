import { SignJWT } from "jose";
import { existsSync, readFileSync, unlinkSync, writeFileSync } from "node:fs";
import { join } from "node:path";

const SEED_FILE = join(__dirname, ".seed.json");

interface E2ESeed {
  title: string;
  slug: string;
}

export default async function globalSetup() {
  if (existsSync(SEED_FILE)) {
    unlinkSync(SEED_FILE);
  }

  const apiUrl = process.env.E2E_API_URL ?? "http://localhost:5000";
  const jwtSecret =
    process.env.API_JWT_SECRET ??
    "dev-secret-key-must-be-at-least-32-characters-long!";

  try {
    const secret = new TextEncoder().encode(jwtSecret);
    const token = await new SignJWT({ sub: "e2e-user", name: "E2E User" })
      .setProtectedHeader({ alg: "HS256" })
      .setIssuedAt()
      .setExpirationTime("1h")
      .sign(secret);

    const title = `E2E Post ${Date.now()}`;
    const createRes = await fetch(`${apiUrl}/api/posts`, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        title,
        content:
          '{"type":"doc","content":[{"type":"paragraph","content":[{"type":"text","text":"E2E body"}]}]}',
        excerpt: "E2E excerpt",
        tagIds: [],
      }),
    });

    if (!createRes.ok) {
      throw new Error("create post failed: " + (await createRes.text()));
    }

    const { postId } = (await createRes.json()) as { postId: string };

    const publishRes = await fetch(`${apiUrl}/api/posts/${postId}/publish`, {
      method: "POST",
      headers: { Authorization: `Bearer ${token}` },
    });

    if (!publishRes.ok) {
      throw new Error("publish post failed: " + (await publishRes.text()));
    }

    const getRes = await fetch(`${apiUrl}/api/posts/${postId}`, {
      headers: { Authorization: `Bearer ${token}` },
    });

    if (!getRes.ok) {
      throw new Error("get post failed: " + (await getRes.text()));
    }

    const post = (await getRes.json()) as { slug: string };
    const seed: E2ESeed = { title, slug: post.slug };
    writeFileSync(SEED_FILE, JSON.stringify(seed));
  } catch (error) {
    if (process.env.CI) {
      throw error;
    }
    console.warn("E2E seed skipped:", error);
  }
}

export function readE2ESeed(): E2ESeed | null {
  if (!existsSync(SEED_FILE)) {
    return null;
  }
  return JSON.parse(readFileSync(SEED_FILE, "utf8")) as E2ESeed;
}
