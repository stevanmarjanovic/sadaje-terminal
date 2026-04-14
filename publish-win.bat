@echo off
setlocal enabledelayedexpansion

set APP_NAME=sadaje
set DIST_DIR=dist

echo Cleaning old dist...
if exist %DIST_DIR% rmdir /s /q %DIST_DIR%
mkdir %DIST_DIR%

echo Building Native AOT binaries...

dotnet publish .\SadaJeTerminal.csproj -c Release -r win-x64 -o %DIST_DIR%\win-x64
dotnet publish .\SadaJeTerminal.csproj -c Release -r win-arm64 -o %DIST_DIR%\win-arm64

echo Cleaning artifacts...
if exist bin rmdir /s /q bin
if exist obj rmdir /s /q obj

echo Done!
echo Output in: %DIST_DIR%

endlocal