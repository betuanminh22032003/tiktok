# 🚀 TikTok Clone - API Documentation

## 📡 Services Overview

| Service | Port | Base URL | Swagger |
|---------|------|----------|---------|
| **Identity** | 5001 | `https://localhost:5001` | `/swagger` |
| **Video** | 5002 | `https://localhost:5002` | `/swagger` |
| **Interaction** | 5003 | `https://localhost:5003` | `/swagger` |

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

---

## 📊 Rate Limits

Currently no rate limiting implemented. Consider adding:
- **Identity**: 10 requests/minute for registration
- **Video Upload**: 5 uploads/hour per user
- **Interactions**: 100 requests/minute per user

---

## 🔗 CORS Configuration

All services allow:
- **Origins**: `http://localhost:3000`, `https://localhost:3000`
- **Methods**: GET, POST, PUT, DELETE, OPTIONS
- **Headers**: Any
- **Credentials**: Allowed

---

*Last Updated: November 9, 2025*
*Version: 1.0.0*
