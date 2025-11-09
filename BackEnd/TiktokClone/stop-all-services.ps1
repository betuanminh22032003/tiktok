# Stop All TikTok Clone Services
# This script finds and stops all dotnet processes related to TikTok Clone services

Write-Host "🛑 Stopping TikTok Clone Services..." -ForegroundColor Red
Write-Host ""

# Get all dotnet processes
$dotnetProcesses = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue

if ($dotnetProcesses) {
    $serviceNames = @(
        "Identity.Web",
        "Video.Web", 
        "Interaction.Web",
        "User.Web",
        "APIGateway.Web"
    )
    
    foreach ($process in $dotnetProcesses) {
        try {
            $commandLine = (Get-CimInstance Win32_Process -Filter "ProcessId = $($process.Id)").CommandLine
            
            foreach ($serviceName in $serviceNames) {
                if ($commandLine -like "*$serviceName*") {
                    Write-Host "  Stopping $serviceName (PID: $($process.Id))..." -ForegroundColor Yellow
                    Stop-Process -Id $process.Id -Force
                    Start-Sleep -Milliseconds 500
                    break
                }
            }
        }
        catch {
            # Process may have already exited
        }
    }
    
    Write-Host ""
    Write-Host "✅ All services stopped!" -ForegroundColor Green
}
else {
    Write-Host "ℹ️  No running dotnet processes found." -ForegroundColor Gray
}

Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor DarkGray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
