/**
 * Video Feed Component with infinite scroll
 * @module components/Feed
 */

'use client';

import { memo, useCallback, useEffect, useRef } from 'react';
import { useInfiniteVideos } from '@/hooks/useVideos';
import VideoPlayer from '@/components/VideoPlayer';
import { cn } from '@/lib/utils';

interface FeedProps {
  className?: string;
}

/**
 * Feed Component - Main video feed with infinite scroll
 */
const Feed = memo<FeedProps>(({ className }) => {
  const { videos, isLoading, isLoadingMore, hasMore, loadMore, error } = useInfiniteVideos();
  const observerRef = useRef<IntersectionObserver | null>(null);
  const loadMoreRef = useRef<HTMLDivElement | null>(null);

  // Set up intersection observer for infinite scroll
  useEffect(() => {
    if (observerRef.current) observerRef.current.disconnect();

    observerRef.current = new IntersectionObserver(
      (entries) => {
        if (entries[0].isIntersecting && hasMore && !isLoadingMore) {
          loadMore();
        }
      },
      {
        threshold: 0.1,
      }
    );

    if (loadMoreRef.current) {
      observerRef.current.observe(loadMoreRef.current);
    }

    return () => {
      if (observerRef.current) {
        observerRef.current.disconnect();
      }
    };
  }, [hasMore, isLoadingMore, loadMore]);

  const handleVideoInView = useCallback((videoId: string) => {
    console.log('Video in view:', videoId);
    // Track video view or analytics here
  }, []);

  if (error) {
    return (
      <div className="flex items-center justify-center h-screen bg-black text-white">
        <div className="text-center space-y-4">
          <svg className="w-16 h-16 mx-auto text-red-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
          <h2 className="text-2xl font-bold">Failed to load videos</h2>
          <p className="text-gray-400">{error.message || 'Please try again later'}</p>
          <button
            onClick={() => window.location.reload()}
            className="px-6 py-2 bg-white text-black rounded-full font-semibold hover:bg-gray-200 transition-colors"
          >
            Retry
          </button>
        </div>
      </div>
    );
  }

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-screen bg-black">
        <div className="animate-spin rounded-full h-16 w-16 border-t-2 border-b-2 border-white"></div>
      </div>
    );
  }

  if (!videos.length) {
    return (
      <div className="flex items-center justify-center h-screen bg-black text-white">
        <div className="text-center space-y-4">
          <svg className="w-16 h-16 mx-auto text-gray-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 10l4.553-2.276A1 1 0 0121 8.618v6.764a1 1 0 01-1.447.894L15 14M5 18h8a2 2 0 002-2V8a2 2 0 00-2-2H5a2 2 0 00-2 2v8a2 2 0 002 2z" />
          </svg>
          <h2 className="text-2xl font-bold">No videos available</h2>
          <p className="text-gray-400">Check back later for new content</p>
        </div>
      </div>
    );
  }

  return (
    <div className={cn('h-screen w-full snap-y snap-mandatory overflow-y-scroll scrollbar-hide', className)}>
      {videos.map((video) => (
        <div key={video.id} className="h-screen w-full snap-start">
          <VideoPlayer video={video} onVideoInView={handleVideoInView} />
        </div>
      ))}

      {/* Loading indicator for infinite scroll */}
      <div
        ref={loadMoreRef}
        className="h-20 flex items-center justify-center bg-black"
      >
        {isLoadingMore && (
          <div className="animate-spin rounded-full h-8 w-8 border-t-2 border-b-2 border-white"></div>
        )}
      </div>
    </div>
  );
});

Feed.displayName = 'Feed';

export default Feed;
