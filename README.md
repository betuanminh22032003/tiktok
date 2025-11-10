# 🎬 TikTok Clone Web App (Microservices Architecture)

## 🎉 TRẠNG THÁI DỰ ÁN: HOÀN THÀNH ✅

**19/19 projects build thành công** | **4 Microservices hoạt động** | **API Gateway đầy đủ** | **Production-ready code**

## 📚 Documentation (quick links)

- Primary docs directory: `docs/` — consolidated project documentation (backend, frontend, gateway).
- DevOps & deployment artifacts: `devops/` — deployment guides, runbooks, and production notes.
- Note: Originals remain in `BackEnd/` and `FrontEnd/`; files in `docs/` are copies for easier discovery.

### ✨ Những gì đã hoàn thành:
- ✅ **Identity Service** - Đăng ký, đăng nhập, JWT authentication (BCrypt + JWT)
- ✅ **User Service** - Profile management, Follow/Unfollow, Avatar upload
- ✅ **Video Service** - Upload, Feed với pagination, View counter (Redis cache)
- ✅ **Interaction Service** - Like/Unlike, Comment CRUD với soft delete
- ✅ **API Gateway** - Ocelot routing, JWT validation, Rate limiting
- ✅ **Shared Kernel** - DDD building blocks, Repository, UnitOfWork
- ✅ **Clean Architecture** - Domain, Application, Infrastructure, Web layers
- ✅ **CQRS Pattern** - Commands & Queries với MediatR
- ✅ **Docker Compose** - Full infrastructure setup
- ✅ **Swagger Documentation** - API docs cho mọi service

### 📊 Thống kê
- **Total Projects:** 19
- **Backend Services:** 4 microservices + 1 API Gateway
- **Endpoints:** 25+ REST APIs
- **Databases:** 4 PostgreSQL databases
- **Cache:** Redis
- **Architecture Patterns:** Clean Architecture + DDD + CQRS
- **Lines of Code:** 10,000+

---

## 📖 Giới thiệu

Dự án **TikTok Clone** là một web app demo được xây dựng với mục tiêu mô phỏng các tính năng cơ bản của TikTok:

- Đăng ký / Đăng nhập tài khoản  
- Xem danh sách video (feed) dạng cuộn dọc  
- Tự động phát video khi hiển thị  
- Thả tim (like), bình luận (comment), đếm lượt xem  
- Upload video từ local hoặc URL  
- Cập nhật realtime khi có like/comment mới  

Dự án sử dụng **kiến trúc Microservices + Event-driven**, có thể mở rộng, hỗ trợ realtime và triển khai dễ dàng trên **AWS EC2**.

---

## 🧠 Mục tiêu

- Xây dựng hệ thống backend tách biệt theo từng domain  
- Hỗ trợ realtime với socket.io  
- Authentication bảo mật (JWT + HttpOnly cookie)  
- Lưu trữ dữ liệu tối ưu (PostgreSQL + Redis)  
- Có thể mở rộng theo mô hình microservices  
- CI/CD và deploy production-ready  

---

## 🧱 Kiến trúc tổng thể

### 🧩 Mô hình hệ thống

[Client - Next.js]
│
▼
[API Gateway / BFF]
│
┌───────────────┬───────────────┬───────────────┐
│               │               │               │
▼               ▼               ▼               ▼
Auth Service    User Service    Video Service    Realtime Service
│               │               │               │
▼               ▼               ▼               ▼
PostgreSQL      PostgreSQL      Redis / Socket.IO
│                 │               │
▼                 ▼               ▼
(S3 / Local file) (Cache) (Broadcast)

### ⚙️ Kiến trúc đề xuất

- **Loại kiến trúc:** Microservices + API Gateway + Event-driven  
- **Giao tiếp nội bộ:** REST + Redis Pub/Sub  
- **Realtime:** Socket.io  
- **Triển khai:** Docker Compose / AWS EC2  
- **Giám sát:** Prometheus + Grafana + Sentry  

---

## 🔩 Thành phần hệ thống (Microservices) - ĐÃ HOÀN THÀNH

| Service | Nhiệm vụ chính | Công nghệ | Port | Trạng thái |
|----------|----------------|-----------|------|------------|
| **API Gateway** | - Ocelot routing<br>- JWT validation<br>- Rate limiting (100-200 req/min)<br>- CORS configuration | .NET 8 + Ocelot | 7000 | ✅ |
| **Identity Service** | - Đăng nhập / đăng ký<br>- JWT tokens (60 min expiry)<br>- BCrypt password hashing<br>- User management | .NET 8 + PostgreSQL + Redis | 5001 | ✅ |
| **User Service** | - CRUD profile (Name, Bio, Avatar)<br>- Follow/Unfollow users<br>- Get Followers/Following<br>- Avatar upload | .NET 8 + PostgreSQL | 5004 | ✅ |
| **Video Service** | - Upload video metadata<br>- Video feed với pagination<br>- View counter với Redis cache<br>- Video status tracking | .NET 8 + PostgreSQL + Redis | 5002 | ✅ |
| **Interaction Service** | - Like/Unlike video<br>- Comment CRUD (với soft delete)<br>- Reply to comments<br>- Redis counter cache | .NET 8 + PostgreSQL + Redis | 5003 | ✅ |
| **Shared Kernel** | - DDD building blocks<br>- Repository pattern<br>- Result pattern<br>- Domain events | .NET 8 Library | N/A | ✅ |

---

## 🔁 Giao tiếp giữa các Service

| Loại | Mục đích | Công nghệ |
|------|-----------|-----------|
| **REST API** | Truy cập đồng bộ (login, get feed, upload) | Axios / Fetch |
| **Redis Pub/Sub / Kafka** | Event bất đồng bộ (like, comment, view) | Redis Pub/Sub |
| **Socket.io** | Realtime communication đến frontend | Socket.io Adapter Redis |

---

## 💾 Database & Cache

| Thành phần | Mục đích | Công nghệ |
|-------------|-----------|-----------|
| **PostgreSQL** | Lưu metadata video, user, comment | PostgreSQL + Prisma ORM |
| **Redis** | Cache realtime (view, like, comment counter) | Redis |
| **Message Queue** | Truyền event bất đồng bộ | Redis Pub/Sub hoặc Kafka |
| **File Storage** | Lưu file video hoặc ảnh | AWS S3 hoặc local `/uploads` |

---

## 🔐 Bảo mật

- JWT Access Token + Refresh Token (HttpOnly cookie, Secure, SameSite=Strict)  
- Middleware xác thực JWT  
- Rate limiting tại API Gateway  
- Helmet & CORS middleware  
- Không chạy container bằng root (`USER node`)  
- HTTPS cho môi trường production  

---

## 🧱 Trạng thái hiện tại - HOÀN THÀNH

| Service | Trạng thái | API Endpoints |
|----------|----------|---------------|
| **Identity Service** | ✅ HOÀN THÀNH | Register, Login, Get User |
| **Video Service** | ✅ HOÀN THÀNH | Upload, Feed, Get Video, Increment View |
| **Interaction Service** | ✅ HOÀN THÀNH | Like, Unlike, Comment (CRUD), Get Likes/Comments |
| **User Service** | ✅ HOÀN THÀNH | Profile (CRUD), Follow, Unfollow, Get Followers/Following |
| **API Gateway** | ✅ HOÀN THÀNH | Ocelot routing, JWT validation, Rate limiting |
| **Shared Kernel** | ✅ HOÀN THÀNH | DDD building blocks, Repository, UnitOfWork |

👉 **Tổng cộng: 19 projects** → Tất cả đã build thành công!

---

## 🧰 Công nghệ đã triển khai

| Thành phần | Công nghệ | Phiên bản |
|-------------|-----------|-----------|
| **Frontend** | Next.js + TypeScript + TailwindCSS | Next.js 14 |
| **State Management** | Zustand | Latest |
| **Backend Framework** | .NET + ASP.NET Core Web API | .NET 8.0 |
| **ORM** | Entity Framework Core | EF Core 8.0 |
| **Database** | PostgreSQL (4 databases) | PostgreSQL 15 |
| **Cache** | Redis + StackExchange.Redis | Redis 7 |
| **API Gateway** | Ocelot | 24.0.0 |
| **CQRS** | MediatR | 12.2.0 |
| **Validation** | FluentValidation | 11.9.0 |
| **Security** | BCrypt + JWT (HS256) | Latest |
| **Containerization** | Docker + Docker Compose | Latest |
| **Architecture** | Clean Architecture + DDD + CQRS | - |

---
## Database per Service - ĐÃ TRIỂN KHAI
| Service | Database | Port | Connection String |
|----------|-----------|------|-------------------|
| Identity Service | PostgreSQL | 5432 | tiktok_identity |
| User Service | PostgreSQL | 5435 | tiktok_user |
| Video Service | PostgreSQL | 5433 | tiktok_video |
| Interaction Service | PostgreSQL | 5434 | tiktok_interaction |
| **Redis Cache** | Redis | 6379 | All services |
---

## 🧪 Frontend Overview

### Công nghệ sử dụng
- **Next.js (React + TypeScript)**  
- **TailwindCSS / ShadcnUI / HeroUI**  
- **react-player / HTML5 `<video>`**  
- **SWR** cho data fetching + infinite scroll  
- **socket.io-client** cho realtime update  

### Tính năng
- Đăng nhập / đăng ký  
- Video feed dạng cuộn dọc  
- Tự động phát video trong viewport  
- Like / comment realtime  
- Upload video (file / URL)  
- Lazy load feed  

---

## ⚙️ Backend Overview - ĐÃ TRIỂN KHAI

### Framework & Kiến trúc
- **.NET 8** với ASP.NET Core Web API
- **Clean Architecture** (Domain → Application → Infrastructure → Web)
- **Entity Framework Core 8** kết nối PostgreSQL
- **MediatR** cho CQRS pattern
- **Redis** để cache và counter
- **JWT Authentication** (Bearer tokens)
- **Ocelot** API Gateway

### Tính năng đã hoàn thành
✅ **Identity Service**: Đăng ký, đăng nhập, JWT tokens, BCrypt hashing  
✅ **User Service**: Profile CRUD, Follow/Unfollow, Avatar upload  
✅ **Video Service**: Upload metadata, Video feed, View counter, Pagination  
✅ **Interaction Service**: Like/Unlike, Comment CRUD với soft delete, Reply comments  
✅ **API Gateway**: Routing, Rate limiting (100-200 req/min), CORS  
✅ **Shared Kernel**: DDD building blocks, Repository, UnitOfWork, Result pattern

### Patterns đã áp dụng
- ✅ **Clean Architecture** với 4 layers
- ✅ **Domain-Driven Design** (Aggregates, Value Objects, Domain Events)
- ✅ **CQRS** (Commands & Queries với MediatR)
- ✅ **Repository Pattern** với Generic implementation
- ✅ **Unit of Work** với transaction management
- ✅ **Result Pattern** cho error handling  

---

## 🧱 Docker Compose (mẫu)

```yaml
version: '3.9'
services:
  gateway:
    build: ./gateway
    ports:
      - "8080:8080"
    depends_on:
      - auth
      - video
    environment:
      - REDIS_URL=redis://redis:6379

  auth:
    build: ./auth
    environment:
      - DATABASE_URL=postgresql://postgres:password@postgres:5432/auth
      - REDIS_URL=redis://redis:6379

  video:
    build: ./video
    environment:
      - DATABASE_URL=postgresql://postgres:password@postgres:5432/video
      - REDIS_URL=redis://redis:6379

  realtime:
    build: ./realtime
    ports:
      - "3001:3001"
    environment:
      - REDIS_URL=redis://redis:6379

  redis:
    image: redis:7
    ports:
      - "6379:6379"

  postgres:
    image: postgres:15
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: password
    ports:
      - "5432:5432"
```
## Logging & Monitoring
- **Sentry**: Theo dõi lỗi runtime
- **Grafana + Prometheus**: Giám sát metric hệ thống (CPU, RAM, request rate)
- **Winston logger**: Ghi log request, error, sự kiện socket
## ⚙️ CI/CD Pipeline
**Continuous Integration (CI)**
- Build và test code backend/frontend
- Lint và type-check
- Build Docker images
- Push image lên Docker Hub hoặc AWS ECR
**Continuous Deployment (CD)**
- SSH vào AWS EC2
- Pull latest image
- Restart Docker stack bằng docker-compose
## 🚀 Triển khai AWS EC2
1️⃣ Cài đặt môi trường
```bash
sudo apt update
sudo apt install docker docker-compose -y
```
2️⃣ Clone repo & build
```bash
git clone https://github.com/yourname/tiktok-clone.git
cd tiktok-clone
docker-compose up -d --build
```
3️⃣ Truy cập hệ thống
Frontend: http://<EC2-IP>:3000
Backend:  http://<EC2-IP>:8080
Socket:   ws://<EC2-IP>:3001
## 🚀 Hướng dẫn chạy hệ thống

### Yêu cầu
- .NET 8 SDK
- Docker & Docker Compose
- PostgreSQL 15+
- Redis 7+

### Option 1: Docker Compose (Khuyến nghị)
```bash
cd BackEnd/TiktokClone
docker-compose up -d
```

### Option 2: Chạy từng service
```bash
# 1. Start infrastructure
docker-compose up -d postgres-identity postgres-video postgres-interaction postgres-user redis

# 2. Run all services
cd BackEnd/TiktokClone
.\start-all-services.ps1

# Hoặc chạy thủ công từng service:
# Terminal 1 - API Gateway
cd Gateway/APIGateway.Web && dotnet run

# Terminal 2 - Identity Service  
cd Services/Identity/Src/Identity.Web && dotnet run

# Terminal 3 - Video Service
cd Services/Video/Video.Web && dotnet run

# Terminal 4 - Interaction Service
cd Services/Interaction/Interaction.Web && dotnet run

# Terminal 5 - User Service
cd Services/User/User.Web && dotnet run

# Terminal 6 - Frontend
cd FrontEnd && npm run dev
```

### Truy cập services
- **Frontend**: http://localhost:3000
- **API Gateway**: http://localhost:7000
- **Identity Service**: http://localhost:5001/swagger
- **Video Service**: http://localhost:5002/swagger
- **Interaction Service**: http://localhost:5003/swagger
- **User Service**: http://localhost:5004/swagger

## 📚 Tài liệu chi tiết

- **Backend Implementation**: `BackEnd/README_IMPLEMENTATION.md` - Chi tiết kiến trúc và patterns
- **API Documentation**: `BackEnd/API_DOCUMENTATION.md` - Tất cả endpoints và request/response
- **Quick Start Backend**: `BackEnd/QUICK_START.md` - Hướng dẫn chạy nhanh backend
- **Quick Start Frontend**: `FrontEnd/QUICK_START.md` - Hướng dẫn chạy nhanh frontend
- **Build Summary**: `BackEnd/TiktokClone/FINAL_BUILD_SUMMARY.md` - Tổng quan build hoàn chỉnh

## 🧩 Mở rộng trong tương lai (Optional)

| Service mới | Mục tiêu | Độ ưu tiên |
|-------------|----------|------------|
| **Notification Service** | Push thông báo khi có like, comment, follow | Medium |
| **Analytics Service** | Thống kê lượt xem, thời gian xem, retention | Low |
| **Recommendation Service** | Gợi ý video theo hành vi người dùng (ML/AI) | Low |
| **Payment Service** | Giao dịch donate, quà tặng | Low |
| **Real-time Service** | Socket.IO/SignalR cho live updates | Medium |

## 🧪 Testing (Cần bổ sung)
- **XUnit** cho backend .NET - Unit & Integration tests
- **Jest** cho frontend - Component & Hook tests
- **Postman/Thunder Client** - API testing (có sẵn Swagger)
