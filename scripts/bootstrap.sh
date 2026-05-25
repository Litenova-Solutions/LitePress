#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT"

check_cmd() {
  if ! command -v "$1" >/dev/null 2>&1; then
    echo "warning: $1 is not on PATH. Install it before running LitePress." >&2
  fi
}

echo "Initializing git submodules..."
git submodule update --init --recursive

if pushd standards >/dev/null; then
  if tag=$(git describe --tags HEAD 2>/dev/null); then
    echo "Standards pinned to: $tag"
  else
    echo "warning: standards submodule has no tags yet." >&2
  fi
  popd >/dev/null
fi

echo "Checking prerequisites..."
check_cmd dotnet
check_cmd node
check_cmd pnpm
check_cmd docker

echo "Restoring pinned dotnet tools..."
dotnet tool restore

echo "Installing frontend dependencies..."
pnpm install

echo ""
echo "Bootstrap complete."
echo "  Recommended: pnpm dev:aspire"
echo "  Admin OAuth:  cp apps/admin/.env.example apps/admin/.env.local"
echo "  Manual path:  bash scripts/dev-manual.sh"
