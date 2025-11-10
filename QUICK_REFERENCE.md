# 🚀 TikTok Clone - Quick Reference Card

## 📊 Project Status: ✅ COMPLETE

**19 Projects Built** | **4 Microservices** | **Production Ready**

---

## 🌐 Service URLs

| Service | URL | Swagger |
|---------|-----|---------|
| **Frontend** | http://localhost:3000 | - |
| **API Gateway** | http://localhost:7000 | - |
| **Identity** | http://localhost:5001 | /swagger |
| **Video** | http://localhost:5002 | /swagger |
| **Interaction** | http://localhost:5003 | /swagger |
| **User** | http://localhost:5004 | /swagger |
| **PostgreSQL** | localhost:5432-5435 | - |
| **Redis** | localhost:6379 | - |

---

## ⚡ Quick Start Commands

### Start Everything (Docker Compose)
```bash
cd BackEnd/TiktokClone
docker-compose up -d

cd ../../FrontEnd
npm install && npm run dev
```

### Start Backend Services
```bash
# Terminal 1 - Infrastructure
docker-compose up -d postgres-identity postgres-video postgres-interaction postgres-user redis

# Terminal 2 - Identity Service
cd Services/Identity/Src/Identity.Web && dotnet run

# Terminal 3 - Video Service  
cd Services/Video/Video.Web && dotnet run

# Terminal 4 - Interaction Service
cd Services/Interaction/Interaction.Web && dotnet run

# Terminal 5 - User Service
cd Services/User/User.Web && dotnet run

# Terminal 6 - API Gateway
cd Gateway/APIGateway.Web && dotnet run
```

---

## 📡 Essential API Endpoints

### Authentication (Identity Service)
```bash
# Register
POST /identity/register
Body: {"email":"user@example.com","username":"user","password":"Pass123!"}

# Login (get JWT token)
POST /identity/login
Body: {"emailOrUsername":"user","password":"Pass123!"}
```

### Video Operations
```bash
# Get Feed
GET /videos/feed?page=1&pageSize=10

# Upload Video (Auth required)
POST /videos
Headers: Authorization: Bearer {token}
Body: {"title":"Video","videoUrl":"https://...","duration":30,...}
```

### Interactions
```bash
# Like Video (Auth required)
POST /interactions/{videoId}/like
Headers: Authorization: Bearer {token}

# Add Comment (Auth required)
POST /interactions/{videoId}/comment
Headers: Authorization: Bearer {token}
Body: {"content":"Great video!"}
```

### User Profile
```bash
# Create Profile (Auth required)
POST /users/profile
Headers: Authorization: Bearer {token}
Body: {"userId":"...","username":"...","displayName":"..."}

# Follow User (Auth required)
POST /users/follow/{userId}
Headers: Authorization: Bearer {token}
```

---

## 🏗️ Architecture Stack

**Backend:** .NET 8 + ASP.NET Core  
**Database:** PostgreSQL 15 (x4 instances)  
**Cache:** Redis 7  
**API Gateway:** Ocelot 24.0  
**CQRS:** MediatR 12.2  
**ORM:** Entity Framework Core 8  
**Validation:** FluentValidation 11.9  
**Security:** BCrypt + JWT (HS256)  

**Frontend:** Next.js 14 + TypeScript + TailwindCSS  
**State:** Zustand  

**Patterns:** Clean Architecture + DDD + CQRS

---

## 🗄️ Database Structure

| Database | Port | Tables |
|----------|------|--------|
| tiktok_identity | 5432 | Users |
| tiktok_video | 5433 | Videos |
| tiktok_interaction | 5434 | Likes, Comments |
| tiktok_user | 5435 | UserProfiles, Follows |

---

## 🔒 Security Features

- ✅ BCrypt password hashing (work factor 12)
- ✅ JWT tokens (60 min expiry)
- ✅ Bearer token authentication
- ✅ Rate limiting (100-200 req/min)
- ✅ CORS configuration
- ✅ Input validation (FluentValidation)
- ✅ SQL injection protection (EF Core)

---

## 🧪 Test Flow

```bash
# 1. Register & Login
curl -X POST http://localhost:7000/identity/register -H "Content-Type: application/json" -d '{"email":"test@test.com","username":"test","password":"Test123!"}'
curl -X POST http://localhost:7000/identity/login -H "Content-Type: application/json" -d '{"emailOrUsername":"test","password":"Test123!"}'

# 2. Save JWT token from response

# 3. Create profile
curl -X POST http://localhost:7000/users/profile -H "Authorization: Bearer {TOKEN}" -H "Content-Type: application/json" -d '{"userId":"{ID}","username":"test"}'

# 4. Upload video
curl -X POST http://localhost:7000/videos -H "Authorization: Bearer {TOKEN}" -H "Content-Type: application/json" -d '{"title":"Test","videoUrl":"https://...","duration":30}'

# 5. Like video
curl -X POST http://localhost:7000/interactions/{VIDEO_ID}/like -H "Authorization: Bearer {TOKEN}"
```

---

## 🐛 Common Issues

### Port in Use
```bash
netstat -ano | findstr :5001
taskkill /PID {PID} /F
```

### Database Connection
```bash
docker ps | findstr postgres
docker-compose restart postgres-identity
```

### Redis Connection
```bash
docker exec -it redis redis-cli ping
# Should return: PONG
```

### EF Migrations
```bash
dotnet ef migrations add InitialCreate --project ../Service.Infrastructure
dotnet ef database update
```

---

## 📚 Documentation Index

| Document | Purpose |
|----------|---------|
| **README.md** | Project overview & features |
| **DEPLOYMENT_GUIDE.md** | Complete setup instructions |
| **BackEnd/QUICK_START.md** | Backend quick start |
| **BackEnd/API_DOCUMENTATION.md** | All API endpoints |
| **BackEnd/FINAL_BUILD_SUMMARY.md** | Architecture details |
| **FrontEnd/README.md** | Frontend guide |
| **FrontEnd/QUICK_START.md** | Frontend quick start |

---

## 📞 Need Help?

1. Check **DEPLOYMENT_GUIDE.md** for troubleshooting
2. Review **API_DOCUMENTATION.md** for endpoint details
3. See **Swagger UIs** at http://localhost:5001-5004/swagger
4. Check Docker logs: `docker-compose logs -f`

---

## ✨ Project Highlights

- ✅ 19 projects, 0 build errors
- ✅ 10,000+ lines of code
- ✅ 25+ REST API endpoints
- ✅ Clean Architecture + DDD + CQRS
- ✅ Microservices architecture
- ✅ Production-ready code
- ✅ Complete documentation
- ✅ Docker containerization

---

**Version:** 2.0.0  
**Status:** Production Ready ✅  
**Last Updated:** November 10, 2025

---

*Print this card for quick reference during development!* 🎯
