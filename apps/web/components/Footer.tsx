import { Separator } from "@/components/ui/separator";

export function Footer() {
  return (
    <footer className="border-t bg-muted/30">
      <div className="mx-auto max-w-5xl space-y-3 px-6 py-8 text-sm text-muted-foreground">
        <Separator />
        <p>
          LitePress ·{" "}
          <a
            href="https://github.com/Litenova-Solutions/LitePress"
            className="underline underline-offset-4 hover:text-foreground"
          >
            GitHub
          </a>{" "}
          ·{" "}
          <a
            href="https://litenova.solutions"
            className="underline underline-offset-4 hover:text-foreground"
          >
            Litenova Solutions
          </a>
        </p>
      </div>
    </footer>
  );
}
