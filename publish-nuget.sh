#!/bin/bash

# NuGet Package Publishing Script for DotNet.WpfToolkit
# Usage: ./publish-nuget.sh [api-key]

set -e  # Exit on error

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}  DotNet.WpfToolkit NuGet Publisher${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""

# Check if API key is provided
if [ -z "$1" ] && [ -z "$NUGET_API_KEY" ]; then
    echo -e "${RED}Error: NuGet API key not provided!${NC}"
    echo ""
    echo "Usage:"
    echo "  ./publish-nuget.sh YOUR_API_KEY"
    echo "  or"
    echo "  export NUGET_API_KEY=YOUR_API_KEY && ./publish-nuget.sh"
    exit 1
fi

# Set API key
if [ ! -z "$1" ]; then
    NUGET_API_KEY=$1
fi

# Navigate to project directory
echo -e "${YELLOW}Navigating to project directory...${NC}"
cd DotNet.WpfToolkit

# Clean previous builds
echo -e "${YELLOW}Cleaning previous builds...${NC}"
dotnet clean --configuration Release

# Build project
echo -e "${YELLOW}Building project in Release mode...${NC}"
dotnet build --configuration Release

# Create NuGet package
echo -e "${YELLOW}Creating NuGet package...${NC}"
dotnet pack --configuration Release --output ./nupkg

# Get package version from csproj
VERSION=$(grep -oP '<Version>\K[^<]+' DotNet.WpfToolkit.csproj)
PACKAGE_FILE="nupkg/DotNet.WpfToolKit.${VERSION}.nupkg"
SYMBOLS_FILE="nupkg/DotNet.WpfToolKit.${VERSION}.snupkg"

echo -e "${GREEN}Package created: ${PACKAGE_FILE}${NC}"

# Verify package exists
if [ ! -f "$PACKAGE_FILE" ]; then
    echo -e "${RED}Error: Package file not found!${NC}"
    exit 1
fi

# Ask for confirmation
echo ""
echo -e "${YELLOW}Ready to publish DotNet.WpfToolKit v${VERSION} to NuGet.org${NC}"
echo -n "Continue? (y/n): "
read -r CONFIRM

if [ "$CONFIRM" != "y" ] && [ "$CONFIRM" != "Y" ]; then
    echo -e "${RED}Publishing cancelled.${NC}"
    exit 0
fi

# Push main package
echo -e "${YELLOW}Publishing package to NuGet.org...${NC}"
dotnet nuget push "$PACKAGE_FILE" \
    --api-key "$NUGET_API_KEY" \
    --source https://api.nuget.org/v3/index.json \
    --skip-duplicate

# Push symbols package if exists
if [ -f "$SYMBOLS_FILE" ]; then
    echo -e "${YELLOW}Publishing symbols package...${NC}"
    dotnet nuget push "$SYMBOLS_FILE" \
        --api-key "$NUGET_API_KEY" \
        --source https://api.nuget.org/v3/index.json \
        --skip-duplicate
fi

echo ""
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}  ? Successfully published!${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""
echo "Package: DotNet.WpfToolKit v${VERSION}"
echo "URL: https://www.nuget.org/packages/DotNet.WpfToolKit/${VERSION}"
echo ""
echo -e "${YELLOW}Note: It may take 5-10 minutes for the package to appear in search.${NC}"
echo ""
