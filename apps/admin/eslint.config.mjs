import baseConfig from "@litenova/config-eslint";

export default [
  ...baseConfig,
  {
    ignores: ["next-env.d.ts", ".next/**"],
  },
];
