import createClient from "@litepress/api-client";
import type { paths } from "@litepress/api-types";
import { auth } from "@/auth";
import { mintApiToken } from "@/lib/auth/mintApiToken";
import { env } from "@/lib/env";

export async function getApiClient() {
  const session = await auth();
  const headers: Record<string, string> = {};

  if (session?.githubId) {
    const token = await mintApiToken(
      session.githubId,
      session.user?.name ?? session.githubId
    );
    headers.Authorization = `Bearer ${token}`;
  }

  return createClient<paths>({
    baseUrl: env.API_URL,
    headers,
    fetch: (input: RequestInfo | URL, init?: RequestInit) =>
      fetch(input, { ...init, cache: "no-store" }),
  });
}
