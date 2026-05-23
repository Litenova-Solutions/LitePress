import { z } from "zod";

const serverSchema = z.object({
  NODE_ENV: z
    .enum(["development", "test", "production"])
    .default("development"),
  API_URL: z.string().url().default("http://localhost:5000"),
  API_JWT_SECRET: z
    .string()
    .min(32)
    .default("dev-secret-key-must-be-at-least-32-characters-long!"),
  AUTH_SECRET: z
    .string()
    .min(32)
    .default("dev-auth-secret-must-be-at-least-32-characters-long!!"),
  AUTH_GITHUB_ID: z.string().min(1).default("placeholder"),
  AUTH_GITHUB_SECRET: z.string().min(1).default("placeholder"),
  GITHUB_OWNER_ID: z.string().min(1).default("0"),
});

export const env = serverSchema.parse(process.env);
