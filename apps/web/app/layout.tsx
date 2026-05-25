import "./globals.css";
import { Header } from "../components/Header";
import { Footer } from "../components/Footer";
import { Geist } from "next/font/google";
import { cn } from "@/lib/utils";

const geist = Geist({ subsets: ["latin"], variable: "--font-sans" });

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en" className={cn("font-sans", geist.variable)}>
      <body className="min-h-screen bg-background text-foreground antialiased">
        <Header />
        <main className="mx-auto max-w-5xl px-6 py-8">{children}</main>
        <Footer />
      </body>
    </html>
  );
}
