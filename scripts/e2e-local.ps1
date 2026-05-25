#!/usr/bin/env pwsh
# Local E2E stack mirroring .github/workflows/e2e.yml
Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$Root = Join-Path $PSScriptRoot ".."
Push-Location $Root

$env:ConnectionStrings__Database = "Host=localhost;Port=5433;Database=litepress;Username=litepress;Password=litepress"
$env:JwtSettings__Secret = "dev-secret-key-must-be-at-least-32-characters-long!"
$env:E2E_API_URL = "http://localhost:5000"
$env:API_JWT_SECRET = "dev-secret-key-must-be-at-least-32-characters-long!"
$env:PLAYWRIGHT_BASE_URL = "http://localhost:3000"
$env:API_URL = "http://localhost:5000"
$env:SITE_URL = "http://localhost:3000"

Write-Host "Starting Postgres..."
docker compose up -d

Write-Host "Building API..."
Push-Location apps/api
dotnet restore LitePress.slnx
dotnet build LitePress.slnx --configuration Release --no-restore
Pop-Location

Write-Host "Applying migrations..."
dotnet tool restore
dotnet ef database update `
  --project apps/api/src/LitePress.Infrastructure `
  --startup-project apps/api/src/LitePress.WebApi `
  --configuration Release

Write-Host "Starting API in background..."
$apiJob = Start-Job {
    param($Root)
    Set-Location $Root
    $env:ConnectionStrings__Database = "Host=localhost;Port=5433;Database=litepress;Username=litepress;Password=litepress"
    $env:JwtSettings__Secret = "dev-secret-key-must-be-at-least-32-characters-long!"
    dotnet run --project apps/api/src/LitePress.WebApi --configuration Release --no-build --urls http://localhost:5000
} -ArgumentList $Root

try {
    for ($i = 1; $i -le 30; $i++) {
        try {
            Invoke-WebRequest -Uri "http://localhost:5000/openapi/v1.json" -UseBasicParsing -TimeoutSec 2 | Out-Null
            break
        } catch {
            if ($i -eq 30) { throw "API did not become ready in time." }
            Start-Sleep -Seconds 2
        }
    }

    Write-Host "Building web app..."
    pnpm turbo build --filter=web

    Push-Location apps/web
    pnpm exec playwright install chromium
    pnpm exec playwright test
    Pop-Location
}
finally {
    Stop-Job $apiJob -ErrorAction SilentlyContinue
    Remove-Job $apiJob -Force -ErrorAction SilentlyContinue
}

Pop-Location
