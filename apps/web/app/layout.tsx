import "./globals.css";
import { Header } from "../components/Header";
import { Footer } from "../components/Footer";

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body>
        <Header />
        <main className="mx-auto max-w-5xl p-6">{children}</main>
        <Footer />
        <script async defer data-website-id={process.env.NEXT_PUBLIC_UMAMI_WEBSITE_ID} src="https://umami.is/script.js" />
      </body>
    </html>
  );
}
