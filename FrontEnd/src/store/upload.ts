/**
 * Upload store using Zustand
 * @module store/upload
 */

import { create } from 'zustand';
import { devtools } from 'zustand/middleware';

interface UploadState {
  isUploading: boolean;
  progress: number;
  error: string | null;
}

interface UploadActions {
  setProgress: (progress: number) => void;
  setUploading: (isUploading: boolean) => void;
  setError: (error: string | null) => void;
  reset: () => void;
}

type UploadStore = UploadState & UploadActions;

const initialState: UploadState = {
  isUploading: false,
  progress: 0,
  error: null,
};

/**
 * Upload store
 */
export const useUploadStore = create<UploadStore>()(
  devtools(
    (set) => ({
      ...initialState,

      setProgress: (progress) => {
        set({ progress });
      },

      setUploading: (isUploading) => {
        set({ isUploading });
      },

      setError: (error) => {
        set({ error, isUploading: false });
      },

      reset: () => {
        set(initialState);
      },
    }),
    { name: 'UploadStore' }
  )
);
