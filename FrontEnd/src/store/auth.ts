/**
 * Authentication store using Zustand
 * @module store/auth
 */

import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';
import { User, LoginCredentials, SignupCredentials } from '@/lib/types';
import * as authApi from '@/lib/api/auth';

interface AuthState {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: string | null;
}

interface AuthActions {
  login: (credentials: LoginCredentials) => Promise<void>;
  signup: (credentials: SignupCredentials) => Promise<void>;
  logout: () => Promise<void>;
  loadUser: () => Promise<void>;
  clearError: () => void;
}

type AuthStore = AuthState & AuthActions;

const initialState: AuthState = {
  user: null,
  isAuthenticated: false,
  isLoading: false,
  error: null,
};

/**
 * Authentication store
 */
export const useAuthStore = create<AuthStore>()(
  devtools(
    persist(
      (set) => ({
        ...initialState,

        login: async (credentials) => {
          set({ isLoading: true, error: null });
          try {
            const response = await authApi.login(credentials);
            set({
              user: response.user,
              isAuthenticated: true,
              isLoading: false,
              error: null,
            });
          } catch (error) {
            const message = error instanceof Error ? error.message : 'Login failed';
            set({
              user: null,
              isAuthenticated: false,
              isLoading: false,
              error: message,
            });
            throw error;
          }
        },

        signup: async (credentials) => {
          set({ isLoading: true, error: null });
          try {
            const response = await authApi.signup(credentials);
            set({
              user: response.user,
              isAuthenticated: true,
              isLoading: false,
              error: null,
            });
          } catch (error) {
            const message = error instanceof Error ? error.message : 'Signup failed';
            set({
              user: null,
              isAuthenticated: false,
              isLoading: false,
              error: message,
            });
            throw error;
          }
        },

        logout: async () => {
          set({ isLoading: true });
          try {
            await authApi.logout();
            set({
              ...initialState,
            });
          } catch {
            // Clear state even if API call fails
            set({
              ...initialState,
            });
          }
        },

        loadUser: async () => {
          // Check if token exists
          if (!authApi.isAuthenticated()) {
            set(initialState);
            return;
          }

          set({ isLoading: true });
          try {
            const user = await authApi.getCurrentUser();
            set({
              user,
              isAuthenticated: true,
              isLoading: false,
              error: null,
            });
          } catch {
            set({
              ...initialState,
            });
          }
        },

        clearError: () => {
          set({ error: null });
        },
      }),
      {
        name: 'auth-storage',
        partialize: (state) => ({
          user: state.user,
          isAuthenticated: state.isAuthenticated,
        }),
      }
    ),
    { name: 'AuthStore' }
  )
);
