/**
 * Custom hook for video intersection observer
 * @module hooks/useVideoInView
 */

import { useEffect, useRef, useState, RefObject } from 'react';

interface UseVideoInViewOptions {
  threshold?: number;
  onEnter?: () => void;
  onExit?: () => void;
}

interface UseVideoInViewReturn {
  ref: RefObject<HTMLVideoElement | null>;
  isInView: boolean;
}

/**
 * Hook to detect when a video element enters/exits viewport
 */
export function useVideoInView(
  options: UseVideoInViewOptions = {}
): UseVideoInViewReturn {
  const { threshold = 0.5, onEnter, onExit } = options;
  const ref = useRef<HTMLVideoElement>(null);
  const [isInView, setIsInView] = useState(false);

  useEffect(() => {
    const element = ref.current;
    if (!element) return;

    const observer = new IntersectionObserver(
      ([entry]) => {
        const inView = entry.isIntersecting;
        setIsInView(inView);

        if (inView) {
          onEnter?.();
        } else {
          onExit?.();
        }
      },
      {
        threshold,
        rootMargin: '0px',
      }
    );

    observer.observe(element);

    return () => {
      observer.unobserve(element);
    };
  }, [threshold, onEnter, onExit]);

  return { ref, isInView };
}
