$APP_NAME = "sadaje"
$DIST_DIR = "dist"

Write-Host "Cleaning old dist..."
Remove-Item -Recurse -Force $DIST_DIR -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Path $DIST_DIR | Out-Null

Write-Host "Building Native AOT binaries..."

dotnet publish ./SadaJeTerminal.csproj -c Release -r win-x64 -o "$DIST_DIR/win-x64"
dotnet publish ./SadaJeTerminal.csproj -c Release -r win-arm64 -o "$DIST_DIR/win-arm64"

Write-Host "Cleaning artifacts..."
Remove-Item -Recurse -Force bin -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force obj -ErrorAction SilentlyContinue

Write-Host "Done!"
Write-Host "Output in: $DIST_DIR"