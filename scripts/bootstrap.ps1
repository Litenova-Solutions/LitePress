#!/usr/bin/env pwsh
Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

git submodule update --init --recursive

$standardsPath = Join-Path $PSScriptRoot ".." "standards"
Push-Location $standardsPath
$tag = git describe --tags HEAD 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Warning "standards submodule has no tags yet. Pin to a tag once the first release is published."
} else {
    Write-Host "Standards pinned to: $tag"
}
Pop-Location
