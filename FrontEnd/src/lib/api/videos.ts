/**
 * Video API service
 * @module lib/api/videos
 */

import { apiClient } from './client';
import { API_ENDPOINTS } from '@/lib/constants';
import { Video, PaginatedResponse } from '@/lib/types';

/**
 * Fetch paginated videos
 */
export async function fetchVideos(
  page: number = 1,
  pageSize: number = 10
): Promise<PaginatedResponse<Video>> {
  const response = await apiClient.get<PaginatedResponse<Video>>(
    API_ENDPOINTS.VIDEOS,
    {
      params: { page, pageSize },
    }
  );
  return response.data;
}

/**
 * Fetch a single video by ID
 */
export async function fetchVideo(id: string): Promise<Video> {
  const response = await apiClient.get<Video>(`${API_ENDPOINTS.VIDEOS}/${id}`);
  return response.data;
}

/**
 * Like/unlike a video
 */
export async function toggleLike(videoId: string): Promise<{ isLiked: boolean; likesCount: number }> {
  const response = await apiClient.post<{ isLiked: boolean; likesCount: number }>(
    `${API_ENDPOINTS.LIKES}/${videoId}`
  );
  return response.data;
}

/**
 * Increment view count for a video
 */
export async function incrementViewCount(videoId: string): Promise<void> {
  await apiClient.post(`${API_ENDPOINTS.VIDEOS}/${videoId}/view`);
}

/**
 * Delete a video
 */
export async function deleteVideo(videoId: string): Promise<void> {
  await apiClient.delete(`${API_ENDPOINTS.VIDEOS}/${videoId}`);
}
