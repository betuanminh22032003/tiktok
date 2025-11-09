/**
 * Custom hook for video player controls
 * @module hooks/useVideoPlayer
 */

import { useCallback, useEffect, useRef, useState } from 'react';
import { useVideoStore } from '@/store/video';

interface UseVideoPlayerOptions {
  videoId: string;
  autoPlay?: boolean;
  onPlay?: () => void;
  onPause?: () => void;
  onEnded?: () => void;
}

interface UseVideoPlayerReturn {
  videoRef: React.RefObject<HTMLVideoElement | null>;
  isPlaying: boolean;
  currentTime: number;
  duration: number;
  volume: number;
  isMuted: boolean;
  play: () => Promise<void>;
  pause: () => void;
  togglePlay: () => Promise<void>;
  seek: (time: number) => void;
  setVideoVolume: (volume: number) => void;
  toggleMute: () => void;
}

/**
 * Hook to manage video player state and controls
 */
export function useVideoPlayer(
  options: UseVideoPlayerOptions
): UseVideoPlayerReturn {
  const { videoId, autoPlay = false, onPlay, onPause, onEnded } = options;
  
  const videoRef = useRef<HTMLVideoElement>(null);
  const [isPlaying, setIsPlaying] = useState(false);
  const [currentTime, setCurrentTime] = useState(0);
  const [duration, setDuration] = useState(0);
  
  const { volume, isMuted, setVolume, toggleMute: toggleStoreMute, setPlayingVideo } = useVideoStore();

  // Play video
  const play = useCallback(async () => {
    const video = videoRef.current;
    if (!video) return;

    try {
      await video.play();
      setIsPlaying(true);
      setPlayingVideo(videoId);
      onPlay?.();
    } catch (error) {
      console.error('Error playing video:', error);
    }
  }, [videoId, setPlayingVideo, onPlay]);

  // Pause video
  const pause = useCallback(() => {
    const video = videoRef.current;
    if (!video) return;

    video.pause();
    setIsPlaying(false);
    setPlayingVideo(null);
    onPause?.();
  }, [setPlayingVideo, onPause]);

  // Toggle play/pause
  const togglePlay = useCallback(async () => {
    if (isPlaying) {
      pause();
    } else {
      await play();
    }
  }, [isPlaying, play, pause]);

  // Seek to specific time
  const seek = useCallback((time: number) => {
    const video = videoRef.current;
    if (!video) return;

    video.currentTime = Math.max(0, Math.min(time, duration));
  }, [duration]);

  // Set volume
  const setVideoVolume = useCallback((newVolume: number) => {
    const video = videoRef.current;
    if (!video) return;

    const clampedVolume = Math.max(0, Math.min(1, newVolume));
    video.volume = clampedVolume;
    setVolume(clampedVolume);
  }, [setVolume]);

  // Toggle mute
  const toggleMute = useCallback(() => {
    const video = videoRef.current;
    if (!video) return;

    video.muted = !isMuted;
    toggleStoreMute();
  }, [isMuted, toggleStoreMute]);

  // Set up event listeners
  useEffect(() => {
    const video = videoRef.current;
    if (!video) return;

    const handleTimeUpdate = () => {
      setCurrentTime(video.currentTime);
    };

    const handleDurationChange = () => {
      setDuration(video.duration);
    };

    const handleEnded = () => {
      setIsPlaying(false);
      setPlayingVideo(null);
      onEnded?.();
    };

    const handlePlay = () => {
      setIsPlaying(true);
      setPlayingVideo(videoId);
      onPlay?.();
    };

    const handlePause = () => {
      setIsPlaying(false);
      setPlayingVideo(null);
      onPause?.();
    };

    video.addEventListener('timeupdate', handleTimeUpdate);
    video.addEventListener('durationchange', handleDurationChange);
    video.addEventListener('ended', handleEnded);
    video.addEventListener('play', handlePlay);
    video.addEventListener('pause', handlePause);

    return () => {
      video.removeEventListener('timeupdate', handleTimeUpdate);
      video.removeEventListener('durationchange', handleDurationChange);
      video.removeEventListener('ended', handleEnded);
      video.removeEventListener('play', handlePlay);
      video.removeEventListener('pause', handlePause);
    };
  }, [videoId, setPlayingVideo, onPlay, onPause, onEnded]);

  // Sync volume and mute with store
  useEffect(() => {
    const video = videoRef.current;
    if (!video) return;

    video.volume = volume;
    video.muted = isMuted;
  }, [volume, isMuted]);

  // Auto-play if enabled
  useEffect(() => {
    if (autoPlay && videoRef.current) {
      void play();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [autoPlay]);

  return {
    videoRef,
    isPlaying,
    currentTime,
    duration,
    volume,
    isMuted,
    play,
    pause,
    togglePlay,
    seek,
    setVideoVolume,
    toggleMute,
  };
}
