#!/bin/bash

# Build and pack .NET NuGet package
# Usage: ./build-dotnet.sh [version]

set -e

VERSION=${1:-"1.0.0"}
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$SCRIPT_DIR/dotnet/src/ArtSwagger"
OUTPUT_DIR="$SCRIPT_DIR/artifacts/nuget"

echo "🔨 Building ArtSwagger for .NET..."
echo "Version: $VERSION"

# Clean
rm -rf "$OUTPUT_DIR"
mkdir -p "$OUTPUT_DIR"

# Build for all frameworks
cd "$PROJECT_DIR"
dotnet restore
dotnet build -c Release

# Pack
dotnet pack -c Release -o "$OUTPUT_DIR" /p:Version="$VERSION"

echo "✅ NuGet package created at: $OUTPUT_DIR"
echo ""
echo "To publish to NuGet.org:"
echo "  dotnet nuget push $OUTPUT_DIR/ArtSwagger.$VERSION.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json"
