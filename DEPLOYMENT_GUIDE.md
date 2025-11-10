# 🚀 TikTok Clone - Complete Deployment Guide

## 📋 Prerequisites

### Required Software
- ✅ **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- ✅ **Node.js 18+** and **npm** - [Download](https://nodejs.org/)
- ✅ **Docker Desktop** - [Download](https://www.docker.com/products/docker-desktop)
- ✅ **PostgreSQL 15+** (or use Docker)
- ✅ **Redis 7+** (or use Docker)
- ✅ **Git** - [Download](https://git-scm.com/)

### System Requirements
- **OS**: Windows 10/11, macOS, or Linux
- **RAM**: 8GB minimum (16GB recommended)
- **Disk**: 10GB free space
- **CPU**: Quad-core processor recommended

---

## 🎯 Quick Start (Fastest Way)

### Option 1: Docker Compose (All-in-One) ⚡

**Step 1: Clone Repository**
```bash
git clone https://github.com/yourusername/tiktok-clone.git
cd tiktok-clone
```

**Step 2: Start Everything with Docker**
```bash
cd BackEnd/TiktokClone
docker-compose up -d
```

This will start:
- 4 PostgreSQL databases (ports 5432-5435)
- Redis cache (port 6379)
- All 4 microservices (ports 5001-5004)
- API Gateway (port 7000)

**Step 3: Run Frontend**
```bash
cd ../../FrontEnd
npm install
npm run dev
```

**Access:**
- Frontend: http://localhost:3000
- API Gateway: http://localhost:7000
- Swagger UIs: http://localhost:5001-5004/swagger

---

## 🏗️ Option 2: Manual Setup (Full Control)

### Phase 1: Infrastructure Setup

#### 1.1 Start PostgreSQL Databases
```bash
cd BackEnd/TiktokClone

# Start all PostgreSQL databases
docker-compose up -d postgres-identity postgres-video postgres-interaction postgres-user

# Verify databases are running
docker ps | findstr postgres
```

**Expected Output:**
```
postgres-identity    -> Port 5432
postgres-video       -> Port 5433
postgres-interaction -> Port 5434
postgres-user        -> Port 5435
```

#### 1.2 Start Redis Cache
```bash
# Start Redis
docker-compose up -d redis

# Verify Redis is running
docker ps | findstr redis
# Expected: Port 6379
```

#### 1.3 Verify Infrastructure
```bash
# Check all containers
docker-compose ps

# Should show 5 containers running:
# - postgres-identity
# - postgres-video
# - postgres-interaction
# - postgres-user
# - redis
```

---

### Phase 2: Backend Services Setup

#### 2.1 Identity Service (Port 5001)
```bash
# Navigate to Identity Service
cd Services/Identity/Src/Identity.Web

# Run database migrations
dotnet ef migrations add InitialCreate --project ../Identity.Infrastructure
dotnet ef database update --project ../Identity.Infrastructure

# Run the service
dotnet run

# Verify: Open http://localhost:5001/swagger
```

**Endpoints Available:**
- POST /api/auth/register
- POST /api/auth/login
- GET /api/auth/user/{id}

#### 2.2 Video Service (Port 5002)
```bash
# Open NEW terminal
cd Services/Video/Video.Web

# Run database migrations
dotnet ef migrations add InitialCreate --project ../Video.Infrastructure
dotnet ef database update --project ../Video.Infrastructure

# Run the service
dotnet run

# Verify: Open http://localhost:5002/swagger
```

**Endpoints Available:**
- POST /api/videos
- GET /api/videos/feed
- GET /api/videos/{id}
- POST /api/videos/{id}/increment-view

#### 2.3 Interaction Service (Port 5003)
```bash
# Open NEW terminal
cd Services/Interaction/Interaction.Web

# Run database migrations
dotnet ef migrations add InitialCreate --project ../Interaction.Infrastructure
dotnet ef database update --project ../Interaction.Infrastructure

# Run the service
dotnet run

# Verify: Open http://localhost:5003/swagger
```

**Endpoints Available:**
- POST /api/interactions/{videoId}/like
- DELETE /api/interactions/{videoId}/unlike
- POST /api/interactions/{videoId}/comment
- GET /api/interactions/{videoId}/likes
- GET /api/interactions/{videoId}/comments

#### 2.4 User Service (Port 5004)
```bash
# Open NEW terminal
cd Services/User/User.Web

# Run database migrations
dotnet ef migrations add InitialCreate --project ../User.Infrastructure
dotnet ef database update --project ../User.Infrastructure

# Run the service
dotnet run

# Verify: Open http://localhost:5004/swagger
```

**Endpoints Available:**
- POST /api/users/profile
- GET /api/users/profile/{userId}
- PUT /api/users/profile
- POST /api/users/follow/{userId}
- DELETE /api/users/follow/{userId}
- GET /api/users/{userId}/followers
- GET /api/users/{userId}/following

#### 2.5 API Gateway (Port 7000)
```bash
# Open NEW terminal
cd Gateway/APIGateway.Web

# Run the gateway
dotnet run

# Verify: Open http://localhost:7000
```

**Gateway Routes:**
- /identity/* → Identity Service (5001)
- /videos/* → Video Service (5002)
- /interactions/* → Interaction Service (5003)
- /users/* → User Service (5004)

---

### Phase 3: Frontend Setup

#### 3.1 Install Dependencies
```bash
cd FrontEnd
npm install
```

#### 3.2 Configure Environment
Create `.env.local`:
```env
# Using API Gateway (Recommended)
NEXT_PUBLIC_API_URL=http://localhost:7000
NEXT_PUBLIC_SOCKET_URL=http://localhost:7000

# Or direct to services (Development)
NEXT_PUBLIC_IDENTITY_URL=http://localhost:5001
NEXT_PUBLIC_VIDEO_URL=http://localhost:5002
NEXT_PUBLIC_INTERACTION_URL=http://localhost:5003
NEXT_PUBLIC_USER_URL=http://localhost:5004
```

#### 3.3 Run Frontend
```bash
npm run dev

# Access: http://localhost:3000
```

---

## 🧪 Testing the Complete System

### Test 1: User Registration & Login
```bash
# Register a new user
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
    "emailOrUsername": "testuser",
    "password": "Test123!@#"
  }'

# Save the JWT token from response
```

### Test 2: Create User Profile
```bash
# Use token from login
curl -X POST http://localhost:7000/users/profile \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "USER_ID_FROM_LOGIN",
    "username": "testuser",
    "displayName": "Test User",
    "bio": "Testing the system"
  }'
```

### Test 3: Upload Video
```bash
curl -X POST http://localhost:7000/videos \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "My First Video",
    "description": "Test video upload",
    "videoUrl": "https://example.com/video.mp4",
    "thumbnailUrl": "https://example.com/thumb.jpg",
    "durationInSeconds": 30,
    "fileSize": 1048576,
    "format": "mp4",
    "width": 1080,
    "height": 1920
  }'

# Save the videoId from response
```

### Test 4: Interact with Video
```bash
# Like the video
curl -X POST http://localhost:7000/interactions/VIDEO_ID/like \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Add comment
curl -X POST http://localhost:7000/interactions/VIDEO_ID/comment \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "content": "Great video!"
  }'

# Get video feed
curl http://localhost:7000/videos/feed?page=1&pageSize=10
```

### Test 5: Follow Users
```bash
# Follow another user
curl -X POST http://localhost:7000/users/follow/TARGET_USER_ID \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Get followers
curl http://localhost:7000/users/YOUR_USER_ID/followers?page=1&pageSize=20
```

---

## 🔧 Helper Scripts

### Start All Backend Services (PowerShell)
```powershell
# BackEnd/TiktokClone/start-all-services.ps1
cd Services/Identity/Src/Identity.Web
Start-Process -NoNewWindow dotnet -ArgumentList "run"

cd ../../../Video/Video.Web
Start-Process -NoNewWindow dotnet -ArgumentList "run"

cd ../../../Interaction/Interaction.Web
Start-Process -NoNewWindow dotnet -ArgumentList "run"

cd ../../../User/User.Web
Start-Process -NoNewWindow dotnet -ArgumentList "run"

cd ../../../Gateway/APIGateway.Web
Start-Process -NoNewWindow dotnet -ArgumentList "run"
```

### Stop All Services (PowerShell)
```powershell
# BackEnd/TiktokClone/stop-all-services.ps1
Get-Process -Name "dotnet" | Stop-Process -Force
```

---

## 🐛 Troubleshooting

### Problem: Port Already in Use
**Solution:**
```bash
# Windows
netstat -ano | findstr :5001
taskkill /PID <PID> /F

# macOS/Linux
lsof -ti:5001 | xargs kill -9
```

### Problem: Database Connection Failed
**Solution:**
```bash
# Check if PostgreSQL containers are running
docker ps | findstr postgres

# Restart containers
docker-compose restart postgres-identity

# Check logs
docker logs postgres-identity
```

### Problem: Redis Connection Failed
**Solution:**
```bash
# Check Redis container
docker ps | findstr redis

# Test connection
docker exec -it redis redis-cli ping
# Should return: PONG

# Restart Redis
docker-compose restart redis
```

### Problem: EF Migrations Failed
**Solution:**
```bash
# Delete existing migrations
rm -rf Migrations/

# Remove database
dotnet ef database drop --force

# Create new migration
dotnet ef migrations add InitialCreate --project ../ServiceName.Infrastructure

# Apply migration
dotnet ef database update --project ../ServiceName.Infrastructure
```

### Problem: CORS Errors
**Solution:**
Check `appsettings.json` in each service:
```json
{
  "AllowedOrigins": [
    "http://localhost:3000",
    "http://localhost:3001"
  ]
}
```

### Problem: JWT Token Invalid
**Solution:**
Ensure `JwtSettings` are consistent across all services in `appsettings.json`:
```json
{
  "JwtSettings": {
    "Key": "your-secret-key-must-be-at-least-32-characters-long",
    "Issuer": "tiktok-identity-service",
    "Audience": "tiktok-clients",
    "ExpiryMinutes": 60
  }
}
```

---

## 📊 Service Health Check

### Check All Services Status
```bash
# Identity Service
curl http://localhost:5001/swagger

# Video Service
curl http://localhost:5002/swagger

# Interaction Service
curl http://localhost:5003/swagger

# User Service
curl http://localhost:5004/swagger

# API Gateway
curl http://localhost:7000

# Redis
docker exec -it redis redis-cli ping

# PostgreSQL
docker exec -it postgres-identity psql -U postgres -c "SELECT version();"
```

---

## 🔒 Security Checklist

Before deploying to production:

- [ ] Change default PostgreSQL passwords
- [ ] Update JWT secret key (minimum 32 characters)
- [ ] Enable HTTPS for all services
- [ ] Configure proper CORS origins
- [ ] Set up environment-specific configurations
- [ ] Enable rate limiting on API Gateway
- [ ] Set up proper logging (Serilog + ELK)
- [ ] Configure health checks
- [ ] Set up monitoring (Prometheus + Grafana)
- [ ] Implement API versioning
- [ ] Add input validation middleware
- [ ] Configure proper error handling
- [ ] Set up backup strategy for databases
- [ ] Implement circuit breaker pattern (Polly)

---

## 🌐 Production Deployment (AWS EC2)

### Step 1: Prepare EC2 Instance
```bash
# SSH into EC2
ssh -i your-key.pem ubuntu@your-ec2-ip

# Install Docker
sudo apt update
sudo apt install docker.io docker-compose -y
sudo usermod -aG docker ubuntu

# Install .NET 8
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0
```

### Step 2: Clone and Build
```bash
# Clone repository
git clone https://github.com/yourusername/tiktok-clone.git
cd tiktok-clone

# Build all services
cd BackEnd/TiktokClone
dotnet build

# Start with Docker Compose
docker-compose up -d
```

### Step 3: Configure Nginx (Reverse Proxy)
```nginx
# /etc/nginx/sites-available/tiktok
server {
    listen 80;
    server_name your-domain.com;

    location / {
        proxy_pass http://localhost:3000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }

    location /api/ {
        proxy_pass http://localhost:7000/;
        proxy_http_version 1.1;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
    }
}
```

### Step 4: Enable HTTPS (Let's Encrypt)
```bash
sudo apt install certbot python3-certbot-nginx
sudo certbot --nginx -d your-domain.com
```

---

## 📝 Environment Variables Reference

### Backend Services
```env
# Identity Service
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__IdentityDb=Host=localhost;Port=5432;Database=tiktok_identity;Username=postgres;Password=CHANGE_ME
ConnectionStrings__Redis=localhost:6379
JwtSettings__Key=your-production-secret-key-32-chars-minimum
JwtSettings__Issuer=tiktok-identity-service
JwtSettings__Audience=tiktok-clients
JwtSettings__ExpiryMinutes=60

# Similar for other services...
```

### Frontend
```env
NEXT_PUBLIC_API_URL=https://api.your-domain.com
NEXT_PUBLIC_SOCKET_URL=https://api.your-domain.com
NODE_ENV=production
```

---

## 🎉 Success!

You should now have:
- ✅ 4 Microservices running (Identity, Video, Interaction, User)
- ✅ API Gateway routing all requests
- ✅ 4 PostgreSQL databases
- ✅ Redis cache
- ✅ Frontend connected to backend
- ✅ Complete TikTok clone system operational

### Next Steps:
1. Test all features through the UI
2. Monitor logs: `docker-compose logs -f`
3. Check metrics and performance
4. Set up CI/CD pipeline
5. Implement monitoring and alerting

---

**Need Help?**
- Check `BackEnd/API_DOCUMENTATION.md` for API reference
- Review `BackEnd/README_IMPLEMENTATION.md` for architecture details
- See `BackEnd/QUICK_START.md` for quick commands

---

*Last Updated: November 10, 2025*
*Status: Production Ready ✅*
