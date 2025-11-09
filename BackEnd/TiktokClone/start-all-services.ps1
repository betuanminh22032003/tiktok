# TikTok Clone Backend - Start All Services
# This script starts all microservices in separate PowerShell windows

Write-Host "🚀 Starting TikTok Clone Microservices..." -ForegroundColor Cyan
Write-Host ""

$BackendRoot = $PSScriptRoot

# Function to start a service in a new PowerShell window
function Start-Service {
    param(
        [string]$ServiceName,
        [string]$ServicePath,
        [int]$Port,
        [string]$Color
    )
    
    $fullPath = Join-Path $BackendRoot $ServicePath
    
    Write-Host "▶️  Starting $ServiceName on port $Port..." -ForegroundColor $Color
    
    Start-Process powershell -ArgumentList @(
        "-NoExit",
        "-Command",
        "cd '$fullPath'; `$host.UI.RawUI.WindowTitle = '$ServiceName - Port $Port'; dotnet run"
    )
}

# Start all services
Write-Host "Starting services in 3 seconds..." -ForegroundColor Yellow
Start-Sleep -Seconds 3

# 1. Identity Service (Port 5001)
Start-Service -ServiceName "Identity Service" -ServicePath "Services\Identity\src\Identity.Web" -Port 5001 -Color "Green"
Start-Sleep -Seconds 2

# 2. Video Service (Port 5002)
Start-Service -ServiceName "Video Service" -ServicePath "Services\Video\Video.Web" -Port 5002 -Color "Blue"
Start-Sleep -Seconds 2

# 3. Interaction Service (Port 5003)
Start-Service -ServiceName "Interaction Service" -ServicePath "Services\Interaction\Interaction.Web" -Port 5003 -Color "Magenta"
Start-Sleep -Seconds 2

# 4. User Service (Port 5004)
Start-Service -ServiceName "User Service" -ServicePath "Services\User\User.Web" -Port 5004 -Color "Cyan"
Start-Sleep -Seconds 2

# 5. API Gateway (Port 7000)
Start-Service -ServiceName "API Gateway" -ServicePath "Gateway\APIGateway.Web" -Port 7000 -Color "Yellow"

Write-Host ""
Write-Host "✅ All services starting!" -ForegroundColor Green
Write-Host ""
Write-Host "📡 Service URLs:" -ForegroundColor Cyan
Write-Host "  • API Gateway:    http://localhost:7000" -ForegroundColor White
Write-Host "  • Identity:       http://localhost:5001/swagger" -ForegroundColor White
Write-Host "  • Video:          http://localhost:5002/swagger" -ForegroundColor White
Write-Host "  • Interaction:    http://localhost:5003/swagger" -ForegroundColor White
Write-Host "  • User:           http://localhost:5004/swagger" -ForegroundColor White
Write-Host ""
Write-Host "⚠️  Make sure PostgreSQL and Redis are running!" -ForegroundColor Yellow
Write-Host "   docker-compose up -d postgres-identity postgres-video postgres-interaction postgres-user redis" -ForegroundColor Gray
Write-Host ""
Write-Host "Press any key to close this window..." -ForegroundColor DarkGray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
