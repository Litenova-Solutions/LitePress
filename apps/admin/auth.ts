import NextAuth from "next-auth";
import GitHub from "next-auth/providers/github";
import type { DefaultSession, Session } from "next-auth";

declare module "next-auth" {
  interface Session extends DefaultSession {
    githubId?: string;
  }
}

const ownerId = process.env.GITHUB_OWNER_ID;

export const { handlers, auth, signIn, signOut } = NextAuth({
  providers: [GitHub],
  session: { strategy: "jwt" },
  callbacks: {
    async signIn({ profile }) {
      return !!profile?.id && String(profile.id) === ownerId;
    },
    async jwt({ token, profile }) {
      if (profile?.id) {
        token.githubId = String(profile.id);
        token.name = (profile.name as string) || token.name;
      }
      return token;
    },
    async session({ session, token }) {
      session.githubId = token.githubId as string | undefined;
      return session;
    },
  },
});