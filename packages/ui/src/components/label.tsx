import * as React from "react";
import { cn } from "../lib/utils";

export type LabelProps = React.ComponentPropsWithoutRef<"label">;

export const Label = React.forwardRef<HTMLElement, LabelProps>(
  ({ className, ...props }, ref) => React.createElement("label", { ref, className: cn(className), ...props })
);
Label.displayName = "Label";
