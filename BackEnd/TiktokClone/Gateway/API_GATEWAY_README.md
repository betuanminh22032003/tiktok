# TikTok Clone - API Gateway Documentation

## 🚀 Overview

API Gateway được xây dựng với **Ocelot** để route requests tới các microservices và cung cấp các tính năng:

- ✅ **Routing** - Forward requests tới đúng service
- ✅ **JWT Authentication** - Xác thực token trước khi forward
- ✅ **Rate Limiting** - Giới hạn số request per minute
- ✅ **CORS** - Cho phép frontend truy cập
- ✅ **Service Discovery** - Tự động tìm services (ready for Docker/Kubernetes)

---

## 📡 Service Ports

| Service | Port | Description |
|---------|------|-------------|
| **API Gateway** | `7000` | Entry point cho tất cả requests |
| Identity Service | `5001` | Authentication & User management |
| Video Service | `5002` | Video upload, feed, view counting |
| Interaction Service | `5003` | Like, Comment, Share |
| User Service | `5004` | User profiles, Follow/Unfollow |

---

## 🔗 API Routes

### Identity Service
```
Gateway: http://localhost:7000/identity/{endpoint}
Direct: http://localhost:5001/api/auth/{endpoint}

Endpoints:
- POST /identity/register    - Đăng ký user mới
- POST /identity/login       - Đăng nhập
- GET  /identity/user/{id}   - Lấy thông tin user
```

### Video Service
```
Gateway: http://localhost:7000/videos/{endpoint}
Direct: http://localhost:5002/api/videos/{endpoint}

Endpoints:
- POST /videos/upload             - Upload video (Auth required)
- GET  /videos/feed               - Lấy video feed
- GET  /videos/{id}               - Lấy video chi tiết
- POST /videos/{id}/increment-view - Tăng view count

Rate Limit: 100 requests/minute
```

### Interaction Service
```
Gateway: http://localhost:7000/interactions/{endpoint}
Direct: http://localhost:5003/api/interactions/{endpoint}

Endpoints:
- POST   /interactions/{videoId}/like     - Like video (Auth required)
- DELETE /interactions/{videoId}/unlike   - Unlike video (Auth required)
- POST   /interactions/{videoId}/comment  - Comment (Auth required)
- GET    /interactions/{videoId}/likes    - Lấy danh sách likes
- GET    /interactions/{videoId}/comments - Lấy danh sách comments

Rate Limit: 200 requests/minute
```

### User Service
```
Gateway: http://localhost:7000/users/{endpoint}
Direct: http://localhost:5004/api/users/{endpoint}

Endpoints:
- POST /users/profile                - Tạo profile (Auth required)
- GET  /users/profile/{userId}       - Lấy profile
- PUT  /users/profile                - Update profile (Auth required)
- POST /users/avatar                 - Upload avatar (Auth required)
- POST /users/follow/{userId}        - Follow user (Auth required)
- DELETE /users/follow/{userId}      - Unfollow user (Auth required)
- GET  /users/{userId}/followers     - Lấy danh sách followers
- GET  /users/{userId}/following     - Lấy danh sách following

Rate Limit: 100 requests/minute
```

---

## 🔐 Authentication

API Gateway sử dụng **JWT Bearer Token** authentication:

### 1. Lấy Token
```bash
POST http://localhost:7000/identity/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "yourPassword"
}

Response:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": "guid",
  "username": "username"
}
```

### 2. Sử dụng Token
```bash
GET http://localhost:7000/videos/upload
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## ⚡ Rate Limiting

Rate limits được apply cho từng service:

| Service | Limit | Period |
|---------|-------|--------|
| Video | 100 req/min | 1 minute |
| Interaction | 200 req/min | 1 minute |
| User | 100 req/min | 1 minute |

**Response khi vượt limit:**
```json
{
  "error": "Rate limit exceeded. Please try again later."
}
```
HTTP Status: `429 Too Many Requests`

---

## 🧪 Testing Examples

### 1. Register & Login
```bash
# Register
curl -X POST http://localhost:7000/identity/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "username": "testuser",
    "password": "Test123!@#"
  }'

# Login
curl -X POST http://localhost:7000/identity/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test123!@#"
  }'
```

### 2. Upload Video (Authenticated)
```bash
curl -X POST http://localhost:7000/videos/upload \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "My First Video",
    "description": "Test video",
    "videoUrl": "https://example.com/video.mp4",
    "duration": 30
  }'
```

### 3. Get Video Feed
```bash
curl http://localhost:7000/videos/feed?page=1&pageSize=10
```

### 4. Like Video (Authenticated)
```bash
curl -X POST http://localhost:7000/interactions/{videoId}/like \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 5. Follow User (Authenticated)
```bash
curl -X POST http://localhost:7000/users/follow/{userId} \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{"followingUsername": "targetUser"}'
```

---

## 🐳 Docker Setup

### Run with Docker Compose
```bash
cd BackEnd/TiktokClone
docker-compose up -d
```

Services sẽ start trên các ports:
- API Gateway: `7000`
- PostgreSQL DBs: `5432-5435`
- Redis: `6379`
- RabbitMQ: `5672`, `15672` (Management UI)

---

## 🛠️ Configuration

### appsettings.json
```json
{
  "JwtSettings": {
    "SecretKey": "your-secret-key-here",
    "Issuer": "TiktokClone.IdentityService",
    "Audience": "TiktokClone.Client",
    "ExpiryMinutes": 60
  },
  "ServiceUrls": {
    "Identity": "http://localhost:5001",
    "Video": "http://localhost:5002",
    "Interaction": "http://localhost:5003",
    "User": "http://localhost:5004"
  }
}
```

### ocelot.json
- Cấu hình routing cho tất cả services
- JWT authentication per route
- Rate limiting per service
- Global CORS policy

---

## 📊 Health Checks

Check service status:
```bash
# Identity Service
curl http://localhost:5001/health

# Video Service
curl http://localhost:5002/health

# Interaction Service
curl http://localhost:5003/health

# User Service
curl http://localhost:5004/health
```

---

## 🔧 Troubleshooting

### 1. Service không accessible
```bash
# Check service đang chạy
netstat -ano | findstr :5001
netstat -ano | findstr :5002
netstat -ano | findstr :5003
netstat -ano | findstr :5004
```

### 2. JWT Token invalid
- Kiểm tra `SecretKey` phải giống nhau ở tất cả services
- Kiểm tra `Issuer` và `Audience` match
- Token có thể đã hết hạn (60 minutes default)

### 3. Rate Limit issues
- Adjust limits trong `ocelot.json`
- Clear rate limit cache bằng cách restart gateway

### 4. CORS errors
- Kiểm tra frontend URL trong CORS policy
- Ensure `AllowCredentials()` enabled

---

## 📝 Development Tips

### Run tất cả services cùng lúc
```powershell
# PowerShell script
cd BackEnd/TiktokClone

# Terminal 1 - Identity
cd Services/Identity/src/Identity.Web
dotnet run

# Terminal 2 - Video
cd Services/Video/Video.Web
dotnet run

# Terminal 3 - Interaction
cd Services/Interaction/Interaction.Web
dotnet run

# Terminal 4 - User
cd Services/User/User.Web
dotnet run

# Terminal 5 - Gateway
cd Gateway/APIGateway.Web
dotnet run
```

### Hot Reload
Ocelot config (`ocelot.json`) sẽ tự động reload khi file thay đổi.

---

## 🚀 Production Checklist

- [ ] Change JWT `SecretKey` thành strong random key
- [ ] Enable HTTPS cho tất cả services
- [ ] Configure service discovery (Consul/Kubernetes)
- [ ] Add logging & monitoring (Serilog, ELK Stack)
- [ ] Implement circuit breaker (Polly)
- [ ] Add caching layer (Redis)
- [ ] Configure proper CORS origins
- [ ] Set up API versioning
- [ ] Add request/response logging
- [ ] Implement distributed tracing (OpenTelemetry)

---

## 📚 Additional Resources

- [Ocelot Documentation](https://ocelot.readthedocs.io/)
- [JWT Best Practices](https://tools.ietf.org/html/rfc8725)
- [Microservices Patterns](https://microservices.io/patterns/)
- [API Gateway Pattern](https://docs.microsoft.com/en-us/azure/architecture/microservices/design/gateway)
