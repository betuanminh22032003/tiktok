<#
run-dotnet-all.ps1

Simple helper to start all services with `dotnet run` in separate PowerShell windows.
Usage:
  .\run-dotnet-all.ps1 start   # start services (opens windows, shows logs)
  .\run-dotnet-all.ps1 stop    # stop services started earlier

Notes:
 - Requires .NET SDK installed and `dotnet restore` completed for each project.
 - This script forces ASPNETCORE_URLS for predictable dev ports so the API Gateway (dev ocelot) routes work.
 - PIDs recorded in run-state/pids-dotnet.json
#>

param(
    [ValidateSet('start','stop')]
    [string]$Action = 'start'
)

Set-StrictMode -Version Latest

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
# Repo root is the parent of the scripts directory
$RepoRoot = Split-Path -Parent $ScriptDir
$RunStateDir = Join-Path $RepoRoot 'run-state'
if (-not (Test-Path $RunStateDir)) { New-Item -ItemType Directory -Path $RunStateDir | Out-Null }
$PidsFile = Join-Path $RunStateDir 'pids-dotnet.json'

$services = @{
    Identity = @{ Path = 'BackEnd/TiktokClone/Services/Identity/Src/Identity.Web'; Url = 'http://localhost:5116' }
    User = @{ Path = 'BackEnd/TiktokClone/Services/User/User.Web'; Url = 'http://localhost:5287' }
    Video = @{ Path = 'BackEnd/TiktokClone/Services/Video/Video.Web'; Url = 'http://localhost:5203' }
    Gateway = @{ Path = 'BackEnd/TiktokClone/Gateway/APIGateway.Web'; Url = 'http://localhost:7000'; IdentityAuthority = 'http://localhost:5116' }
    # FrontEnd is optional; uncomment if you want to start Next.js dev server in a new window
    # FrontEnd = @{ Path = 'FrontEnd'; Cmd = 'npm run dev' }
}

function Start-Services {
    $pids = @{}
    foreach ($name in $services.Keys) {
        $svc = $services[$name]
        $rel = $svc.Path
        $path = Join-Path $RepoRoot $rel
        if (-not (Test-Path $path)) {
            Write-Warning "Service path not found: $path - skipping $name"
            continue
        }

        $envParts = @()
        if ($svc.Url) { $envParts += ('$env:ASPNETCORE_URLS=' + "'" + $svc.Url + "'") }
        if ($name -eq 'Gateway' -and $svc.IdentityAuthority) { $envParts += ('$env:IDENTITY_AUTHORITY=' + "'" + $svc.IdentityAuthority + "'") }
        $envParts += ('$env:ASPNETCORE_ENVIRONMENT=' + "'Development'")
        $envCmd = [string]::Join('; ', $envParts) + ';'

        # Build command to run in new window
        $cmd = "$envCmd cd '$path'; dotnet run"

        Write-Host "Starting $name -> $path" -ForegroundColor Green
        $proc = Start-Process -FilePath 'powershell' -ArgumentList '-NoExit','-Command',$cmd -PassThru
        $pids[$name] = $proc.Id
        Start-Sleep -Milliseconds 400
    }

    if ($pids.Count -gt 0) {
        $pids | ConvertTo-Json | Out-File -FilePath $PidsFile -Encoding utf8
        Write-Host "Started services and saved PIDs to $PidsFile" -ForegroundColor Cyan
    } else {
        Write-Warning "No services started."
    }
}

function Stop-Services {
    if (-not (Test-Path $PidsFile)) { Write-Warning "No pids file found at $PidsFile"; return }
    $data = Get-Content $PidsFile -Raw | ConvertFrom-Json
    foreach ($name in $data.PSObject.Properties.Name) {
        $procId = [int]$data.$name
        try {
            Write-Host "Stopping $name (PID $procId)" -ForegroundColor Yellow
            Stop-Process -Id $procId -Force -ErrorAction Stop
        } catch {
            Write-Warning ("Failed to stop PID {0}: {1}" -f $procId, $_)
        }
    }
    Remove-Item $PidsFile -ErrorAction SilentlyContinue
    Write-Host "Stopped services and removed $PidsFile" -ForegroundColor Cyan
}

if ($Action -eq 'start') { Start-Services } else { Stop-Services }