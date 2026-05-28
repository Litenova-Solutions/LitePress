param(
    [string]$AcceptanceTestsRoot = "apps/api/tests"
)

$standardsScript = Join-Path $PSScriptRoot "..\standards\scripts\validate-feature-files.ps1"
if (-not (Test-Path $standardsScript)) {
    $standardsScript = Join-Path $PSScriptRoot "validate-feature-files.ps1"
}

& $standardsScript -AcceptanceTestsRoot $AcceptanceTestsRoot
exit $LASTEXITCODE
