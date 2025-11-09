/**
 * Upload API service
 * @module lib/api/upload
 */

import { uploadFile } from './client';
import { API_ENDPOINTS } from '@/lib/constants';
import { Video, VideoUpload } from '@/lib/types';

/**
 * Upload a video with optional thumbnail
 */
export async function uploadVideo(
  data: VideoUpload,
  onProgress?: (progress: number) => void
): Promise<Video> {
  const formData = new FormData();
  formData.append('video', data.file);
  formData.append('caption', data.caption);
  
  if (data.thumbnailFile) {
    formData.append('thumbnail', data.thumbnailFile);
  }
  
  const response = await uploadFile(
    API_ENDPOINTS.UPLOAD,
    formData,
    onProgress
  );
  
  return response as Video;
}

/**
 * Validate video file
 */
export function validateVideoFile(file: File): { valid: boolean; error?: string } {
  const maxSize = 50 * 1024 * 1024; // 50MB
  const allowedTypes = ['video/mp4', 'video/webm', 'video/quicktime'];
  
  if (!allowedTypes.includes(file.type)) {
    return {
      valid: false,
      error: 'Invalid file type. Please upload MP4, WebM, or MOV files.',
    };
  }
  
  if (file.size > maxSize) {
    return {
      valid: false,
      error: 'File size exceeds 50MB limit.',
    };
  }
  
  return { valid: true };
}
