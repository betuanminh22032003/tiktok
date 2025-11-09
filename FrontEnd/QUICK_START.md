# 🎯 Quick Start Guide

## Getting Started in 5 Minutes

### 1. Install Dependencies
```bash
cd FrontEnd
npm install
```

### 2. Set Up Environment
```bash
cp .env.example .env.local
```

Edit `.env.local`:
```env
NEXT_PUBLIC_API_URL=http://localhost:3000
NEXT_PUBLIC_SOCKET_URL=http://localhost:3000
```

### 3. Run Development Server
```bash
npm run dev
```

Visit: `http://localhost:3000`

---

## 📁 Project Structure Overview

```
src/
├── app/                    # Next.js Pages
│   ├── page.tsx           # Home (Feed)
│   ├── login/             # Login
│   ├── signup/            # Signup
│   └── upload/            # Upload
│
├── components/            # UI Components
│   ├── Feed/             # Video feed
│   ├── VideoPlayer/      # Video player
│   └── Upload/           # Upload form
│
├── hooks/                # Custom Hooks
│   ├── useVideos.ts      # Video data
│   ├── useVideoPlayer.ts # Player controls
│   ├── useSocket.ts      # WebSocket
│   └── ...
│
├── store/                # State (Zustand)
│   ├── auth.ts           # Authentication
│   ├── video.ts          # Player state
│   └── upload.ts         # Upload state
│
├── lib/                  # Core Utilities
│   ├── api/              # API Layer
│   ├── types.ts          # Types
│   ├── constants.ts      # Config
│   └── utils.ts          # Helpers
│
└── services/             # External Services
    └── socket.ts         # Socket.IO
```

---

## 🔑 Key Concepts

### 1. Data Fetching (SWR)
```typescript
// Automatic caching, revalidation, and more
const { videos, loadMore, hasMore } = useInfiniteVideos();
```

### 2. State Management (Zustand)
```typescript
// Simple, scalable global state
const { user, login, logout } = useAuthStore();
```

### 3. Real-time (Socket.IO)
```typescript
// Live updates for likes, comments
const { socket, isConnected } = useSocket();
```

### 4. Video Player
```typescript
// Full control over video playback
const { play, pause, seek, volume } = useVideoPlayer({ videoId });
```

---

## 💡 Common Tasks

### Add a New Component
```typescript
// src/components/MyComponent/MyComponent.tsx
'use client';

import { memo } from 'react';

interface MyComponentProps {
  title: string;
}

const MyComponent = memo<MyComponentProps>(({ title }) => {
  return <div>{title}</div>;
});

MyComponent.displayName = 'MyComponent';

export default MyComponent;
```

### Create a Custom Hook
```typescript
// src/hooks/useMyHook.ts
import { useState, useEffect } from 'react';

export function useMyHook() {
  const [data, setData] = useState(null);
  
  useEffect(() => {
    // Hook logic
  }, []);
  
  return { data };
}
```

### Add an API Endpoint
```typescript
// src/lib/api/myService.ts
import { apiClient } from './client';

export async function fetchMyData() {
  const response = await apiClient.get('/my-endpoint');
  return response.data;
}
```

---

## 🎨 Styling Guidelines

### Using Tailwind
```typescript
import { cn } from '@/lib/utils';

<div className={cn(
  'base-class',
  condition && 'conditional-class',
  props.className
)}>
  Content
</div>
```

### Common Patterns
```css
/* Full height container */
className="h-screen w-full"

/* Flex center */
className="flex items-center justify-center"

/* Responsive */
className="w-full md:w-1/2 lg:w-1/3"

/* Hover effects */
className="hover:bg-gray-100 transition-colors"
```

---

## 🔧 Development Tips

### 1. Auto-completion
VS Code will provide full TypeScript auto-completion thanks to proper typing.

### 2. Hot Reload
Changes are reflected instantly in development mode.

### 3. Error Handling
All API errors are caught and displayed appropriately.

### 4. Performance
Components are optimized with `memo` and `useCallback`.

---

## 📚 Learn More

- **Architecture**: See `ARCHITECTURE.md` for detailed documentation
- **Summary**: See `REFACTORING_SUMMARY.md` for what was built
- **Types**: Check `src/lib/types.ts` for all TypeScript definitions
- **API**: Look at `src/lib/api/` for all API methods

---

## 🐛 Troubleshooting

### Build Errors
```bash
# Clear cache and reinstall
rm -rf .next node_modules
npm install
npm run dev
```

### Type Errors
```bash
# Check TypeScript
npx tsc --noEmit
```

### Linting
```bash
# Fix auto-fixable issues
npm run lint -- --fix
```

---

## 🚀 Deployment

### Build for Production
```bash
npm run build
npm start
```

### Environment Variables
Set these in your hosting platform:
- `NEXT_PUBLIC_API_URL`
- `NEXT_PUBLIC_SOCKET_URL`

---

## ✅ Checklist

Before pushing to production:

- [ ] All environment variables set
- [ ] API endpoints tested
- [ ] Socket.IO connection verified
- [ ] Build completes without errors
- [ ] No TypeScript errors
- [ ] No linting errors
- [ ] Performance tested
- [ ] Mobile responsive
- [ ] SEO optimized
- [ ] Analytics added

---

## 🎉 You're Ready!

The codebase is production-ready with:
- ✅ Clean architecture
- ✅ Type safety
- ✅ Best practices
- ✅ Full documentation
- ✅ Scalable structure

Happy coding! 🚀
