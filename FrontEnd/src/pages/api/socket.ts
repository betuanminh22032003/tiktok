// src/pages/api/socket.ts
import { Server } from 'socket.io';
import type { NextApiRequest, NextApiResponse } from 'next';
import type { Server as HTTPServer } from 'http';
import type { Socket as NetSocket } from 'net';

interface SocketServer extends HTTPServer {
  io?: Server;
}

interface SocketNextApiResponse extends NextApiResponse {
  socket: NetSocket & {
    server: SocketServer;
  };
}

const SocketHandler = (req: NextApiRequest, res: SocketNextApiResponse) => {
  if (res.socket.server.io) {
    console.log('Socket is already running');
  } else {
    console.log('Socket is initializing');
    const io = new Server(res.socket.server);
    res.socket.server.io = io;

    io.on('connection', (socket) => {
      console.log('A user connected:', socket.id);

      socket.on('disconnect', () => {
        console.log('A user disconnected:', socket.id);
      });

      socket.on('like', (videoId: string) => {
        console.log(`Received like for video ${videoId}`);
        // In a real app, you would update the like count in the database
        // and then broadcast the new count to all clients.
        io.emit('likeUpdate', { videoId, likes: Math.floor(Math.random() * 100) });
      });

      socket.on('comment', (data: { videoId: string; comment: string }) => {
        console.log(`Received comment for video ${data.videoId}: ${data.comment}`);
        // In a real app, you would save the comment to the database
        // and then broadcast the new comment to all clients.
        io.emit('commentUpdate', {
          videoId: data.videoId,
          comment: {
            id: new Date().toISOString(),
            text: data.comment,
            user: 'Anonymous', // In a real app, you'd get the user from the socket
          },
        });
      });
    });
  }
  res.end();
};

export const config = {
  api: {
    bodyParser: false,
  },
};

export default SocketHandler;
