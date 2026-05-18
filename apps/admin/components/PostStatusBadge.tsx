export function PostStatusBadge({ status }: { status: "Draft"|"Published"|"Scheduled" }){ return <span>{status}</span>; }
