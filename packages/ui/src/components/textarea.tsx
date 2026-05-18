import * as React from "react";
import { cn } from "../lib/utils";

export type TextareaProps = React.ComponentPropsWithoutRef<"textarea">;

export const Textarea = React.forwardRef<HTMLElement, TextareaProps>(
  ({ className, ...props }, ref) => React.createElement("textarea", { ref, className: cn(className), ...props })
);
Textarea.displayName = "Textarea";
