import * as React from "react";
import { cn } from "../lib/utils";

export type TableProps = React.ComponentPropsWithoutRef<"table">;

export const Table = React.forwardRef<HTMLElement, TableProps>(
  ({ className, ...props }, ref) => React.createElement("table", { ref, className: cn(className), ...props })
);
Table.displayName = "Table";
