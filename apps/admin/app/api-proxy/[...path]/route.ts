import { NextRequest, NextResponse } from "next/server";
import { auth } from "../../../auth";
import { SignJWT } from "jose";

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";
const apiSecret = new TextEncoder().encode(
  process.env.API_JWT_SECRET ?? "dev-secret-key-must-be-at-least-32-characters-long!"
);

async function createApiToken(sub: string, name: string): Promise<string> {
  return new SignJWT({ sub, name })
    .setProtectedHeader({ alg: "HS256" })
    .setIssuedAt()
    .setExpirationTime("1h")
    .sign(apiSecret);
}

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
    const token = await createApiToken(session.githubId, session.user?.name ?? session.githubId);
    headers["Authorization"] = "Bearer " + token;
  }

  const apiPath = "/api/" + params.path.join("/");
  const url = API_URL + apiPath + (req.nextUrl.search || "");

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