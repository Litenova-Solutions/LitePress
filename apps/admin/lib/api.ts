import { auth } from "../auth";
import { mintApiToken } from "./auth/mintApiToken";
import { env } from "./env";

async function getAuthHeaders(): Promise<HeadersInit> {
  const session = await auth();
  if (!session?.githubId) return {};
  const token = await mintApiToken(
    session.githubId,
    session.user?.name ?? session.githubId
  );
  return { Authorization: `Bearer ${token}` };
}

export async function apiGet<T>(path: string): Promise<T> {
  const headers = await getAuthHeaders();
  const res = await fetch(`${env.API_URL}${path}`, { headers, cache: "no-store" });
  if (!res.ok) throw new Error(`API error ${res.status}: ${await res.text()}`);
  return res.json() as Promise<T>;
}

export async function apiPost<T>(path: string, body: unknown): Promise<T> {
  const headers = await getAuthHeaders();
  const res = await fetch(`${env.API_URL}${path}`, {
    method: "POST",
    headers: { ...headers, "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });
  if (!res.ok) throw new Error(`API error ${res.status}: ${await res.text()}`);
  return res.json() as Promise<T>;
}

export async function apiPut<T>(path: string, body: unknown): Promise<T> {
  const headers = await getAuthHeaders();
  const res = await fetch(`${env.API_URL}${path}`, {
    method: "PUT",
    headers: { ...headers, "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });
  if (!res.ok) throw new Error(`API error ${res.status}: ${await res.text()}`);
  return res.json() as Promise<T>;
}

export async function apiDelete(path: string): Promise<void> {
  const headers = await getAuthHeaders();
  const res = await fetch(`${env.API_URL}${path}`, { method: "DELETE", headers });
  if (!res.ok) throw new Error(`API error ${res.status}: ${await res.text()}`);
}

export async function apiPostNoContent(path: string): Promise<void> {
  const headers = await getAuthHeaders();
  const res = await fetch(`${env.API_URL}${path}`, { method: "POST", headers });
  if (!res.ok) throw new Error(`API error ${res.status}: ${await res.text()}`);
}