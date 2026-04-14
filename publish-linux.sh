#!/bin/bash

set -e

APP_NAME="sadaje"
DIST_DIR="./dist"

echo "Cleaning old dist..."
rm -rf "$DIST_DIR"
mkdir -p "$DIST_DIR"

echo "Building Native AOT binaries..."

dotnet publish ./SadaJeTerminal.csproj -c Release -r linux-arm64 -o "$DIST_DIR/linux-arm64"
dotnet publish ./SadaJeTerminal.csproj -c Release -r linux-x64 -o "$DIST_DIR/linux-x64"

echo "Cleaning artifacts..."
rm -rf ./bin ./obj

echo "Done!"
echo "Output in: $DIST_DIR"
