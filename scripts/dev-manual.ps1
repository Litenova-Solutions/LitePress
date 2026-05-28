#!/usr/bin/env pwsh
# Manual dev stack: docker-compose PostgreSQL + API. Start frontends in separate terminals.
Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$Root = Join-Path $PSScriptRoot ".."
Push-Location $Root

Write-Host "Starting docker-compose PostgreSQL on port 5432..."
docker compose up -d

Write-Host "Applying Marten storage schema..."
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run --project apps/api/src/LitePress.WebApi -- --apply-schema-only

Write-Host "Starting API (Ctrl+C to stop)..."
Write-Host "In other terminals: pnpm dev:web and pnpm dev:admin"
dotnet run --project apps/api/src/LitePress.WebApi

Pop-Location
