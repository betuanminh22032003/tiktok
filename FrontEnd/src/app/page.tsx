/**
 * Home Page - Main video feed
 * @module app/page
 */

import Feed from '@/components/Feed';

export default function Home() {
  return (
    <main className="overflow-hidden">
      <Feed />
    </main>
  );
}

