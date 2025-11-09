/**
 * Custom hook for Socket.IO connection
 * @module hooks/useSocket
 */

import { useEffect, useRef, useState } from 'react';
import { Socket } from 'socket.io-client';
import { initializeSocket, connectSocket, disconnectSocket, getSocket } from '@/services/socket';

interface UseSocketReturn {
  socket: Socket | null;
  isConnected: boolean;
}

/**
 * Hook to manage socket connection lifecycle
 */
export function useSocket(): UseSocketReturn {
  const socketRef = useRef<Socket | null>(null);
  const [isConnected, setIsConnected] = useState(false);

  useEffect(() => {
    // Initialize and connect socket
    const socketInstance = initializeSocket();
    socketRef.current = socketInstance;

    const handleConnect = () => {
      setIsConnected(true);
    };

    const handleDisconnect = () => {
      setIsConnected(false);
    };

    socketInstance.on('connect', handleConnect);
    socketInstance.on('disconnect', handleDisconnect);

    connectSocket();

    // Cleanup
    return () => {
      socketInstance.off('connect', handleConnect);
      socketInstance.off('disconnect', handleDisconnect);
      disconnectSocket();
    };
  }, []);

  return {
    socket: getSocket(),
    isConnected,
  };
}

/**
 * Hook to emit and listen to socket events for a specific video
 */
export function useVideoSocket() {
  const { socket, isConnected } = useSocket();

  const emit = (event: string, data?: unknown) => {
    if (socket && isConnected) {
      socket.emit(event, data);
    }
  };

  const on = (event: string, handler: (...args: unknown[]) => void) => {
    if (socket) {
      socket.on(event, handler);
      return () => {
        socket.off(event, handler);
      };
    }
    return () => {};
  };

  return {
    socket,
    isConnected,
    emit,
    on,
  };
}

/**
 * Hook to listen to socket events
 */
export function useSocketEvent<T = unknown>(
  event: string,
  handler: (data: T) => void
): void {
  const socket = getSocket();

  useEffect(() => {
    if (!socket) return;

    const wrappedHandler = (data: T) => {
      handler(data);
    };

    socket.on(event, wrappedHandler);

    return () => {
      socket.off(event, wrappedHandler);
    };
  }, [event, handler, socket]);
}
