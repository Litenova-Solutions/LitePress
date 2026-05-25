import { test, expect } from "@playwright/test";

test("footer stays at the bottom on short pages", async ({ page }) => {
  await page.goto("/");

  const footerAtBottom = await page.evaluate(() => {
    const footer = document.querySelector("footer");
    if (!footer) {
      return false;
    }

    const rect = footer.getBoundingClientRect();
    return Math.abs(window.innerHeight - rect.bottom) < 2;
  });

  expect(footerAtBottom).toBe(true);
});

test("footer stays below content on tall pages", async ({ page }) => {
  await page.goto("/");

  const footerBelowContent = await page.evaluate(() => {
    const footer = document.querySelector("footer");
    const main = document.querySelector("main");
    if (!footer || !main) {
      return false;
    }

    return footer.getBoundingClientRect().top >= main.getBoundingClientRect().bottom - 1;
  });

  expect(footerBelowContent).toBe(true);
});
