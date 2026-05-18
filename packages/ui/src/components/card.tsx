import * as React from "react";
import { cn } from "../lib/utils";

export type CardProps = React.ComponentPropsWithoutRef<"div">;

export const Card = React.forwardRef<HTMLElement, CardProps>(
  ({ className, ...props }, ref) => React.createElement("div", { ref, className: cn(className), ...props })
);
Card.displayName = "Card";
