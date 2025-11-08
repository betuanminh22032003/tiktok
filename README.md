# 🎬 TikTok Clone Web App (Microservices Architecture)

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

## 🔩 Thành phần hệ thống (Microservices)

| Service | Nhiệm vụ chính | Công nghệ đề xuất |
|----------|----------------|------------------|
| **API Gateway / BFF** | - Nhận request từ client<br>- Kiểm tra token, forward đến service<br>- Có thể xử lý aggregation | .NET |
| **Auth Service** | - Đăng nhập / đăng ký<br>- JWT + Refresh token (HttpOnly cookie)<br>- Redis cache token | .NET + PostgreSQL + Redis |
| **User Service** | - CRUD thông tin người dùng, profile, avatar | Node.js + PostgreSQL |
| **Video Service** | - Metadata video<br>- Upload file / URL<br>- Phân trang feed | .NET + PostgreSQL / MongoDB |
| **Interaction Service** | - Like, comment, view<br>- Realtime counter<br>- Redis cache | Node.js + Redis + PostgreSQL |
| **Realtime Service** | - Socket.io server<br>- Broadcast sự kiện like/comment | Node.js + Socket.io + Redis Pub/Sub |
| **Logging & Monitoring** | - Ghi log & metric<br>- Giám sát lỗi | Grafana + Prometheus + Sentry |
| **File Service (optional)** | - Upload video, lưu file local hoặc mock S3 | Express + Multer + AWS SDK |

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

## 🧱 Cấu trúc demo tối thiểu (4 services)

| Service | Bao gồm |
|----------|----------|
| Auth + User | ✅ |
| Video + Interaction (gộp) | ✅ |
| Realtime (Socket.io) | ✅ |
| Redis + PostgreSQL | ✅ |

👉 Tổng cộng: **4 container** → `frontend`, `backend`, `redis`, `postgres`.

---

## 🧰 Công nghệ tổng quan

| Thành phần | Công nghệ |
|-------------|-----------|
| **Frontend** | Next.js + TailwindCSS + SWR + react-player |
| **State Management** | React Context / Zustand |
| **Backend** | .NET |
| **Database** | PostgreSQL + Prisma ORM |
| **Cache / Queue** | Redis |
| **Realtime** | Socket.io + Redis adapter |
| **Logging** | Winston + Sentry |
| **Monitoring** | Prometheus + Grafana |
| **Containerization** | Docker + Docker Compose |
| **CI/CD** | GitHub Actions |
| **Deploy** | AWS EC2 (Ubuntu + Docker Compose) |

---
## Database per Service
| Service | Database |
|----------|-----------|
| Identity Service | PostgreSQL|
| User Service | PostgreSQL|
| Video Service | (PostgreSQL) |
| Interaction Service | NoSQL (MongoDB / Cassandra / DynamoDB) |
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

## ⚙️ Backend Overview

### Framework
- **.NET (Node.js + TypeScript)**
- **Prisma ORM** kết nối PostgreSQL  
- **Socket.io** để realtime update  
- **Redis** để cache và Pub/Sub  
- **JWT Authentication** với HttpOnly cookie  

### Tính năng
- Auth: đăng ký, đăng nhập, refresh token  
- Video: CRUD metadata, pagination  
- Interaction: like, comment, view  
- Realtime: socket.io broadcast khi có thay đổi  

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
## 🧩 Mở rộng Microservices trong tương lai
Service mới	Mục tiêu
Notification Service	Push thông báo khi có like, comment, follow
Analytics Service	Thống kê lượt xem, thời gian xem, retention
Recommendation Service	Gợi ý video theo hành vi người dùng
Payment Service	Giao dịch donate, quà tặng
## 🧪 Unit Test
- **Jest** cho frontend
- **XUnit** cho backend .NET
- **Supertest** cho API integration test
