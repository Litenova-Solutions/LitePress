import * as React from "react";
import { cn } from "../lib/utils";

export type SeparatorProps = React.ComponentPropsWithoutRef<"hr">;

export const Separator = React.forwardRef<HTMLElement, SeparatorProps>(
  ({ className, ...props }, ref) => React.createElement("hr", { ref, className: cn(className), ...props })
);
Separator.displayName = "Separator";
