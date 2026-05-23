import { NextRequest, NextResponse } from "next/server";
import { auth } from "../../../auth";
import { mintApiToken } from "../../../lib/auth/mintApiToken";

import { env } from "@/lib/env";

export async function GET(req: NextRequest, { params }: { params: Promise<{ path: string[] }> }) {
  return proxyRequest(req, await params, "GET");
}
export async function POST(req: NextRequest, { params }: { params: Promise<{ path: string[] }> }) {
  return proxyRequest(req, await params, "POST");
}
export async function PUT(req: NextRequest, { params }: { params: Promise<{ path: string[] }> }) {
  return proxyRequest(req, await params, "PUT");
}
export async function DELETE(req: NextRequest, { params }: { params: Promise<{ path: string[] }> }) {
  return proxyRequest(req, await params, "DELETE");
}

async function proxyRequest(req: NextRequest, params: { path: string[] }, method: string) {
  const session = await auth();
  const headers: HeadersInit = { "Content-Type": "application/json" };

  if (session?.githubId) {
    const token = await mintApiToken(session.githubId, session.user?.name ?? session.githubId);
    headers["Authorization"] = "Bearer " + token;
  }

  const apiPath = "/api/" + params.path.join("/");
  const url = env.API_URL + apiPath + (req.nextUrl.search || "");

  const body = method !== "GET" && method !== "DELETE"
    ? await req.text()
    : undefined;

  const res = await fetch(url, { method, headers, body });
  const data = await res.text();

  return new NextResponse(data, {
    status: res.status,
    headers: { "Content-Type": res.headers.get("Content-Type") || "application/json" },
  });
}