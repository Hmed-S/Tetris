#!/bin/sh

set -eu

echo "======================================"
echo "       TETRIS CODE COVERAGE"
echo "======================================"

# Remove previous test results to avoid stale coverage files.
echo ""
echo "[1/3] Cleaning previous test results..."

find . -type d -name TestResults -prune -exec rm -rf {} +

# Run all tests and collect coverage.
echo ""
echo "[2/3] Running tests and collecting coverage..."

dotnet test Tetris.sln \
    --configuration Release \
    --collect:"XPlat Code Coverage"

# Generate combined HTML report.
echo ""
echo "[3/3] Generating coverage report..."

dotnet reportgenerator \
    "-reports:**/coverage.cobertura.xml" \
    "-targetdir:artifacts/coverage" \
    "-reporttypes:Html"

echo ""
echo "======================================"
echo "       COVERAGE COMPLETED"
echo "======================================"

echo ""
echo "HTML report:"
echo "artifacts/coverage/index.html"
