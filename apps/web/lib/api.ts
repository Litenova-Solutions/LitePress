const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";

export async function apiGet<T>(path: string, tags?: string[]): Promise<T> {
  const res = await fetch(API_URL + path, {
    next: { tags: tags || ["default"], revalidate: 3600 },
  });
  if (!res.ok) throw new Error("API error " + res.status);
  return res.json() as Promise<T>;
}