import * as React from "react";
import { cn } from "../lib/utils";

export type ToastProps = React.ComponentPropsWithoutRef<"div">;

export const Toast = React.forwardRef<HTMLElement, ToastProps>(
  ({ className, ...props }, ref) => React.createElement("div", { ref, className: cn(className), ...props })
);
Toast.displayName = "Toast";
