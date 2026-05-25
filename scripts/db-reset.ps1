#!/usr/bin/env pwsh
# Drop the local Postgres data volume and re-apply EF Core migrations.
#
# Usage:
#   pwsh scripts/db-reset.ps1              # manual path (docker compose, port 5433)
#   pwsh scripts/db-reset.ps1 -Aspire      # Aspire path (litepress-postgres-data volume)
#
# Stop the running stack before resetting. For Aspire, stop pnpm dev:aspire first.
param(
    [switch]$Aspire
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$Root = Join-Path $PSScriptRoot ".."
Push-Location $Root

function Wait-PostgresHealthy {
    param([int]$MaxAttempts = 30)
    for ($i = 1; $i -le $MaxAttempts; $i++) {
        $status = docker compose ps --format json postgres 2>$null | ConvertFrom-Json
        if ($status -and $status.Health -eq "healthy") {
            return
        }
        Start-Sleep -Seconds 2
    }
    throw "Postgres did not become healthy in time."
}

if ($Aspire) {
    Write-Host "Stopping Aspire resources..."
    & (Join-Path $PSScriptRoot "dev-aspire-stop.ps1")

    Write-Host "Resetting Aspire Postgres volume (litepress-postgres-data)..."
    $rmOutput = docker volume rm litepress-postgres-data -f 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Warning "Could not remove volume. It may still be in use by a running Aspire Postgres container."
        Write-Warning "Run pnpm dev:stop, then run this script again."
        if ($rmOutput) { Write-Warning $rmOutput }
        exit 1
    }
    Write-Host ""
    Write-Host "Volume removed. Start the stack again:"
    Write-Host "  pnpm dev:aspire"
    Write-Host ""
    Write-Host "The API applies pending migrations automatically on startup in Development."
} else {
    Write-Host "Resetting manual-path Postgres (docker compose, port 5433)..."
    docker compose down -v
    docker compose up -d
    Wait-PostgresHealthy

    Write-Host "Applying migrations..."
    dotnet tool restore
    dotnet ef database update `
        --project apps/api/src/LitePress.Infrastructure `
        --startup-project apps/api/src/LitePress.WebApi

    Write-Host ""
    Write-Host "Database recreated. Connection string:"
    Write-Host "  Host=localhost;Port=5433;Database=litepress;Username=litepress;Password=litepress"
}

Pop-Location
