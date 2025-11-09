/**
 * Video Controls Component - Progress bar and volume controls
 * @module components/VideoPlayer/VideoControls
 */

'use client';

import { memo, useCallback, useRef } from 'react';
import { cn, formatDuration } from '@/lib/utils';

interface VideoControlsProps {
  currentTime: number;
  duration: number;
  volume: number;
  isMuted: boolean;
  onSeek: (time: number) => void;
  onVolumeChange: (volume: number) => void;
  onToggleMute: () => void;
  className?: string;
}

/**
 * Video Controls Component
 */
const VideoControls = memo<VideoControlsProps>(({
  currentTime,
  duration,
  volume,
  isMuted,
  onSeek,
  onVolumeChange,
  onToggleMute,
  className,
}) => {
  const progressRef = useRef<HTMLDivElement>(null);
  const volumeRef = useRef<HTMLDivElement>(null);

  const handleProgressClick = useCallback(
    (e: React.MouseEvent<HTMLDivElement>) => {
      if (!progressRef.current) return;
      
      const rect = progressRef.current.getBoundingClientRect();
      const percent = (e.clientX - rect.left) / rect.width;
      const time = percent * duration;
      onSeek(time);
    },
    [duration, onSeek]
  );

  const handleVolumeClick = useCallback(
    (e: React.MouseEvent<HTMLDivElement>) => {
      if (!volumeRef.current) return;
      
      const rect = volumeRef.current.getBoundingClientRect();
      const percent = (e.clientX - rect.left) / rect.width;
      onVolumeChange(percent);
    },
    [onVolumeChange]
  );

  const progress = duration > 0 ? (currentTime / duration) * 100 : 0;

  return (
    <div className={cn('flex items-center space-x-3 text-white', className)}>
      {/* Time */}
      <span className="text-xs font-medium min-w-20">
        {formatDuration(currentTime)} / {formatDuration(duration)}
      </span>

      {/* Progress Bar */}
      <div
        ref={progressRef}
        onClick={handleProgressClick}
        className="flex-1 h-1 bg-white/30 rounded-full cursor-pointer hover:h-1.5 transition-all"
      >
        <div
          className="h-full bg-white rounded-full transition-all"
          style={{ width: `${progress}%` }}
        />
      </div>

      {/* Volume Control */}
      <button
        onClick={onToggleMute}
        className="p-1 hover:bg-white/10 rounded transition-colors"
        aria-label={isMuted ? 'Unmute' : 'Mute'}
      >
        {isMuted || volume === 0 ? (
          <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
            <path
              fillRule="evenodd"
              d="M9.383 3.076A1 1 0 0110 4v12a1 1 0 01-1.707.707L4.586 13H2a1 1 0 01-1-1V8a1 1 0 011-1h2.586l3.707-3.707a1 1 0 011.09-.217zM12.293 7.293a1 1 0 011.414 0L15 8.586l1.293-1.293a1 1 0 111.414 1.414L16.414 10l1.293 1.293a1 1 0 01-1.414 1.414L15 11.414l-1.293 1.293a1 1 0 01-1.414-1.414L13.586 10l-1.293-1.293a1 1 0 010-1.414z"
              clipRule="evenodd"
            />
          </svg>
        ) : (
          <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
            <path
              fillRule="evenodd"
              d="M9.383 3.076A1 1 0 0110 4v12a1 1 0 01-1.707.707L4.586 13H2a1 1 0 01-1-1V8a1 1 0 011-1h2.586l3.707-3.707a1 1 0 011.09-.217zM14.657 2.929a1 1 0 011.414 0A9.972 9.972 0 0119 10a9.972 9.972 0 01-2.929 7.071 1 1 0 01-1.414-1.414A7.971 7.971 0 0017 10c0-2.21-.894-4.208-2.343-5.657a1 1 0 010-1.414zm-2.829 2.828a1 1 0 011.415 0A5.983 5.983 0 0115 10a5.984 5.984 0 01-1.757 4.243 1 1 0 01-1.415-1.415A3.984 3.984 0 0013 10a3.983 3.983 0 00-1.172-2.828 1 1 0 010-1.415z"
              clipRule="evenodd"
            />
          </svg>
        )}
      </button>

      {/* Volume Bar */}
      <div
        ref={volumeRef}
        onClick={handleVolumeClick}
        className="w-16 h-1 bg-white/30 rounded-full cursor-pointer hover:h-1.5 transition-all"
      >
        <div
          className="h-full bg-white rounded-full transition-all"
          style={{ width: `${isMuted ? 0 : volume * 100}%` }}
        />
      </div>
    </div>
  );
});

VideoControls.displayName = 'VideoControls';

export default VideoControls;
