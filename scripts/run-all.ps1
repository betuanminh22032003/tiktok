<#
Run-All.ps1

Usage:
  .\run-all.ps1 -Mode docker    # runs docker-compose (BackEnd/TiktokClone/docker-compose.yml)
  .\run-all.ps1 -Mode local     # starts each service locally (dotnet run) and frontend (npm run dev)
  .\run-all.ps1 -Action stop    # stops processes started by local mode

Notes:
 - Designed for PowerShell on Windows. Starts each service in a new PowerShell window.
 - Saves PIDs to .\run-state\pids.json so you can stop them later with -Action stop.
#>

param(
    [ValidateSet('docker','local')]
    [string]$Mode = 'docker',
    [ValidateSet('start','stop')]
    [string]$Action = 'start'
)

Set-StrictMode -Version Latest

$RepoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition
$RunStateDir = Join-Path $RepoRoot 'run-state'
if (-not (Test-Path $RunStateDir)) { New-Item -ItemType Directory -Path $RunStateDir | Out-Null }
$PidsFile = Join-Path $RunStateDir 'pids.json'

function Start-DockerCompose {
    Write-Host "Starting Docker Compose (BackEnd/TiktokClone) ..." -ForegroundColor Cyan
    Push-Location (Join-Path $RepoRoot 'BackEnd\TiktokClone')
    # Run docker-compose in current window so user sees logs; use -NoExit if you want a new window instead
    docker-compose up --build
    Pop-Location
}

function Start-Local {
    Write-Host "Starting services locally (dotnet run + npm dev) ..." -ForegroundColor Cyan

    $services = @{
        Identity = 'BackEnd/TiktokClone/Services/Identity/Src/Identity.Web'
        User = 'BackEnd/TiktokClone/Services/User/User.Web'
        Video = 'BackEnd/TiktokClone/Services/Video/Video.Web'
        Interaction = 'BackEnd/TiktokClone/Services/Interaction/Interaction.Web'
        Gateway = 'BackEnd/TiktokClone/Gateway/APIGateway.Web'
    }

    $pids = @{}

    foreach ($name in $services.Keys) {
        $rel = $services[$name]
        $path = Join-Path $RepoRoot $rel
        if (-not (Test-Path $path)) {
            Write-Warning "Service path not found: $path — skipping $name"
            continue
        }

        if ($name -eq 'Gateway') {
            $cmd = "$env:ASPNETCORE_ENVIRONMENT='Development'; $env:IDENTITY_AUTHORITY='http://localhost:5116'; cd '$path'; dotnet run"
        } else {
            $cmd = "cd '$path'; dotnet run"
        }

        Write-Host "Starting $name in new window (path: $path)" -ForegroundColor Green
        $proc = Start-Process -FilePath 'powershell' -ArgumentList "-NoExit","-Command",$cmd -PassThru
        $pids[$name] = $proc.Id
        Start-Sleep -Milliseconds 500
    }

    # Start frontend
    $fePath = Join-Path $RepoRoot 'FrontEnd'
    if (Test-Path $fePath) {
        Write-Host "Starting FrontEnd (npm run dev) in new window" -ForegroundColor Green
        $cmd = "cd '$fePath'; npm run dev"
        $proc = Start-Process -FilePath 'powershell' -ArgumentList "-NoExit","-Command",$cmd -PassThru
        $pids['FrontEnd'] = $proc.Id
    } else {
        Write-Warning "FrontEnd folder not found at $fePath"
    }

    # Save PIDs
    $pids | ConvertTo-Json | Out-File -FilePath $PidsFile -Encoding utf8
    Write-Host "Saved PIDs to $PidsFile" -ForegroundColor Cyan
}

function Stop-Local {
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
    Write-Host "Stopped local services and removed $PidsFile" -ForegroundColor Cyan
}

if ($Mode -eq 'docker') {
    if ($Action -eq 'start') { Start-DockerCompose }
    elseif ($Action -eq 'stop') {
        Push-Location (Join-Path $RepoRoot 'BackEnd\TiktokClone')
        docker-compose down
        Pop-Location
    }
} else {
    if ($Action -eq 'start') { Start-Local }
    elseif ($Action -eq 'stop') { Stop-Local }
}
