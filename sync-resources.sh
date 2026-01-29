#!/bin/bash

# Sync shared frontend resources to .NET and Java projects
# Run this after modifying files in shared/resources/

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
SHARED_DIR="$SCRIPT_DIR/shared/resources"
DOTNET_DIR="$SCRIPT_DIR/dotnet/src/ArtSwagger/wwwroot"
JAVA_DIR="$SCRIPT_DIR/java/art-swagger-spring-boot-starter/src/main/resources/static/art-swagger"

echo "📦 Syncing shared resources..."

# Sync to .NET
echo "  → Syncing to .NET project..."
rm -rf "$DOTNET_DIR"
mkdir -p "$DOTNET_DIR"
cp -r "$SHARED_DIR"/* "$DOTNET_DIR/"

# Sync to Java
echo "  → Syncing to Java project..."
rm -rf "$JAVA_DIR"
mkdir -p "$JAVA_DIR"
cp -r "$SHARED_DIR"/* "$JAVA_DIR/"

echo "✅ Resources synced successfully!"
echo ""
echo "Files synced:"
ls -la "$SHARED_DIR"
