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

... (file copied from root `DEPLOYMENT_GUIDE.md`)
