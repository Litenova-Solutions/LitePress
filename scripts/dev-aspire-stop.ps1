#!/usr/bin/env pwsh
# Stop LitePress Aspire resources left running after Ctrl+C on Windows.
#
# Usage: pwsh scripts/dev-aspire-stop.ps1
Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$RootPattern = [regex]::Escape($Root)

function Stop-ProcessTree {
    param([int]$ProcessId)
    if ($ProcessId -le 0) { return }
    taskkill /PID $ProcessId /T /F 2>$null | Out-Null
}

function Get-LitePressProcessIds {
    Get-CimInstance Win32_Process |
        Where-Object {
            $cmd = $_.CommandLine
            if (-not $cmd) { return $false }
            ($cmd -match 'LitePress\.AppHost') -or
            ($cmd -match 'LitePress\.WebApi') -or
            (($cmd -match $RootPattern) -and ($_.Name -in @('node.exe', 'pnpm.exe')))
        } |
        Select-Object -ExpandProperty ProcessId -Unique
}

Write-Host "Stopping LitePress Aspire processes..."
$pids = @(Get-LitePressProcessIds)
if ($pids.Count -eq 0) {
    Write-Host "  No LitePress AppHost, API, or frontend processes found."
} else {
    foreach ($pid in $pids) {
        Write-Host "  Stopping PID $pid (and child processes)..."
        Stop-ProcessTree -ProcessId $pid
    }
}

Write-Host "Stopping Aspire PostgreSQL containers (litepress-postgres-data)..."
$containerIds = @(docker ps -aq --filter "volume=litepress-postgres-data" 2>$null)
if ($containerIds.Count -eq 0) {
    Write-Host "  No running containers using litepress-postgres-data."
} else {
    foreach ($id in $containerIds) {
        $name = docker inspect --format '{{.Name}}' $id 2>$null
        Write-Host "  Removing container $name ($id)..."
        docker rm -f $id 2>$null | Out-Null
    }
}

Write-Host ""
Write-Host "Aspire stack stopped."
Write-Host "  Start again: pnpm dev:aspire"
Write-Host "  Reset DB:     pnpm db:reset:aspire"
