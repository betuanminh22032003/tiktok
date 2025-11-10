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

(continued...)
