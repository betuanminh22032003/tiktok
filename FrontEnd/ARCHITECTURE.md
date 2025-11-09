/**
 * Project Structure Documentation
 * @module ARCHITECTURE.md
 */

# TikTok Clone - Frontend Architecture

## Technology Stack

- **Framework**: Next.js 16 (App Router)
- **Language**: TypeScript
- **Styling**: TailwindCSS 4
- **State Management**: Zustand
- **Data Fetching**: SWR
- **HTTP Client**: Axios
- **Real-time**: Socket.IO Client
- **Video Player**: HTML5 Video

## Project Structure

```
src/
├── app/                      # Next.js App Router
│   ├── layout.tsx           # Root layout
│   ├── page.tsx             # Home page (Feed)
│   ├── login/               # Login page
│   ├── signup/              # Signup page
│   └── upload/              # Upload page
│
├── components/              # React components
│   ├── Feed/               # Video feed with infinite scroll
│   ├── VideoPlayer/        # Video player components
│   │   ├── VideoPlayer.tsx # Main video player
│   │   ├── VideoControls.tsx # Playback controls
│   │   ├── VideoActions.tsx # Like, comment, share
│   │   └── VideoInfo.tsx   # Video metadata
│   └── Upload/             # Video upload component
│
├── hooks/                   # Custom React hooks
│   ├── useSocket.ts        # Socket.IO connection
│   ├── useVideoPlayer.ts   # Video player controls
│   ├── useVideoInView.ts   # Intersection observer
│   ├── useVideos.ts        # Video data fetching
│   └── useComments.ts      # Comments data fetching
│
├── store/                   # Zustand stores
│   ├── auth.ts             # Authentication state
│   ├── video.ts            # Video player state
│   └── upload.ts           # Upload state
│
├── lib/                     # Core utilities
│   ├── api/                # API client layer
│   │   ├── client.ts       # Axios configuration
│   │   ├── videos.ts       # Video API
│   │   ├── comments.ts     # Comments API
│   │   ├── auth.ts         # Auth API
│   │   └── upload.ts       # Upload API
│   ├── types.ts            # TypeScript types
│   ├── constants.ts        # App constants
│   └── utils.ts            # Utility functions
│
└── services/               # External services
    └── socket.ts           # Socket.IO service

```

## Design Patterns

### 1. **Separation of Concerns**
- **Components**: Pure UI components, no business logic
- **Hooks**: Reusable logic, state management
- **Services**: External API/Socket interactions
- **Store**: Global state management

### 2. **API Layer Architecture**
```
Component → Hook → API Service → HTTP Client → Backend
```

### 3. **State Management Strategy**
- **Local State**: `useState` for component-specific state
- **Global State**: Zustand stores for app-wide state
- **Server State**: SWR for data fetching and caching

### 4. **Error Handling**
- Custom `ApiClientError` class
- Axios interceptors for global error handling
- Component-level error boundaries

## Key Features

### Video Player
- Auto-play on scroll with Intersection Observer
- Custom controls (play/pause, seek, volume)
- Optimized performance with `memo` and `useCallback`
- Smooth scroll with CSS snap points

### Infinite Scroll
- SWR infinite loading
- Intersection Observer for trigger
- Optimistic UI updates
- Loading states management

### Real-time Features
- Socket.IO integration
- Like/comment live updates
- View count tracking
- Reconnection handling

### File Upload
- Drag and drop support
- File validation
- Progress tracking
- Preview before upload

## Best Practices

1. **Type Safety**: Full TypeScript coverage
2. **Code Organization**: Modular structure with clear boundaries
3. **Performance**: Memoization, lazy loading, code splitting
4. **Accessibility**: ARIA labels, keyboard navigation
5. **Error Handling**: Graceful degradation
6. **Documentation**: JSDoc comments for all modules

## Environment Variables

```env
NEXT_PUBLIC_API_URL=http://localhost:3000
NEXT_PUBLIC_SOCKET_URL=http://localhost:3000
```

## Running the Project

```bash
# Install dependencies
npm install

# Development
npm run dev

# Production build
npm run build
npm start

# Linting
npm run lint
```

## Component Guidelines

### Creating New Components
1. Use TypeScript with proper interfaces
2. Add JSDoc comments
3. Use `memo` for optimization
4. Extract reusable logic to hooks
5. Keep components small and focused

### Example Component Structure
```typescript
/**
 * Component description
 * @module components/ComponentName
 */

'use client';

import { memo } from 'react';

interface ComponentProps {
  prop1: string;
  prop2?: number;
}

const Component = memo<ComponentProps>(({ prop1, prop2 }) => {
  // Component logic
  return (
    <div>
      {/* JSX */}
    </div>
  );
});

Component.displayName = 'Component';

export default Component;
```

## Testing Strategy

- Unit tests for utilities and hooks
- Integration tests for API services
- Component tests with React Testing Library
- E2E tests for critical user flows

## Future Enhancements

1. Comments modal implementation
2. User profile pages
3. Search functionality
4. Notifications system
5. Video editing features
6. Analytics dashboard
7. PWA support
8. Dark mode theme
