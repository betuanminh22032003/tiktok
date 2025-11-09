/**
 * Custom hooks for comments data fetching with SWR
 * @module hooks/useComments
 */

import useSWR from 'swr';
import useSWRInfinite from 'swr/infinite';
import { fetchComments } from '@/lib/api/comments';
import { QUERY_KEYS } from '@/lib/constants';
import { Comment, PaginatedResponse } from '@/lib/types';

/**
 * Hook to fetch comments for a video
 */
export function useComments(videoId: string | null) {
  const { data, error, isLoading, isValidating, mutate } = useSWR<PaginatedResponse<Comment>>(
    videoId ? QUERY_KEYS.COMMENTS(videoId) : null,
    videoId ? () => fetchComments(videoId, 1, 20) : null,
    {
      revalidateOnFocus: false,
      revalidateOnReconnect: false,
    }
  );

  return {
    comments: data?.data || [],
    error,
    isLoading,
    isValidating,
    hasMore: data?.hasMore || false,
    mutate,
  };
}

/**
 * Hook to fetch infinite scrolling comments
 */
export function useInfiniteComments(videoId: string | null) {
  const { data, error, size, setSize, isLoading, isValidating, mutate } = useSWRInfinite<PaginatedResponse<Comment>>(
    videoId ? (pageIndex) => `${QUERY_KEYS.COMMENTS(videoId)}-${pageIndex + 1}` : () => null,
    videoId
      ? (key) => {
          const page = parseInt(key.split('-').pop() || '1', 10);
          return fetchComments(videoId, page, 20);
        }
      : null,
    {
      revalidateFirstPage: false,
      revalidateOnFocus: false,
      revalidateOnReconnect: true,
    }
  );

  const comments: Comment[] = data ? data.flatMap((page) => page.data) : [];
  const hasMore = data ? data[data.length - 1]?.hasMore : true;
  const isLoadingMore = isLoading || (size > 0 && data && typeof data[size - 1] === 'undefined');

  const loadMore = () => {
    if (!isLoadingMore && hasMore) {
      setSize(size + 1);
    }
  };

  return {
    comments,
    error,
    isLoading,
    isLoadingMore,
    isValidating,
    hasMore,
    loadMore,
    mutate,
  };
}
