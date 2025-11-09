/**
 * Video Actions Component - Like, Comment, Share buttons
 * @module components/VideoPlayer/VideoActions
 */

'use client';

import { memo, useState, useCallback } from 'react';
import { Video } from '@/lib/types';
import { toggleLike } from '@/lib/api/videos';
import { cn, formatNumber } from '@/lib/utils';
import { useAuthStore } from '@/store/auth';

interface VideoActionsProps {
  video: Video;
  className?: string;
}

/**
 * Video Actions Component
 */
const VideoActions = memo<VideoActionsProps>(({ video, className }) => {
  const { isAuthenticated } = useAuthStore();
  const [likesCount, setLikesCount] = useState(video.likesCount);
  const [isLiked, setIsLiked] = useState(video.isLiked || false);
  const [isLiking, setIsLiking] = useState(false);

  const handleLike = useCallback(async () => {
    if (!isAuthenticated || isLiking) return;

    setIsLiking(true);
    // Optimistic update
    setIsLiked(!isLiked);
    setLikesCount(isLiked ? likesCount - 1 : likesCount + 1);

    try {
      const result = await toggleLike(video.id);
      setIsLiked(result.isLiked);
      setLikesCount(result.likesCount);
    } catch (error) {
      // Revert on error
      setIsLiked(isLiked);
      setLikesCount(likesCount);
      console.error('Failed to toggle like:', error);
    } finally {
      setIsLiking(false);
    }
  }, [video.id, isAuthenticated, isLiked, likesCount, isLiking]);

  const handleComment = useCallback(() => {
    // TODO: Open comments modal
    console.log('Open comments for video:', video.id);
  }, [video.id]);

  const handleShare = useCallback(() => {
    // TODO: Open share modal
    if (navigator.share) {
      navigator.share({
        title: video.caption,
        url: window.location.href,
      }).catch(console.error);
    }
  }, [video.caption]);

  return (
    <div className={cn('flex flex-col items-center space-y-6 text-white', className)}>
      {/* Like Button */}
      <button
        onClick={handleLike}
        disabled={!isAuthenticated || isLiking}
        className="flex flex-col items-center space-y-1 transition-transform hover:scale-110 disabled:opacity-50"
        aria-label={isLiked ? 'Unlike' : 'Like'}
      >
        <div className="w-12 h-12 rounded-full bg-gray-800/50 flex items-center justify-center">
          <svg
            className={cn('w-7 h-7 transition-colors', isLiked ? 'fill-red-500' : 'fill-white')}
            viewBox="0 0 20 20"
          >
            <path d="M3.172 5.172a4 4 0 015.656 0L10 6.343l1.172-1.171a4 4 0 115.656 5.656L10 17.657l-6.828-6.829a4 4 0 010-5.656z" />
          </svg>
        </div>
        <span className="text-xs font-semibold">{formatNumber(likesCount)}</span>
      </button>

      {/* Comment Button */}
      <button
        onClick={handleComment}
        className="flex flex-col items-center space-y-1 transition-transform hover:scale-110"
        aria-label="Comment"
      >
        <div className="w-12 h-12 rounded-full bg-gray-800/50 flex items-center justify-center">
          <svg className="w-7 h-7 fill-white" viewBox="0 0 20 20">
            <path
              fillRule="evenodd"
              d="M18 10c0 3.866-3.582 7-8 7a8.841 8.841 0 01-4.083-.98L2 17l.92-3.917A6.959 6.959 0 012 10c0-3.866 3.582-7 8-7s8 3.134 8 7zM7 9H5v2h2V9zm8 0h-2v2h2V9zM9 9h2v2H9V9z"
              clipRule="evenodd"
            />
          </svg>
        </div>
        <span className="text-xs font-semibold">{formatNumber(video.commentsCount)}</span>
      </button>

      {/* Share Button */}
      <button
        onClick={handleShare}
        className="flex flex-col items-center space-y-1 transition-transform hover:scale-110"
        aria-label="Share"
      >
        <div className="w-12 h-12 rounded-full bg-gray-800/50 flex items-center justify-center">
          <svg className="w-7 h-7 fill-white" viewBox="0 0 20 20">
            <path d="M15 8a3 3 0 10-2.977-2.63l-4.94 2.47a3 3 0 100 4.319l4.94 2.47a3 3 0 10.895-1.789l-4.94-2.47a3.027 3.027 0 000-.74l4.94-2.47C13.456 7.68 14.19 8 15 8z" />
          </svg>
        </div>
        <span className="text-xs font-semibold">{formatNumber(video.sharesCount || 0)}</span>
      </button>
    </div>
  );
});

VideoActions.displayName = 'VideoActions';

export default VideoActions;
