#!/bin/bash

# Build and deploy Java Maven artifact
# Usage: ./build-java.sh [version]

set -e

VERSION=${1:-"1.0.0"}
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$SCRIPT_DIR/java/art-swagger-spring-boot-starter"
OUTPUT_DIR="$SCRIPT_DIR/artifacts/maven"

echo "🔨 Building ArtSwagger for Java..."
echo "Version: $VERSION"

# Clean
rm -rf "$OUTPUT_DIR"
mkdir -p "$OUTPUT_DIR"

cd "$PROJECT_DIR"

# Update version in pom.xml
if [[ "$OSTYPE" == "darwin"* ]]; then
    sed -i '' "s/<version>.*<\/version>/<version>$VERSION<\/version>/" pom.xml
else
    sed -i "s/<version>.*<\/version>/<version>$VERSION<\/version>/" pom.xml
fi

# Build
mvn clean package -DskipTests

# Copy artifacts
cp target/*.jar "$OUTPUT_DIR/"

echo "✅ Maven artifacts created at: $OUTPUT_DIR"
echo ""
echo "To deploy to Maven Central:"
echo "  1. Configure your ~/.m2/settings.xml with OSSRH credentials"
echo "  2. Run: mvn clean deploy -Prelease"
echo ""
echo "For local install:"
echo "  mvn clean install"
