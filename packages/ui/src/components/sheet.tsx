import * as React from "react";
import { cn } from "../lib/utils";

export type SheetProps = React.ComponentPropsWithoutRef<"div">;

export const Sheet = React.forwardRef<HTMLElement, SheetProps>(
  ({ className, ...props }, ref) => React.createElement("div", { ref, className: cn(className), ...props })
);
Sheet.displayName = "Sheet";
