import createClient from "@litenova/api-client";
import type { paths } from "@litenova/api-types";
import { env } from "@/lib/env";

export async function getApiClient() {
  return createClient<paths>({ baseUrl: env.API_URL });
}
