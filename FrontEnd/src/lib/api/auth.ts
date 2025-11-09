/**
 * Authentication API service
 * @module lib/api/auth
 */

import { apiClient } from './client';
import { API_ENDPOINTS, STORAGE_KEYS } from '@/lib/constants';
import { AuthResponse, LoginCredentials, SignupCredentials, User } from '@/lib/types';

/**
 * Login user
 */
export async function login(credentials: LoginCredentials): Promise<AuthResponse> {
  const response = await apiClient.post<AuthResponse>(
    API_ENDPOINTS.AUTH.LOGIN,
    credentials
  );
  
  // Store token and user
  localStorage.setItem(STORAGE_KEYS.AUTH_TOKEN, response.data.token);
  localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(response.data.user));
  
  return response.data;
}

/**
 * Sign up new user
 */
export async function signup(credentials: SignupCredentials): Promise<AuthResponse> {
  const response = await apiClient.post<AuthResponse>(
    API_ENDPOINTS.AUTH.SIGNUP,
    credentials
  );
  
  // Store token and user
  localStorage.setItem(STORAGE_KEYS.AUTH_TOKEN, response.data.token);
  localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(response.data.user));
  
  return response.data;
}

/**
 * Logout user
 */
export async function logout(): Promise<void> {
  try {
    await apiClient.post(API_ENDPOINTS.AUTH.LOGOUT);
  } finally {
    // Clear storage even if API call fails
    localStorage.removeItem(STORAGE_KEYS.AUTH_TOKEN);
    localStorage.removeItem(STORAGE_KEYS.USER);
  }
}

/**
 * Get current user
 */
export async function getCurrentUser(): Promise<User> {
  const response = await apiClient.get<User>(API_ENDPOINTS.AUTH.ME);
  return response.data;
}

/**
 * Get user from local storage
 */
export function getStoredUser(): User | null {
  if (typeof window === 'undefined') return null;
  
  const userStr = localStorage.getItem(STORAGE_KEYS.USER);
  if (!userStr) return null;
  
  try {
    return JSON.parse(userStr) as User;
  } catch {
    return null;
  }
}

/**
 * Check if user is authenticated
 */
export function isAuthenticated(): boolean {
  if (typeof window === 'undefined') return false;
  return !!localStorage.getItem(STORAGE_KEYS.AUTH_TOKEN);
}
