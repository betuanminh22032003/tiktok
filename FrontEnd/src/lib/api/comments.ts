/**
 * Comments API service
 * @module lib/api/comments
 */

import { apiClient } from './client';
import { API_ENDPOINTS } from '@/lib/constants';
import { Comment, PaginatedResponse } from '@/lib/types';

/**
 * Fetch comments for a video
 */
export async function fetchComments(
  videoId: string,
  page: number = 1,
  pageSize: number = 20
): Promise<PaginatedResponse<Comment>> {
  const response = await apiClient.get<PaginatedResponse<Comment>>(
    `${API_ENDPOINTS.COMMENTS}`,
    {
      params: { videoId, page, pageSize },
    }
  );
  return response.data;
}

/**
 * Create a new comment
 */
export async function createComment(
  videoId: string,
  text: string
): Promise<Comment> {
  const response = await apiClient.post<Comment>(API_ENDPOINTS.COMMENTS, {
    videoId,
    text,
  });
  return response.data;
}

/**
 * Delete a comment
 */
export async function deleteComment(commentId: string): Promise<void> {
  await apiClient.delete(`${API_ENDPOINTS.COMMENTS}/${commentId}`);
}

/**
 * Like/unlike a comment
 */
export async function toggleCommentLike(
  commentId: string
): Promise<{ isLiked: boolean; likesCount: number }> {
  const response = await apiClient.post<{ isLiked: boolean; likesCount: number }>(
    `${API_ENDPOINTS.COMMENTS}/${commentId}/like`
  );
  return response.data;
}
