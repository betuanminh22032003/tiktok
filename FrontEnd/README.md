# TikTok Clone - Frontend

A professional, production-ready TikTok clone built with modern web technologies.

## 🚀 Features - Ready to Integrate

### ✅ Backend Integration Ready
- **Authentication**: JWT tokens from Identity Service (Port 5001)
- **User Profiles**: Profile CRUD, Follow/Unfollow from User Service (Port 5004)
- **Video Feed**: Infinite scroll from Video Service (Port 5002)
- **Interactions**: Like/Unlike, Comments from Interaction Service (Port 5003)
- **API Gateway**: Single entry point at Port 7000 with rate limiting

### 🎨 Frontend Features
- **Infinite Video Feed**: Smooth scrolling with auto-play
- **Real-time Interactions**: Live likes, comments, and views via Socket.IO
- **Video Upload**: Drag-and-drop with progress tracking
- **Responsive Design**: Mobile-first approach
- **Performance Optimized**: Code splitting, memoization, lazy loading
- **Type-Safe**: Full TypeScript coverage
- **Clean Architecture**: Modular, maintainable, scalable

## 🛠️ Tech Stack

| Category | Technology |
|----------|-----------|
| Framework | Next.js 16 |
| Language | TypeScript |
| Styling | TailwindCSS 4 |
| State Management | Zustand |
| Data Fetching | SWR |
| HTTP Client | Axios |
| Real-time | Socket.IO Client |
| Video Player | HTML5 Video |

## 📦 Installation

```bash
# Clone the repository
git clone <repository-url>

# Navigate to frontend directory
cd FrontEnd

# Install dependencies
npm install

# Set up environment variables
cp .env.example .env.local
# Edit .env.local with your configuration

# Run development server
npm run dev
```

## 🔧 Environment Variables

Create a `.env.local` file:

```env
# Option 1: Use API Gateway (Recommended)
NEXT_PUBLIC_API_URL=http://localhost:7000
NEXT_PUBLIC_SOCKET_URL=http://localhost:7000

# Option 2: Direct to services (Development only)
NEXT_PUBLIC_IDENTITY_URL=http://localhost:5001
NEXT_PUBLIC_VIDEO_URL=http://localhost:5002
NEXT_PUBLIC_INTERACTION_URL=http://localhost:5003
NEXT_PUBLIC_USER_URL=http://localhost:5004
```

### Backend Services Must Be Running
Ensure these are started before running frontend:
- API Gateway: `http://localhost:7000` (or individual services on ports 5001-5004)
- PostgreSQL databases (4 instances)
- Redis cache

## 📁 Project Structure

```
src/
├── app/               # Next.js pages
├── components/        # React components
├── hooks/            # Custom hooks
├── store/            # Zustand stores
├── lib/              # Core utilities
│   ├── api/          # API layer
│   ├── types.ts      # TypeScript definitions
│   ├── constants.ts  # App constants
│   └── utils.ts      # Helper functions
└── services/         # External services
```

See [ARCHITECTURE.md](./ARCHITECTURE.md) for detailed documentation.

## 🎯 Key Components

### VideoPlayer
Auto-playing video with custom controls, optimized for mobile scrolling.

```typescript
import VideoPlayer from '@/components/VideoPlayer';

<VideoPlayer 
  video={videoData} 
  onVideoInView={(id) => console.log('Viewing:', id)} 
/>
```

### Feed
Infinite scrolling video feed with SWR data fetching.

```typescript
import Feed from '@/components/Feed';

<Feed />
```

### Upload
Drag-and-drop video upload with validation and progress tracking.

```typescript
import VideoUpload from '@/components/Upload';

<VideoUpload />
```

## 🪝 Custom Hooks

### useVideoPlayer
Complete video player control logic.

```typescript
const {
  videoRef,
  isPlaying,
  currentTime,
  duration,
  play,
  pause,
  togglePlay,
  seek,
} = useVideoPlayer({ videoId: 'video-1' });
```

### useInfiniteVideos
SWR-based infinite scroll implementation.

```typescript
const {
  videos,
  isLoading,
  hasMore,
  loadMore,
} = useInfiniteVideos();
```

### useSocket
Socket.IO connection management.

```typescript
const { socket, isConnected } = useSocket();
```

## 🎨 Styling Guidelines

Using TailwindCSS with utility-first approach:

```typescript
import { cn } from '@/lib/utils';

<div className={cn(
  'base-classes',
  condition && 'conditional-classes',
  className
)}>
  Content
</div>
```

## 📡 API Integration - Connected to .NET Backend

### Available Backend Endpoints:

**Identity Service (Authentication)**
```typescript
POST /identity/register - Register new user
POST /identity/login - Login and get JWT token
GET /identity/user/{id} - Get user info
```

**Video Service**
```typescript
GET /videos/feed?page=1&pageSize=10 - Get video feed
GET /videos/{id} - Get video details
POST /videos - Upload video (requires auth)
POST /videos/{id}/increment-view - Track view
```

**Interaction Service**
```typescript
POST /interactions/{videoId}/like - Like video (requires auth)
DELETE /interactions/{videoId}/unlike - Unlike video (requires auth)
POST /interactions/{videoId}/comment - Add comment (requires auth)
GET /interactions/{videoId}/likes - Get likes
GET /interactions/{videoId}/comments - Get comments
```

**User Service**
```typescript
POST /users/profile - Create profile (requires auth)
GET /users/profile/{userId} - Get profile
POST /users/follow/{userId} - Follow user (requires auth)
GET /users/{userId}/followers - Get followers
GET /users/{userId}/following - Get following
```

### API Client Usage:

```typescript
import { fetchVideos, toggleLike } from '@/lib/api';

// Fetch videos from backend
const videos = await fetchVideos(page, pageSize);

// Toggle like (sends to backend)
const result = await toggleLike(videoId);

// All requests include JWT token automatically
```

## 🔌 Real-time Features

Socket.IO integration for live updates:

```typescript
import { emitLike, onLikeUpdate } from '@/services/socket';

// Emit event
emitLike(videoId);

// Listen for updates
const unsubscribe = onLikeUpdate((data) => {
  console.log('Like update:', data);
});
```

## 🧪 Development

```bash
# Development server
npm run dev

# Type checking
npm run type-check

# Linting
npm run lint

# Build for production
npm run build

# Start production server
npm start
```

## 📱 Mobile Optimization

- Touch-friendly controls
- Optimized video loading
- Responsive layouts
- Smooth animations
- Battery-efficient playback

## 🎭 Performance Tips

1. **Video Optimization**
   - Use appropriate video codecs (H.264/VP9)
   - Compress videos before upload
   - Generate thumbnails for posters

2. **Code Splitting**
   - Dynamic imports for heavy components
   - Route-based code splitting with Next.js

3. **Caching Strategy**
   - SWR for smart data caching
   - Service worker for offline support

## 🔒 Security

- XSS protection
- CSRF tokens
- Secure headers
- Content validation
- Rate limiting

## 🤝 Contributing

1. Follow the component guidelines in ARCHITECTURE.md
2. Write TypeScript with proper types
3. Add JSDoc comments
4. Keep components small and focused
5. Write tests for new features

## 📝 Code Style

- Use functional components with hooks
- Prefer `const` over `let`
- Use arrow functions
- Destructure props
- Use TypeScript strict mode

## 🐛 Troubleshooting

### Videos not loading
- Check API_URL environment variable
- Verify backend is running
- Check network tab for errors

### Socket connection fails
- Verify SOCKET_URL is correct
- Check CORS settings on backend
- Ensure Socket.IO versions match

### Build errors
- Clear `.next` directory
- Delete `node_modules` and reinstall
- Update to latest dependencies

## 📄 License

MIT

## 👥 Authors

Your Name

## 🙏 Acknowledgments

- Next.js team for the amazing framework
- Vercel for hosting solutions
- Open source community
