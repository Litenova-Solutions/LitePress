import { Badge } from "@/components/ui/badge";
import { cn } from "@/lib/utils";

type PostStatusBadgeProps = {
  status: string;
  className?: string;
};

export function PostStatusBadge({ status, className }: PostStatusBadgeProps) {
  const variant =
    status === "Published"
      ? "default"
      : status === "Archived"
        ? "secondary"
        : status === "Scheduled"
          ? "outline"
          : "secondary";

  return (
    <Badge variant={variant} className={cn(className)}>
      {status}
    </Badge>
  );
}
