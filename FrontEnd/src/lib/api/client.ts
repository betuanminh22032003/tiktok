/**
 * HTTP client configuration and utilities
 * @module lib/api/client
 */

import axios, { AxiosInstance, AxiosError, InternalAxiosRequestConfig } from 'axios';
import { APP_CONFIG, STORAGE_KEYS } from '@/lib/constants';
import { ApiError } from '@/lib/types';

/**
 * Custom error class for API errors
 */
export class ApiClientError extends Error {
  constructor(
    public status: number,
    public code: string,
    message: string,
    public details?: Record<string, unknown>
  ) {
    super(message);
    this.name = 'ApiClientError';
  }

  static fromAxiosError(error: AxiosError<ApiError>): ApiClientError {
    if (error.response) {
      const { data, status } = error.response;
      return new ApiClientError(
        status,
        data.code || 'UNKNOWN_ERROR',
        data.message || error.message,
        data.details
      );
    }
    
    if (error.request) {
      return new ApiClientError(
        0,
        'NETWORK_ERROR',
        'Network error. Please check your connection.'
      );
    }

    return new ApiClientError(
      0,
      'UNKNOWN_ERROR',
      error.message || 'An unknown error occurred'
    );
  }
}

/**
 * Create and configure axios instance
 */
function createApiClient(): AxiosInstance {
  const client = axios.create({
    baseURL: APP_CONFIG.API_BASE_URL,
    timeout: 30000,
    headers: {
      'Content-Type': 'application/json',
    },
  });

  // Request interceptor - Add auth token
  client.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
      const token = localStorage.getItem(STORAGE_KEYS.AUTH_TOKEN);
      
      if (token && config.headers) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      
      return config;
    },
    (error: AxiosError<ApiError>) => {
      return Promise.reject(ApiClientError.fromAxiosError(error));
    }
  );

  // Response interceptor - Handle errors
  client.interceptors.response.use(
    (response) => response,
    (error: AxiosError<ApiError>) => {
      const apiError = ApiClientError.fromAxiosError(error);
      
      // Handle unauthorized errors
      if (apiError.status === 401) {
        localStorage.removeItem(STORAGE_KEYS.AUTH_TOKEN);
        localStorage.removeItem(STORAGE_KEYS.USER);
        // Redirect to login or trigger auth state update
        if (typeof window !== 'undefined') {
          window.location.href = '/login';
        }
      }
      
      return Promise.reject(apiError);
    }
  );

  return client;
}

/**
 * Singleton API client instance
 */
export const apiClient = createApiClient();

/**
 * Helper function to handle file uploads with progress tracking
 */
export async function uploadFile(
  url: string,
  formData: FormData,
  onProgress?: (progress: number) => void
): Promise<unknown> {
  const token = localStorage.getItem(STORAGE_KEYS.AUTH_TOKEN);
  
  try {
    const response = await axios.post(url, formData, {
      baseURL: APP_CONFIG.API_BASE_URL,
      headers: {
        'Content-Type': 'multipart/form-data',
        ...(token && { Authorization: `Bearer ${token}` }),
      },
      onUploadProgress: (progressEvent) => {
        if (onProgress && progressEvent.total) {
          const percentage = (progressEvent.loaded * 100) / progressEvent.total;
          onProgress(Math.round(percentage));
        }
      },
    });
    
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      throw ApiClientError.fromAxiosError(error);
    }
    throw error;
  }
}
