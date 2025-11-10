# 🚀 TikTok Clone - API Documentation

## 📡 Services Overview

| Service | Port | Base URL | Swagger |
|---------|------|----------|---------|
| **API Gateway** | 7000 | `http://localhost:7000` | N/A |
| **Identity** | 5001 | `https://localhost:5001` | `/swagger` |
| **Video** | 5002 | `https://localhost:5002` | `/swagger` |
| **Interaction** | 5003 | `https://localhost:5003` | `/swagge---

## 👤 User Service API

### **POST /api/users/profile**
Create user profile (requires authentication)

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "username": "johndoe",
  "displayName": "John Doe",
  "bio": "Content creator | Tech enthusiast 🚀",
  "avatarUrl": "https://cdn.example.com/avatars/johndoe.jpg"
}
```

**Response:** `200 OK`
```json
{
  "message": "Profile created successfully",
  "profileId": "9f8e7d6c-5b4a-3c2d-1e0f-123456789abc"
}
```

---

### **GET /api/users/profile/{userId}**
Get user profile by ID

**Request:**
```
GET /api/users/profile/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

**Response:** `200 OK`
```json
{
  "id": "9f8e7d6c-5b4a-3c2d-1e0f-123456789abc",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "username": "johndoe",
  "displayName": "John Doe",
  "bio": "Content creator | Tech enthusiast 🚀",
  "avatarUrl": "https://cdn.example.com/avatars/johndoe.jpg",
  "followersCount": 1250,
  "followingCount": 350,
  "videosCount": 45,
  "totalLikes": 125000,
  "createdAt": "2025-11-01T10:00:00Z",
  "updatedAt": "2025-11-09T15:30:00Z"
}
```

---

### **PUT /api/users/profile**
Update user profile (requires authentication)

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```json
{
  "displayName": "John Doe - Updated",
  "bio": "Updated bio with new information",
  "avatarUrl": "https://cdn.example.com/avatars/johndoe-new.jpg"
}
```

**Response:** `200 OK`
```json
{
  "message": "Profile updated successfully"
}
```

---

### **POST /api/users/avatar**
Upload/Update avatar (requires authentication)

**Headers:**
```
Authorization: Bearer {token}
Content-Type: multipart/form-data
```

**Request:**
```
Form Data:
- file: [binary image file]
```

**Response:** `200 OK`
```json
{
  "message": "Avatar updated successfully",
  "avatarUrl": "https://cdn.example.com/avatars/johndoe-12345.jpg"
}
```

---

### **POST /api/users/follow/{userId}**
Follow a user (requires authentication)

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```
POST /api/users/follow/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

**Response:** `200 OK`
```json
{
  "message": "User followed successfully",
  "followId": "f1e2d3c4-b5a6-7890-1234-567890abcdef"
}
```

---

### **DELETE /api/users/follow/{userId}**
Unfollow a user (requires authentication)

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```
DELETE /api/users/follow/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

**Response:** `200 OK`
```json
{
  "message": "User unfollowed successfully"
}
```

---

### **GET /api/users/{userId}/followers**
Get user's followers with pagination

**Query Parameters:**
- `page` (default: 1)
- `pageSize` (default: 20, max: 100)

**Request:**
```
GET /api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6/followers?page=1&pageSize=20
```

**Response:** `200 OK`
```json
{
  "followers": [
    {
      "id": "f1e2d3c4-b5a6-7890-1234-567890abcdef",
      "followerUserId": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
      "followerUsername": "janedoe",
      "followerDisplayName": "Jane Doe",
      "followerAvatarUrl": "https://cdn.example.com/avatars/janedoe.jpg",
      "followedAt": "2025-11-08T10:30:00Z"
    }
  ],
  "page": 1,
  "pageSize": 20,
  "totalCount": 1250,
  "totalPages": 63
}
```

---

### **GET /api/users/{userId}/following**
Get users that this user is following with pagination

**Query Parameters:**
- `page` (default: 1)
- `pageSize` (default: 20, max: 100)

**Request:**
```
GET /api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6/following?page=1&pageSize=20
```

**Response:** `200 OK`
```json
{
  "following": [
    {
      "id": "f1e2d3c4-b5a6-7890-1234-567890abcdef",
      "followingUserId": "b2c3d4e5-f6a7-8901-2345-678901bcdef0",
      "followingUsername": "techcreator",
      "followingDisplayName": "Tech Creator",
      "followingAvatarUrl": "https://cdn.example.com/avatars/techcreator.jpg",
      "followedAt": "2025-11-05T14:20:00Z"
    }
  ],
  "page": 1,
  "pageSize": 20,
  "totalCount": 350,
  "totalPages": 18
}
```

---

## 🔐 Authentication|
| **User** | 5004 | `https://localhost:5004` | `/swagger` |

---

## 🔐 Identity Service API

### **POST /api/auth/register**
Register a new user

**Request:**
```json
{
  "email": "user@example.com",
  "username": "johndoe",
  "password": "SecurePassword123!"
}
```

**Response:** `200 OK`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "user@example.com",
  "username": "johndoe"
}
```

---

### **POST /api/auth/login**
Login and get JWT token

**Request:**
```json
{
  "emailOrUsername": "johndoe",
  "password": "SecurePassword123!"
}
```

**Response:** `200 OK`
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "dGVzdC1yZWZyZXNoLXRva2Vu",
  "expiresIn": 3600,
  "user": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "username": "johndoe",
    "email": "user@example.com",
    "role": "User"
  }
}
```

---

### **GET /api/auth/me**
Get current user info (requires authentication)

**Headers:**
```
Authorization: Bearer {token}
```

**Response:** `200 OK`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "username": "johndoe",
  "email": "user@example.com",
  "role": "User",
  "isEmailVerified": false,
  "createdAt": "2025-11-09T10:00:00Z"
}
```

---

## 🎥 Video Service API

### **POST /api/videos**
Upload a new video (requires authentication)

**Headers:**
```
Authorization: Bearer {token}
Content-Type: application/json
```

**Request:**
```json
{
  "title": "My Awesome Video",
  "description": "Check out this cool video!",
  "videoUrl": "https://cdn.example.com/videos/abc123.mp4",
  "thumbnailUrl": "https://cdn.example.com/thumbnails/abc123.jpg",
  "durationInSeconds": 30,
  "fileSize": 5242880,
  "format": "mp4",
  "width": 1080,
  "height": 1920
}
```

**Response:** `200 OK`
```json
{
  "id": "7b3e4f5a-1234-5678-90ab-cdef12345678",
  "title": "My Awesome Video",
  "videoUrl": "https://cdn.example.com/videos/abc123.mp4",
  "status": "Processing",
  "createdAt": "2025-11-09T10:00:00Z"
}
```

---

### **GET /api/videos/feed**
Get paginated video feed

**Query Parameters:**
- `page` (default: 1)
- `pageSize` (default: 10, max: 50)

**Request:**
```
GET /api/videos/feed?page=1&pageSize=20
```

**Response:** `200 OK`
```json
{
  "videos": [
    {
      "id": "7b3e4f5a-1234-5678-90ab-cdef12345678",
      "title": "My Awesome Video",
      "description": "Check out this cool video!",
      "videoUrl": "https://cdn.example.com/videos/abc123.mp4",
      "thumbnailUrl": "https://cdn.example.com/thumbnails/abc123.jpg",
      "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "username": "johndoe",
      "duration": 30,
      "viewCount": 1250,
      "likeCount": 89,
      "commentCount": 12,
      "shareCount": 5,
      "status": "Ready",
      "createdAt": "2025-11-09T10:00:00Z"
    }
  ],
  "page": 1,
  "pageSize": 20,
  "totalCount": 150,
  "totalPages": 8
}
```

---

### **GET /api/videos/{id}**
Get video by ID

**Request:**
```
GET /api/videos/7b3e4f5a-1234-5678-90ab-cdef12345678
```

**Response:** `200 OK`
```json
{
  "id": "7b3e4f5a-1234-5678-90ab-cdef12345678",
  "title": "My Awesome Video",
  "description": "Check out this cool video!",
  "videoUrl": "https://cdn.example.com/videos/abc123.mp4",
  "thumbnailUrl": "https://cdn.example.com/thumbnails/abc123.jpg",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "username": "johndoe",
  "duration": 30,
  "viewCount": 1250,
  "likeCount": 89,
  "commentCount": 12,
  "shareCount": 5,
  "metadata": {
    "width": 1080,
    "height": 1920,
    "format": "mp4",
    "fileSize": 5242880
  },
  "status": "Ready",
  "createdAt": "2025-11-09T10:00:00Z"
}
```

---

### **POST /api/videos/{id}/view**
Increment view count

**Request:**
```
POST /api/videos/7b3e4f5a-1234-5678-90ab-cdef12345678/view
```

**Response:** `200 OK`
```json
{
  "message": "View counted",
  "viewCount": 1251
}
```

---

## ❤️ Interaction Service API

### **POST /api/interactions/likes**
Like a video (requires authentication)

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```json
{
  "videoId": "7b3e4f5a-1234-5678-90ab-cdef12345678"
}
```

**Response:** `200 OK`
```json
{
  "message": "Video liked successfully",
  "likeId": "9c8d7e6f-5a4b-3c2d-1e0f-123456789abc"
}
```

---

### **DELETE /api/interactions/likes/{videoId}**
Unlike a video (requires authentication)

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```
DELETE /api/interactions/likes/7b3e4f5a-1234-5678-90ab-cdef12345678
```

**Response:** `200 OK`
```json
{
  "message": "Video unliked successfully"
}
```

---

### **GET /api/interactions/videos/{videoId}/likes**
Get all likes for a video

**Request:**
```
GET /api/interactions/videos/7b3e4f5a-1234-5678-90ab-cdef12345678/likes
```

**Response:** `200 OK`
```json
{
  "likes": [
    {
      "id": "9c8d7e6f-5a4b-3c2d-1e0f-123456789abc",
      "videoId": "7b3e4f5a-1234-5678-90ab-cdef12345678",
      "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "username": "johndoe",
      "createdAt": "2025-11-09T10:30:00Z"
    }
  ],
  "totalCount": 89
}
```

---

### **POST /api/interactions/comments**
Add a comment (requires authentication)

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```json
{
  "videoId": "7b3e4f5a-1234-5678-90ab-cdef12345678",
  "content": "Great video! Keep it up!",
  "parentCommentId": null
}
```

**Response:** `200 OK`
```json
{
  "message": "Comment added successfully",
  "commentId": "a1b2c3d4-e5f6-7890-abcd-ef1234567890"
}
```

---

### **GET /api/interactions/videos/{videoId}/comments**
Get all comments for a video

**Request:**
```
GET /api/interactions/videos/7b3e4f5a-1234-5678-90ab-cdef12345678/comments
```

**Response:** `200 OK`
```json
{
  "comments": [
    {
      "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
      "videoId": "7b3e4f5a-1234-5678-90ab-cdef12345678",
      "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "username": "johndoe",
      "content": "Great video! Keep it up!",
      "parentCommentId": null,
      "createdAt": "2025-11-09T10:35:00Z",
      "updatedAt": null
    }
  ],
  "totalCount": 12
}
```

---

### **PUT /api/interactions/comments/{commentId}**
Update a comment (requires authentication, owner only)

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```json
{
  "content": "Updated: Great video! Keep it up! 🔥"
}
```

**Response:** `200 OK`
```json
{
  "message": "Comment updated successfully"
}
```

---

### **DELETE /api/interactions/comments/{commentId}**
Delete a comment - soft delete (requires authentication, owner only)

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```
DELETE /api/interactions/comments/a1b2c3d4-e5f6-7890-abcd-ef1234567890
```

**Response:** `200 OK`
```json
{
  "message": "Comment deleted successfully"
}
```

---

## 🔒 Authentication

All protected endpoints require JWT Bearer token:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Token Claims:
- `sub`: User ID (Guid)
- `unique_name`: Username
- `email`: User email
- `role`: User role (User, Admin, etc.)
- `exp`: Expiration timestamp

---

## ❌ Error Responses

### **400 Bad Request**
```json
{
  "error": "Validation failed: Title is required"
}
```

### **401 Unauthorized**
```json
{
  "error": "Unauthorized"
}
```

### **404 Not Found**
```json
{
  "error": "Video not found"
}
```

### **500 Internal Server Error**
```json
{
  "error": "An unexpected error occurred"
}
```

---

## 🧪 Testing with cURL

### Register & Login Flow
```bash
# 1. Register
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","username":"testuser","password":"Test123456!"}'

# 2. Login
TOKEN=$(curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"emailOrUsername":"testuser","password":"Test123456!"}' \
  | jq -r '.accessToken')

# 3. Get current user
curl https://localhost:5001/api/auth/me \
  -H "Authorization: Bearer $TOKEN"
```

### Video Operations
```bash
# Upload video
curl -X POST https://localhost:5002/api/videos \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "title":"Test Video",
    "description":"Test",
    "videoUrl":"https://example.com/video.mp4",
    "thumbnailUrl":"https://example.com/thumb.jpg",
    "durationInSeconds":30,
    "fileSize":1048576,
    "format":"mp4",
    "width":1080,
    "height":1920
  }'

# Get feed
curl https://localhost:5002/api/videos/feed?page=1&pageSize=10
```

### Interaction Operations
```bash
# Like video
curl -X POST https://localhost:5003/api/interactions/likes \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"videoId":"VIDEO_ID_HERE"}'

# Add comment
curl -X POST https://localhost:5003/api/interactions/comments \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"videoId":"VIDEO_ID_HERE","content":"Great video!"}'
```

### User Profile Operations
```bash
# Create profile
curl -X POST https://localhost:5004/api/users/profile \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "userId":"USER_ID_HERE",
    "username":"johndoe",
    "displayName":"John Doe",
    "bio":"Content creator"
  }'

# Follow user
curl -X POST https://localhost:5004/api/users/follow/TARGET_USER_ID \
  -H "Authorization: Bearer $TOKEN"

# Get user profile
curl https://localhost:5004/api/users/profile/USER_ID_HERE

# Get followers
curl https://localhost:5004/api/users/USER_ID_HERE/followers?page=1&pageSize=20
```

---

## 📊 Rate Limits - ĐÃ TRIỂN KHAI

Rate limiting được cấu hình tại API Gateway:
- **Identity Service**: 100 requests/minute
- **Video Service**: 100 requests/minute (per user)
- **Interaction Service**: 200 requests/minute (per user)
- **User Service**: 100 requests/minute (per user)

HTTP Response khi vượt rate limit:
```json
{
  "error": "Rate limit exceeded. Please try again later.",
  "retryAfter": 60
}
```

---

## 🔗 CORS Configuration

All services allow:
- **Origins**: `http://localhost:3000`, `https://localhost:3000`
- **Methods**: GET, POST, PUT, DELETE, OPTIONS
- **Headers**: Any
- **Credentials**: Allowed

---

## 📦 Complete API Summary

### Total Endpoints: 25+

| Service | Endpoints | Authentication Required |
|---------|-----------|------------------------|
| **Identity** | 3 | Register, Login (No), Get User (Yes) |
| **Video** | 4 | Upload (Yes), Feed/Get (No), View (No) |
| **Interaction** | 7 | Like/Comment (Yes), Get Likes/Comments (No) |
| **User** | 8 | Create/Update/Follow (Yes), Get Profile/Followers (No) |
| **API Gateway** | All above | Routes to services |

### Authentication Flow
```
1. Register: POST /identity/register
2. Login: POST /identity/login → Get JWT token
3. Use token: Authorization: Bearer {token}
4. Token expires in 60 minutes
5. Refresh: Re-login or implement refresh token
```

### Typical User Journey
```
1. Register & Login → Get JWT token
2. Create Profile → POST /users/profile
3. Upload Video → POST /videos
4. Browse Feed → GET /videos/feed
5. Like Video → POST /interactions/{videoId}/like
6. Comment → POST /interactions/{videoId}/comment
7. Follow User → POST /users/follow/{userId}
8. View Profile → GET /users/profile/{userId}
```

---

*Last Updated: November 10, 2025*
*Version: 2.0.0 - All Services Complete*
*Status: Production Ready ✅*
