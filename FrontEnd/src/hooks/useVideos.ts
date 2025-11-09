/**
 * Custom hooks for video data fetching with SWR
 * @module hooks/useVideos
 */

import useSWR from 'swr';
import useSWRInfinite from 'swr/infinite';
import { fetchVideos, fetchVideo } from '@/lib/api/videos';
import { APP_CONFIG, QUERY_KEYS } from '@/lib/constants';
import { Video, PaginatedResponse } from '@/lib/types';

/**
 * Hook to fetch infinite scrolling videos
 */
export function useInfiniteVideos() {
  const { data, error, size, setSize, isLoading, isValidating, mutate } = useSWRInfinite<PaginatedResponse<Video>>(
    (pageIndex) => `${QUERY_KEYS.VIDEOS}-${pageIndex + 1}`,
    (key) => {
      const page = parseInt(key.split('-').pop() || '1', 10);
      return fetchVideos(page, APP_CONFIG.VIDEO_PAGE_SIZE);
    },
    {
      revalidateFirstPage: false,
      revalidateOnFocus: false,
      revalidateOnReconnect: true,
    }
  );

  const videos: Video[] = data ? data.flatMap((page) => page.data) : [];
  const hasMore = data ? data[data.length - 1]?.hasMore : true;
  const isLoadingMore = isLoading || (size > 0 && data && typeof data[size - 1] === 'undefined');

  const loadMore = () => {
    if (!isLoadingMore && hasMore) {
      setSize(size + 1);
    }
  };

  return {
    videos,
    error,
    isLoading,
    isLoadingMore,
    isValidating,
    hasMore,
    loadMore,
    mutate,
  };
}

/**
 * Hook to fetch a single video
 */
export function useVideo(videoId: string | null) {
  const { data, error, isLoading, isValidating, mutate } = useSWR<Video>(
    videoId ? QUERY_KEYS.VIDEO(videoId) : null,
    videoId ? () => fetchVideo(videoId) : null,
    {
      revalidateOnFocus: false,
      revalidateOnReconnect: false,
    }
  );

  return {
    video: data,
    error,
    isLoading,
    isValidating,
    mutate,
  };
}
