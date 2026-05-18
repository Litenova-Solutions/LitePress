import * as React from "react";
import { cn } from "../lib/utils";

export type DialogProps = React.ComponentPropsWithoutRef<"div">;

export const Dialog = React.forwardRef<HTMLElement, DialogProps>(
  ({ className, ...props }, ref) => React.createElement("div", { ref, className: cn(className), ...props })
);
Dialog.displayName = "Dialog";
