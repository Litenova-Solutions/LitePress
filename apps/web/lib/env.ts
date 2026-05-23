import { z } from "zod";

const serverSchema = z.object({
  NODE_ENV: z
    .enum(["development", "test", "production"])
    .default("development"),
  API_URL: z.string().url().default("http://localhost:5000"),
  SITE_URL: z.string().url().default("http://localhost:3000"),
});

const publicSchema = z.object({
  NEXT_PUBLIC_SITE_URL: z.string().url().optional(),
  NEXT_PUBLIC_GISCUS_REPO: z.string().optional(),
  NEXT_PUBLIC_GISCUS_REPO_ID: z.string().optional(),
  NEXT_PUBLIC_GISCUS_CATEGORY_ID: z.string().optional(),
});

const serverEnv =
  typeof window === "undefined"
    ? serverSchema.parse(process.env)
    : {
        NODE_ENV: "development" as const,
        API_URL: "http://localhost:5000",
        SITE_URL: "http://localhost:3000",
      };

const publicEnv = publicSchema.parse({
  NEXT_PUBLIC_SITE_URL: process.env.NEXT_PUBLIC_SITE_URL,
  NEXT_PUBLIC_GISCUS_REPO: process.env.NEXT_PUBLIC_GISCUS_REPO,
  NEXT_PUBLIC_GISCUS_REPO_ID: process.env.NEXT_PUBLIC_GISCUS_REPO_ID,
  NEXT_PUBLIC_GISCUS_CATEGORY_ID: process.env.NEXT_PUBLIC_GISCUS_CATEGORY_ID,
});

export const env = {
  ...serverEnv,
  ...publicEnv,
  siteUrl: publicEnv.NEXT_PUBLIC_SITE_URL ?? serverEnv.SITE_URL,
};

export type WebEnv = typeof env;
