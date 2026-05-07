import * as React from "react";
import { cn } from "../lib/utils";

export type DropdownMenuProps = React.ComponentPropsWithoutRef<"div">;

export const DropdownMenu = React.forwardRef<HTMLElement, DropdownMenuProps>(
  ({ className, ...props }, ref) => React.createElement("div", { ref, className: cn(className), ...props })
);
DropdownMenu.displayName = "DropdownMenu";
