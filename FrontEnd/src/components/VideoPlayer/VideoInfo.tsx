/**
 * Video Info Component - displays video caption and user info
 * @module components/VideoPlayer/VideoInfo
 */

'use client';

import { memo } from 'react';
import Link from 'next/link';
import { Video } from '@/lib/types';
import { cn } from '@/lib/utils';

interface VideoInfoProps {
  video: Video;
  className?: string;
}

/**
 * Video Info Component
 */
const VideoInfo = memo<VideoInfoProps>(({ video, className }) => {
  return (
    <div className={cn('text-white space-y-2', className)}>
      {/* User Info */}
      <Link
        href={`/user/${video.user.id}`}
        className="flex items-center space-x-2 hover:opacity-80 transition-opacity"
      >
        {video.user.avatar && (
          // eslint-disable-next-line @next/next/no-img-element
          <img
            src={video.user.avatar}
            alt={video.user.username}
            className="w-10 h-10 rounded-full object-cover border-2 border-white"
          />
        )}
        <span className="font-semibold text-lg">@{video.user.username}</span>
      </Link>

      {/* Caption */}
      {video.caption && (
        <p className="text-sm line-clamp-2">{video.caption}</p>
      )}
    </div>
  );
});

VideoInfo.displayName = 'VideoInfo';

export default VideoInfo;
