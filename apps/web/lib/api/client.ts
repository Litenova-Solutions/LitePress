import createClient from "@litenova/api-client";
import type { paths } from "@litenova/api-types";
import { env } from "@/lib/env";

interface ApiClientOptions {
  tags?: string[];
  revalidate?: number;
}

export async function getApiClient(options?: ApiClientOptions) {
  const revalidate = options?.revalidate ?? 3600;

  return createClient<paths>({
    baseUrl: env.API_URL,
    fetch: (input: RequestInfo | URL, init?: RequestInit) =>
      fetch(input, {
        ...init,
        next: {
          revalidate,
          tags: options?.tags,
        },
      }),
  });
}
