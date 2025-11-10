# 🚀 Quick Start Guide - TikTok Clone Backend

## Prerequisites

- .NET 8 SDK
- PostgreSQL 15+
- Redis
- Docker & Docker Compose (optional but recommended)

---

## Option 1: Run with Docker Compose (Recommended)

### 1. Start Infrastructure Only (PostgreSQL + Redis)
```bash
cd BackEnd/TiktokClone

# Start databases
docker-compose up -d postgres-identity redis
```

### 2. Run Identity Service Locally
```bash
cd Services/Identity/Src/Identity.Web

# Apply migrations
dotnet ef migrations add InitialCreate --project ../Identity.Infrastructure
dotnet ef database update

# Run service
dotnet run
```

### 3. Access Services
- **Identity API**: http://localhost:5001
- **Swagger UI**: http://localhost:5001/swagger

---

## Option 2: Run Everything Locally

### 1. Install Dependencies
```bash
# Install PostgreSQL
# Windows: Download from postgresql.org
# Mac: brew install postgresql
# Linux: sudo apt-get install postgresql

# Install Redis
# Windows: Download from redis.io or use WSL
# Mac: brew install redis
# Linux: sudo apt-get install redis-server

# Start services
# PostgreSQL: pg_ctl start
# Redis: redis-server
```

### 2. Create Databases
```sql
CREATE DATABASE tiktok_identity;
CREATE DATABASE tiktok_video;
CREATE DATABASE tiktok_interaction;
CREATE DATABASE tiktok_user;
```

### 3. Update Connection Strings
Edit `appsettings.json` in each service:
```json
{
  "ConnectionStrings": {
    "IdentityDb": "Host=localhost;Port=5432;Database=tiktok_identity;Username=postgres;Password=postgres",
    "Redis": "localhost:6379"
  }
}
```

### 4. Run Migrations
```bash
cd BackEnd/TiktokClone/Services/Identity/Src/Identity.Web
dotnet ef migrations add InitialCreate --project ../Identity.Infrastructure
dotnet ef database update
```

### 5. Run Service
```bash
dotnet run
```

---

## Testing the API

### Register a User
```bash
curl -X POST http://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "username": "testuser",
    "password": "Test123456"
  }'
```

### Login
```bash
curl -X POST http://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "emailOrUsername": "testuser",
    "password": "Test123456"
  }'
```

### Get Current User (Protected)
```bash
curl -X GET http://localhost:5001/api/auth/me \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

---

## Architecture Overview

```
┌─────────────────────────────────────────────────────────┐
│                    Frontend (Next.js)                    │
│                   http://localhost:3000                  │
└────────────────────────┬────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────┐
│                   API Gateway (Ocelot)                   │
│                   http://localhost:5000                  │
└─────┬──────────┬──────────┬──────────┬──────────────────┘
      │          │          │          │
      ▼          ▼          ▼          ▼
  ┌────────┐ ┌────────┐ ┌──────────┐ ┌────────┐
  │Identity│ │ Video  │ │Interaction│ │ User   │
  │:5001   │ │:5002   │ │  :5003    │ │:5004   │
  └───┬────┘ └───┬────┘ └─────┬─────┘ └───┬────┘
      │          │            │            │
      ▼          ▼            ▼            ▼
  ┌──────────────────────────────────────────┐
  │            PostgreSQL Cluster             │
  │  Identity│Video│Interaction│User          │
  │   :5432  │:5433│  :5434    │:5435         │
  └──────────────────────────────────────────┘
                     │
                     ▼
              ┌──────────┐
              │  Redis   │
              │  :6379   │
              └──────────┘
```

---

## Project Structure - HOÀN THÀNH

```
BackEnd/TiktokClone/
├── Shared/
│   └── TiktokClone.SharedKernel/     # ✅ DDD building blocks, Repository, UnitOfWork
├── Services/
│   ├── Identity/                      # ✅ HOÀN THÀNH (4 projects)
│   │   └── Src/
│   │       ├── Identity.Domain/       # User aggregate, Email VO, Events
│   │       ├── Identity.Application/  # Register/Login commands
│   │       ├── Identity.Infrastructure/ # EF Core, JWT, BCrypt
│   │       └── Identity.Web/          # API Controller, Swagger
│   ├── Video/                         # ✅ HOÀN THÀNH (4 projects)
│   │   ├── Video.Domain/              # Video aggregate, ValueObjects
│   │   ├── Video.Application/         # Upload/Feed commands & queries
│   │   ├── Video.Infrastructure/      # VideoDbContext, Repository, Redis
│   │   └── Video.Web/                 # VideosController
│   ├── Interaction/                   # ✅ HOÀN THÀNH (4 projects)
│   │   ├── Interaction.Domain/        # Like & Comment aggregates
│   │   ├── Interaction.Application/   # Like/Comment commands & queries
│   │   ├── Interaction.Infrastructure/ # InteractionDbContext, Repositories
│   │   └── Interaction.Web/           # InteractionsController
│   └── User/                          # ✅ HOÀN THÀNH (4 projects)
│       ├── User.Domain/               # UserProfile & Follow entities
│       ├── User.Application/          # Profile/Follow commands & queries
│       ├── User.Infrastructure/       # UserDbContext, Repositories
│       └── User.Web/                  # UsersController
├── Gateway/
│   └── APIGateway.Web/                # ✅ HOÀN THÀNH (Ocelot routing)
└── docker-compose.yml                 # ✅ HOÀN THÀNH (All services + DBs)
```

---

## Environment Variables

### Identity Service
```env
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__IdentityDb=Host=localhost;Port=5432;Database=tiktok_identity;Username=postgres;Password=postgres
ConnectionStrings__Redis=localhost:6379
JwtSettings__Key=YOUR_SECRET_KEY_HERE_CHANGE_IN_PRODUCTION
JwtSettings__Issuer=tiktok-identity-service
JwtSettings__Audience=tiktok-clients
JwtSettings__ExpiryMinutes=60
```

---

## Common Commands

### Entity Framework Migrations
```bash
# Add migration
dotnet ef migrations add MigrationName --project ../Identity.Infrastructure

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove --project ../Identity.Infrastructure

# Generate SQL script
dotnet ef migrations script --project ../Identity.Infrastructure
```

### Docker
```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f identity-service

# Stop all services
docker-compose down

# Remove volumes (WARNING: deletes data)
docker-compose down -v
```

### Redis CLI
```bash
# Connect to Redis
redis-cli

# Check keys
KEYS *

# Get value
GET key_name

# Monitor commands
MONITOR
```

---

## Troubleshooting

### Port Already in Use
```bash
# Windows
netstat -ano | findstr :5001
taskkill /PID <PID> /F

# Mac/Linux
lsof -ti:5001 | xargs kill -9
```

### Database Connection Failed
```bash
# Check PostgreSQL is running
# Windows: services.msc -> PostgreSQL
# Mac/Linux: pg_isready

# Test connection
psql -h localhost -U postgres -d tiktok_identity
```

### Redis Connection Failed
```bash
# Check Redis is running
redis-cli ping
# Should return: PONG
```

---

## ✅ Completed Steps

1. ✅ **Identity Service** - Complete (Register, Login, JWT)
2. ✅ **Video Service** - Complete (Upload, Feed, View counter)
3. ✅ **Interaction Service** - Complete (Like, Comment CRUD)
4. ✅ **User Service** - Complete (Profile, Follow/Unfollow)
5. ✅ **API Gateway** - Complete (Ocelot, Rate limiting)
6. ✅ **Docker Compose** - Complete (All services + DBs)
7. ✅ **Swagger Documentation** - Complete (All services)

## 🎯 Optional Enhancements

1. ⏳ **Add Real-time with SignalR/Socket.IO** - For live updates
2. ⏳ **Write Integration Tests** - XUnit + TestContainers
3. ⏳ **Add Health Checks** - Monitor service health
4. ⏳ **Implement Logging** - Serilog + ELK Stack
5. ⏳ **Add Monitoring** - Prometheus + Grafana

---

## Support

For issues or questions:
- Check `README_IMPLEMENTATION.md` for architecture details
- Check `IMPLEMENTATION_STATUS.md` for current status
- Review Swagger docs at `/swagger` endpoint
