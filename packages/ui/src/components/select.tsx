import * as React from "react";
import { cn } from "../lib/utils";

export type SelectProps = React.ComponentPropsWithoutRef<"select">;

export const Select = React.forwardRef<HTMLElement, SelectProps>(
  ({ className, ...props }, ref) => React.createElement("select", { ref, className: cn(className), ...props })
);
Select.displayName = "Select";
