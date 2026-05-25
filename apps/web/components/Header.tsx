import Link from "next/link";
import { buttonVariants } from "@/components/ui/button";
import { cn } from "@/lib/utils";
import { Separator } from "@/components/ui/separator";

export function Header() {
  return (
    <header className="border-b bg-background">
      <div className="mx-auto flex max-w-5xl items-center gap-6 px-6 py-4">
        <Link href="/" className="font-heading text-lg font-semibold tracking-tight">
          LitePress
        </Link>
        <Separator orientation="vertical" className="h-5" />
        <nav className="flex items-center gap-2">
          <Link
            href="/"
            className={cn(buttonVariants({ variant: "ghost", size: "sm" }), "text-muted-foreground")}
          >
            Posts
          </Link>
          <Link
            href="/tags"
            className={cn(buttonVariants({ variant: "ghost", size: "sm" }), "text-muted-foreground")}
          >
            Tags
          </Link>
        </nav>
      </div>
    </header>
  );
}
