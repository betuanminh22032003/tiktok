/**
 * TypeScript type definitions for the application
 * @module lib/types
 */

/**
 * User entity
 */
export interface User {
  id: string;
  email: string;
  username: string;
  avatar?: string;
  bio?: string;
  followersCount?: number;
  followingCount?: number;
  likesCount?: number;
  createdAt: string;
}

/**
 * Video entity
 */
export interface Video {
  id: string;
  userId: string;
  user: Pick<User, 'id' | 'username' | 'avatar'>;
  url: string;
  thumbnailUrl: string;
  caption: string;
  duration: number;
  width: number;
  height: number;
  likesCount: number;
  commentsCount: number;
  sharesCount: number;
  viewsCount: number;
  isLiked?: boolean;
  createdAt: string;
  updatedAt: string;
}

/**
 * Comment entity
 */
export interface Comment {
  id: string;
  videoId: string;
  userId: string;
  user: Pick<User, 'id' | 'username' | 'avatar'>;
  text: string;
  likesCount: number;
  isLiked?: boolean;
  createdAt: string;
  updatedAt: string;
}

/**
 * Paginated response
 */
export interface PaginatedResponse<T> {
  data: T[];
  page: number;
  pageSize: number;
  total: number;
  hasMore: boolean;
}

/**
 * API Error response
 */
export interface ApiError {
  message: string;
  code: string;
  status: number;
  details?: Record<string, unknown>;
}

/**
 * Upload progress
 */
export interface UploadProgress {
  loaded: number;
  total: number;
  percentage: number;
}

/**
 * Video upload data
 */
export interface VideoUpload {
  file: File;
  caption: string;
  thumbnailFile?: File;
}

/**
 * Auth credentials
 */
export interface LoginCredentials {
  email: string;
  password: string;
}

export interface SignupCredentials extends LoginCredentials {
  username: string;
}

/**
 * Auth response
 */
export interface AuthResponse {
  user: User;
  token: string;
}

/**
 * Socket events payload types
 */
export interface SocketLikeEvent {
  videoId: string;
  userId: string;
  isLiked: boolean;
  likesCount: number;
}

export interface SocketCommentEvent {
  videoId: string;
  comment: Comment;
}

export interface SocketViewEvent {
  videoId: string;
  viewsCount: number;
}
