#!/usr/bin/env pwsh
# Manual dev stack: docker-compose Postgres + API. Start frontends in separate terminals.
Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$Root = Join-Path $PSScriptRoot ".."
Push-Location $Root

Write-Host "Starting docker-compose Postgres on port 5433..."
docker compose up -d

Write-Host "Applying migrations..."
dotnet tool restore
dotnet ef database update `
  --project apps/api/src/LitePress.Infrastructure `
  --startup-project apps/api/src/LitePress.WebApi

Write-Host "Starting API (Ctrl+C to stop)..."
Write-Host "In other terminals: pnpm dev:web and pnpm dev:admin"
dotnet run --project apps/api/src/LitePress.WebApi

Pop-Location
