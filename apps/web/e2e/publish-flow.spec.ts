import { test, expect } from "@playwright/test";
import { existsSync, readFileSync } from "node:fs";
import { join } from "node:path";

const SEED_FILE = join(__dirname, ".seed.json");

function readSeed(): { title: string; slug: string } | null {
  if (!existsSync(SEED_FILE)) {
    return null;
  }
  return JSON.parse(readFileSync(SEED_FILE, "utf8")) as {
    title: string;
    slug: string;
  };
}

test("published post appears on home and slug page", async ({ page }) => {
  const seed = readSeed();
  test.skip(!seed, "Requires API seed — run with E2E_API_URL and a running API");

  await page.goto("/");
  await expect(page.getByText(seed!.title)).toBeVisible();

  await page.goto("/" + seed!.slug);
  await expect(page.getByRole("heading", { level: 1 })).toHaveText(seed!.title);
});
