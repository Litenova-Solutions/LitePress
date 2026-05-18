import NextAuth from "next-auth";
import GitHub from "next-auth/providers/github";

const ownerId = process.env.GITHUB_OWNER_ID;

export const { handlers, auth, signIn, signOut } = NextAuth({
  providers: [GitHub],
  callbacks: {
    async signIn({ profile }) {
      return !!profile?.id && String(profile.id) === ownerId;
    }
  }
});
