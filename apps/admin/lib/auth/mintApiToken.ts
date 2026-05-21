import { SignJWT } from "jose";

const secret = new TextEncoder().encode(
  process.env.API_JWT_SECRET ?? "dev-secret-key-must-be-at-least-32-characters-long!"
);

/**
 * Mints a short-lived HS256 JWT for authenticating server-to-server calls to the backend API.
 *
 * `sub` must be the authenticated user's stable identifier (e.g. GitHub numeric user ID).
 * The token lifetime is 1 hour. For sensitive write operations consider reducing to 5 minutes.
 *
 * The `secret` must match `JwtSettings__Secret` in the API's appsettings.json.
 * Never set `API_JWT_SECRET` with a `NEXT_PUBLIC_` prefix — it must not appear in the client bundle.
 */
export async function mintApiToken(sub: string, name: string): Promise<string> {
  return new SignJWT({ sub, name })
    .setProtectedHeader({ alg: "HS256" })
    .setIssuedAt()
    .setExpirationTime("1h")
    .sign(secret);
}
