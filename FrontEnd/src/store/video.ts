/**
 * Video player store using Zustand
 * @module store/video
 */

import { create } from 'zustand';
import { devtools } from 'zustand/middleware';

interface VideoState {
  currentVideoId: string | null;
  playingVideoId: string | null;
  volume: number;
  isMuted: boolean;
}

interface VideoActions {
  setCurrentVideo: (videoId: string) => void;
  setPlayingVideo: (videoId: string | null) => void;
  setVolume: (volume: number) => void;
  toggleMute: () => void;
}

type VideoStore = VideoState & VideoActions;

const initialState: VideoState = {
  currentVideoId: null,
  playingVideoId: null,
  volume: 0.7,
  isMuted: false,
};

/**
 * Video player store
 */
export const useVideoStore = create<VideoStore>()(
  devtools(
    (set) => ({
      ...initialState,

      setCurrentVideo: (videoId) => {
        set({ currentVideoId: videoId });
      },

      setPlayingVideo: (videoId) => {
        set({ playingVideoId: videoId });
      },

      setVolume: (volume) => {
        set({ volume: Math.max(0, Math.min(1, volume)), isMuted: volume === 0 });
      },

      toggleMute: () => {
        set((state) => ({ isMuted: !state.isMuted }));
      },
    }),
    { name: 'VideoStore' }
  )
);
