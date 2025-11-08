// src/components/Video.tsx
'use client';

import { useEffect, useRef, useState } from 'react';
import io, { Socket } from 'socket.io-client';

interface VideoPlayerProps {
  id: string;
  src: string;
  poster?: string;
}

const VideoPlayer: React.FC<VideoPlayerProps> = ({ id, src, poster }) => {
  const videoRef = useRef<HTMLVideoElement>(null);
  const [isPlaying, setIsPlaying] = useState(false);
  const [likes, setLikes] = useState(() => Math.floor(Math.random() * 100));
  const [comments, setComments] = useState<{ id: string; text: string; user: string }[]>([]);
  const [socket] = useState<Socket | null>(() => io());

  useEffect(() => {
    if (!socket) return;

    socket.on('connect', () => {
      console.log('Socket connected');
    });

    socket.on('likeUpdate', (data: { videoId: string; likes: number }) => {
      if (data.videoId === id) {
        setLikes(data.likes);
      }
    });

    socket.on('commentUpdate', (data: { videoId: string; comment: { id: string; text: string; user: string } }) => {
      if (data.videoId === id) {
        setComments((prevComments) => [...prevComments, data.comment]);
      }
    });

    return () => {
      socket.disconnect();
    };
  }, [id, socket]);

  const handleLike = () => {
    socket?.emit('like', id);
  };

  const handleComment = (commentText: string) => {
    socket?.emit('comment', { videoId: id, comment: commentText });
  };

  const handleVideoPress = () => {
    if (videoRef.current) {
      if (isPlaying) {
        videoRef.current.pause();
        setIsPlaying(false);
      } else {
        videoRef.current.play();
        setIsPlaying(true);
      }
    }
  };

  useEffect(() => {
    const video = videoRef.current;
    if (!video) return;

    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          video.play();
          setIsPlaying(true);
        } else {
          video.pause();
          setIsPlaying(false);
        }
      },
      {
        threshold: 0.5,
      }
    );

    observer.observe(video);

    return () => {
      if (video) {
        observer.unobserve(video);
      }
    };
  }, []);

  return (
    <div className="relative w-full h-full snap-start">
      <video
        ref={videoRef}
        onClick={handleVideoPress}
        className="w-full h-full object-cover"
        src={src}
        poster={poster}
        loop
        playsInline
      />
      <div className="absolute bottom-20 right-2 text-white">
        <button onClick={handleLike} className="p-2">
          <svg className="w-8 h-8" fill="currentColor" viewBox="0 0 20 20"><path d="M3.172 5.172a4 4 0 015.656 0L10 6.343l1.172-1.171a4 4 0 115.656 5.656L10 17.657l-6.828-6.829a4 4 0 010-5.656z" /></svg>
          <span>{likes}</span>
        </button>
        <div className="p-2">
          <svg className="w-8 h-8" fill="currentColor" viewBox="0 0 20 20"><path d="M18 10c0 3.866-3.582 7-8 7a8.94 8.94 0 01-4.43-1.232l-2.14 2.14a1 1 0 01-1.414-1.414l2.14-2.14A8.94 8.94 0 012 10c0-3.866 3.582-7 8-7s8 3.134 8 7zM4.5 9a1.5 1.5 0 100-3 1.5 1.5 0 000 3zm3 0a1.5 1.5 0 100-3 1.5 1.5 0 000 3zm6.5-1.5a1.5 1.5 0 10-3 0 1.5 1.5 0 003 0z" /></svg>
          <span>{comments.length}</span>
        </div>
      </div>
      {/* Simple comment form for demo */}
      <div className="absolute bottom-5 right-20 text-white">
        <form onSubmit={(e) => {
          e.preventDefault();
          const input = e.currentTarget.elements.namedItem('comment') as HTMLInputElement;
          if (input.value) {
            handleComment(input.value);
            input.value = '';
          }
        }}>
          <input name="comment" placeholder="Add a comment..." className="bg-transparent border-b" />
          <button type="submit" className="hidden">Send</button>
        </form>
      </div>
    </div>
  );
};

export default VideoPlayer;

