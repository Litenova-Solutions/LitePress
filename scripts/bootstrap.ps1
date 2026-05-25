#!/usr/bin/env pwsh
Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$Root = Join-Path $PSScriptRoot ".."
Push-Location $Root

function Test-Command($Name) {
    if (-not (Get-Command $Name -ErrorAction SilentlyContinue)) {
        Write-Warning "$Name is not on PATH. Install it before running LitePress."
        return $false
    }
    return $true
}

Write-Host "Initializing git submodules..."
git submodule update --init --recursive

$standardsPath = Join-Path $Root "standards"
Push-Location $standardsPath
$tag = git describe --tags HEAD 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Warning "standards submodule has no tags yet. Pin to a tag once the first release is published."
} else {
    Write-Host "Standards pinned to: $tag"
}
Pop-Location

Write-Host "Checking prerequisites..."
Test-Command "dotnet" | Out-Null
Test-Command "node" | Out-Null
Test-Command "pnpm" | Out-Null
Test-Command "docker" | Out-Null

Write-Host "Restoring pinned dotnet tools..."
dotnet tool restore

Write-Host "Installing frontend dependencies..."
pnpm install

function Copy-ExampleIfMissing($Example, $Target) {
    if (-not (Test-Path $Target) -and (Test-Path $Example)) {
        Copy-Item $Example $Target
        Write-Host "Created $Target from example."
    }
}

$appHost = Join-Path $Root "apps/api/src/LitePress.AppHost"
Copy-ExampleIfMissing (Join-Path $appHost "Properties/launchSettings.json.example") (Join-Path $appHost "Properties/launchSettings.json")
Copy-ExampleIfMissing (Join-Path $appHost "appsettings.Development.json.example") (Join-Path $appHost "appsettings.Development.json")
Copy-ExampleIfMissing (Join-Path $Root "apps/admin/.env.example") (Join-Path $Root "apps/admin/.env.local")

function Test-FrontendUiScaffold($AppPath, $AppName) {
    $missing = @()
    if (-not (Test-Path (Join-Path $AppPath "postcss.config.mjs"))) { $missing += "postcss.config.mjs" }
    if (-not (Test-Path (Join-Path $AppPath "components.json"))) { $missing += "components.json" }
    if (-not (Test-Path (Join-Path $AppPath "components/ui"))) { $missing += "components/ui/" }
    $globals = Join-Path $AppPath "app/globals.css"
    if (-not (Test-Path $globals)) {
        $missing += "app/globals.css"
    } elseif (-not (Select-String -Path $globals -Pattern '@import "tailwindcss"' -Quiet)) {
        $missing += 'app/globals.css (@import "tailwindcss")'
    }
    if ($missing.Count -gt 0) {
        Write-Warning "$AppName missing UI scaffold: $($missing -join ', '). See docs/technical/development.md#frontend-ui-shadcnui"
    }
}

Test-FrontendUiScaffold (Join-Path $Root "apps/web") "apps/web"
Test-FrontendUiScaffold (Join-Path $Root "apps/admin") "apps/admin"

Write-Host ""
Write-Host "Bootstrap complete."
Write-Host "  Recommended: pnpm dev:aspire"
Write-Host "  API docs:     {api-url}/scalar/v1 (Development)"
Write-Host "  Admin OAuth:  copy apps/admin/.env.example to apps/admin/.env.local (or set AppHost user secrets)"
Write-Host "  Manual path:  pwsh scripts/dev-manual.ps1"

Pop-Location
