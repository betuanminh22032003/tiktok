# Backend Quick Start (copy)

This is a copy of `BackEnd/QUICK_START.md` to make backend run instructions easy to find in `docs/`.

## Docker (recommended)
```bash
cd BackEnd/TiktokClone
docker-compose up -d
```

## Manual
```powershell
# Start infrastructure
docker-compose up -d postgres-identity postgres-video postgres-interaction postgres-user redis

# Run identity
cd Services/Identity/Src/Identity.Web
dotnet run

# Run video
cd Services/Video/Video.Web
dotnet run

# Run interaction
cd Services/Interaction/Interaction.Web
dotnet run

# Run user
cd Services/User/User.Web
dotnet run

# Run gateway
cd Gateway/APIGateway.Web
dotnet run
```
