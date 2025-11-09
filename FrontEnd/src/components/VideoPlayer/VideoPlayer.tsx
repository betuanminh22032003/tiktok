/**
 * Video Player Component
 * @module components/VideoPlayer
 */

'use client';

import { memo, useCallback, useEffect } from 'react';
import { Video as VideoType } from '@/lib/types';
import { useVideoPlayer } from '@/hooks/useVideoPlayer';
import { useVideoInView } from '@/hooks/useVideoInView';
import VideoControls from './VideoControls';
import VideoActions from './VideoActions';
import VideoInfo from './VideoInfo';

interface VideoPlayerProps {
  video: VideoType;
  onVideoInView?: (videoId: string) => void;
}

/**
 * Video Player Component with auto-play on scroll
 */
const VideoPlayer = memo<VideoPlayerProps>(({ video, onVideoInView }) => {
  const {
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
  } = useVideoPlayer({
    videoId: video.id,
  });

  const { isInView } = useVideoInView({
    threshold: 0.65,
  });

  // Auto-play/pause based on visibility
  useEffect(() => {
    if (isInView) {
      void play();
      onVideoInView?.(video.id);
    } else {
      pause();
    }
  }, [isInView, play, pause, video.id, onVideoInView]);

  // Merge callback for both intersection observer and video element
  const containerRef = useCallback(
    (node: HTMLDivElement | null) => {
      if (node) {
        const videoElement = node.querySelector('video') as HTMLVideoElement | null;
        videoRef.current = videoElement;
      }
    },
    [videoRef]
  );

  return (
    <div ref={containerRef} className="relative w-full h-full snap-start bg-black">
      {/* Video Element */}
      <video
        className="w-full h-full object-contain cursor-pointer"
        src={video.url}
        poster={video.thumbnailUrl}
        loop
        playsInline
        onClick={togglePlay}
        preload="metadata"
      />

      {/* Play/Pause Overlay */}
      {!isPlaying && (
        <button
          onClick={togglePlay}
          className="absolute inset-0 flex items-center justify-center bg-black/20 transition-opacity hover:bg-black/30"
          aria-label="Play video"
        >
          <div className="w-20 h-20 rounded-full bg-white/90 flex items-center justify-center">
            <svg
              className="w-10 h-10 text-black ml-1"
              fill="currentColor"
              viewBox="0 0 20 20"
            >
              <path d="M6.3 2.841A1.5 1.5 0 004 4.11V15.89a1.5 1.5 0 002.3 1.269l9.344-5.89a1.5 1.5 0 000-2.538L6.3 2.84z" />
            </svg>
          </div>
        </button>
      )}

      {/* Video Info */}
      <VideoInfo
        video={video}
        className="absolute bottom-20 left-4 right-20"
      />

      {/* Video Actions (Like, Comment, Share) */}
      <VideoActions
        video={video}
        className="absolute bottom-20 right-4"
      />

      {/* Video Controls */}
      <VideoControls
        currentTime={currentTime}
        duration={duration}
        volume={volume}
        isMuted={isMuted}
        onSeek={seek}
        onVolumeChange={setVideoVolume}
        onToggleMute={toggleMute}
        className="absolute bottom-2 left-0 right-0 px-4"
      />
    </div>
  );
});

VideoPlayer.displayName = 'VideoPlayer';

export default VideoPlayer;
