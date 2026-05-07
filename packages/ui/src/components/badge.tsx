import * as React from "react";
import { cn } from "../lib/utils";

export type BadgeProps = React.ComponentPropsWithoutRef<"span">;

export const Badge = React.forwardRef<HTMLElement, BadgeProps>(
  ({ className, ...props }, ref) => React.createElement("span", { ref, className: cn(className), ...props })
);
Badge.displayName = "Badge";
