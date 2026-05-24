import baseConfig from "@litepress/config-eslint";

export default [
  ...baseConfig,
  {
    ignores: ["next-env.d.ts", ".next/**"],
  },
];
