import * as React from "react";
import { cn } from "../lib/utils";

export type ButtonProps = React.ComponentPropsWithoutRef<"button">;

export const Button = React.forwardRef<HTMLElement, ButtonProps>(
  ({ className, ...props }, ref) => React.createElement("button", { ref, className: cn(className), ...props })
);
Button.displayName = "Button";
