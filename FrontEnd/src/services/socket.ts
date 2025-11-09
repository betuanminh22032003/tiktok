/**
 * Socket.IO client service
 * @module services/socket
 */

import { io, Socket } from 'socket.io-client';
import { APP_CONFIG, SOCKET_EVENTS } from '@/lib/constants';
import { SocketLikeEvent, SocketCommentEvent, SocketViewEvent } from '@/lib/types';

/**
 * Socket.IO client instance
 */
let socketInstance: Socket | null = null;

/**
 * Initialize socket connection
 */
export function initializeSocket(): Socket {
  if (socketInstance?.connected) {
    return socketInstance;
  }

  socketInstance = io(APP_CONFIG.SOCKET_URL, {
    autoConnect: false,
    reconnection: true,
    reconnectionDelay: 1000,
    reconnectionDelayMax: 5000,
    reconnectionAttempts: 5,
  });

  // Connection event handlers
  socketInstance.on(SOCKET_EVENTS.CONNECT, () => {
    console.log('[Socket] Connected:', socketInstance?.id);
  });

  socketInstance.on(SOCKET_EVENTS.DISCONNECT, (reason) => {
    console.log('[Socket] Disconnected:', reason);
  });

  socketInstance.on('connect_error', (error) => {
    console.error('[Socket] Connection error:', error.message);
  });

  return socketInstance;
}

/**
 * Get socket instance
 */
export function getSocket(): Socket | null {
  return socketInstance;
}

/**
 * Connect socket
 */
export function connectSocket(): void {
  if (!socketInstance) {
    initializeSocket();
  }
  socketInstance?.connect();
}

/**
 * Disconnect socket
 */
export function disconnectSocket(): void {
  socketInstance?.disconnect();
}

/**
 * Emit like event
 */
export function emitLike(videoId: string): void {
  socketInstance?.emit(SOCKET_EVENTS.LIKE, { videoId });
}

/**
 * Emit comment event
 */
export function emitComment(videoId: string, text: string): void {
  socketInstance?.emit(SOCKET_EVENTS.COMMENT, { videoId, text });
}

/**
 * Emit view count event
 */
export function emitViewCount(videoId: string): void {
  socketInstance?.emit(SOCKET_EVENTS.VIEW_COUNT, { videoId });
}

/**
 * Subscribe to like updates
 */
export function onLikeUpdate(callback: (data: SocketLikeEvent) => void): () => void {
  socketInstance?.on(SOCKET_EVENTS.LIKE_UPDATE, callback);
  return () => {
    socketInstance?.off(SOCKET_EVENTS.LIKE_UPDATE, callback);
  };
}

/**
 * Subscribe to comment updates
 */
export function onCommentUpdate(callback: (data: SocketCommentEvent) => void): () => void {
  socketInstance?.on(SOCKET_EVENTS.COMMENT_UPDATE, callback);
  return () => {
    socketInstance?.off(SOCKET_EVENTS.COMMENT_UPDATE, callback);
  };
}

/**
 * Subscribe to view count updates
 */
export function onViewUpdate(callback: (data: SocketViewEvent) => void): () => void {
  socketInstance?.on(SOCKET_EVENTS.VIEW_COUNT, callback);
  return () => {
    socketInstance?.off(SOCKET_EVENTS.VIEW_COUNT, callback);
  };
}

/**
 * Join a video room
 */
export function joinVideoRoom(videoId: string): void {
  socketInstance?.emit('join:video', { videoId });
}

/**
 * Leave a video room
 */
export function leaveVideoRoom(videoId: string): void {
  socketInstance?.emit('leave:video', { videoId });
}
