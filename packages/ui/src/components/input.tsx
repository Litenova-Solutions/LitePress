import * as React from "react";
import { cn } from "../lib/utils";

export type InputProps = React.ComponentPropsWithoutRef<"input">;

export const Input = React.forwardRef<HTMLElement, InputProps>(
  ({ className, ...props }, ref) => React.createElement("input", { ref, className: cn(className), ...props })
);
Input.displayName = "Input";
