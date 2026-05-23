import Link from "next/link";

export function Header() {
  return (
    <header className="border-b p-4 flex gap-6 items-center">
      <Link href="/" className="font-semibold hover:text-blue-600">
        LiteNova Blog
      </Link>
      <Link href="/tags" className="text-sm text-gray-600 hover:text-blue-600">
        Tags
      </Link>
    </header>
  );
}
