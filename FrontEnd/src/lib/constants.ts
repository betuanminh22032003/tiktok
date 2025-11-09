/**
 * Application constants
 * @module lib/constants
 */

/**
 * API endpoints configuration
 */
export const API_ENDPOINTS = {
  VIDEOS: '/api/videos',
  UPLOAD: '/api/upload',
  LIKES: '/api/likes',
  COMMENTS: '/api/comments',
  AUTH: {
    LOGIN: '/api/auth/login',
    SIGNUP: '/api/auth/signup',
    LOGOUT: '/api/auth/logout',
    ME: '/api/auth/me',
  },
  USER: {
    PROFILE: (id: string) => `/api/users/${id}`,
    VIDEOS: (id: string) => `/api/users/${id}/videos`,
  },
} as const;

/**
 * Socket.IO event names
 */
export const SOCKET_EVENTS = {
  CONNECT: 'connect',
  DISCONNECT: 'disconnect',
  LIKE: 'like',
  COMMENT: 'comment',
  LIKE_UPDATE: 'likeUpdate',
  COMMENT_UPDATE: 'commentUpdate',
  VIEW_COUNT: 'viewCount',
} as const;

/**
 * Application configuration
 */
export const APP_CONFIG = {
  VIDEO_PAGE_SIZE: 10,
  MAX_VIDEO_DURATION: 180, // 3 minutes in seconds
  MAX_FILE_SIZE: 50 * 1024 * 1024, // 50MB
  SUPPORTED_VIDEO_FORMATS: ['video/mp4', 'video/webm', 'video/quicktime'],
  SOCKET_URL: process.env.NEXT_PUBLIC_SOCKET_URL || 'http://localhost:3000',
  API_BASE_URL: process.env.NEXT_PUBLIC_API_URL || 'http://localhost:3000',
} as const;

/**
 * Query keys for SWR
 */
export const QUERY_KEYS = {
  VIDEOS: 'videos',
  VIDEO: (id: string) => ['video', id],
  USER: (id: string) => ['user', id],
  USER_VIDEOS: (id: string) => ['user-videos', id],
  COMMENTS: (videoId: string) => ['comments', videoId],
} as const;

/**
 * Local storage keys
 */
export const STORAGE_KEYS = {
  AUTH_TOKEN: 'auth_token',
  USER: 'user',
  THEME: 'theme',
} as const;
