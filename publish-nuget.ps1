# NuGet Package Publishing Script for DotNet.WpfToolkit
# Usage: .\publish-nuget.ps1 [-ApiKey "your-api-key"]

param(
    [string]$ApiKey = $env:NUGET_API_KEY
)

# Colors
$Green = "Green"
$Yellow = "Yellow"
$Red = "Red"

Write-Host "========================================" -ForegroundColor $Green
Write-Host "  DotNet.WpfToolkit NuGet Publisher" -ForegroundColor $Green
Write-Host "========================================" -ForegroundColor $Green
Write-Host ""

# Check if API key is provided
if ([string]::IsNullOrEmpty($ApiKey)) {
    Write-Host "Error: NuGet API key not provided!" -ForegroundColor $Red
    Write-Host ""
    Write-Host "Usage:"
    Write-Host "  .\publish-nuget.ps1 -ApiKey YOUR_API_KEY"
    Write-Host "  or"
    Write-Host "  `$env:NUGET_API_KEY = 'YOUR_API_KEY'; .\publish-nuget.ps1"
    exit 1
}

# Navigate to project directory
Write-Host "Navigating to project directory..." -ForegroundColor $Yellow
Set-Location DotNet.WpfToolkit

# Clean previous builds
Write-Host "Cleaning previous builds..." -ForegroundColor $Yellow
dotnet clean --configuration Release

# Build project
Write-Host "Building project in Release mode..." -ForegroundColor $Yellow
dotnet build --configuration Release

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor $Red
    exit 1
}

# Create NuGet package
Write-Host "Creating NuGet package..." -ForegroundColor $Yellow
dotnet pack --configuration Release --output ./nupkg

if ($LASTEXITCODE -ne 0) {
    Write-Host "Pack failed!" -ForegroundColor $Red
    exit 1
}

# Get package version from csproj
[xml]$csproj = Get-Content DotNet.WpfToolkit.csproj
$Version = $csproj.Project.PropertyGroup.Version
$PackageFile = "nupkg/DotNet.WpfToolKit.$Version.nupkg"
$SymbolsFile = "nupkg/DotNet.WpfToolKit.$Version.snupkg"

Write-Host "Package created: $PackageFile" -ForegroundColor $Green

# Verify package exists
if (-not (Test-Path $PackageFile)) {
    Write-Host "Error: Package file not found!" -ForegroundColor $Red
    exit 1
}

# Ask for confirmation
Write-Host ""
Write-Host "Ready to publish DotNet.WpfToolKit v$Version to NuGet.org" -ForegroundColor $Yellow
$Confirm = Read-Host "Continue? (y/n)"

if ($Confirm -ne "y" -and $Confirm -ne "Y") {
    Write-Host "Publishing cancelled." -ForegroundColor $Red
    exit 0
}

# Push main package
Write-Host "Publishing package to NuGet.org..." -ForegroundColor $Yellow
dotnet nuget push $PackageFile `
    --api-key $ApiKey `
    --source https://api.nuget.org/v3/index.json `
    --skip-duplicate

if ($LASTEXITCODE -ne 0) {
    Write-Host "Push failed!" -ForegroundColor $Red
    exit 1
}

# Push symbols package if exists
if (Test-Path $SymbolsFile) {
    Write-Host "Publishing symbols package..." -ForegroundColor $Yellow
    dotnet nuget push $SymbolsFile `
        --api-key $ApiKey `
        --source https://api.nuget.org/v3/index.json `
        --skip-duplicate
}

Write-Host ""
Write-Host "========================================" -ForegroundColor $Green
Write-Host "  ? Successfully published!" -ForegroundColor $Green
Write-Host "========================================" -ForegroundColor $Green
Write-Host ""
Write-Host "Package: DotNet.WpfToolKit v$Version"
Write-Host "URL: https://www.nuget.org/packages/DotNet.WpfToolKit/$Version"
Write-Host ""
Write-Host "Note: It may take 5-10 minutes for the package to appear in search." -ForegroundColor $Yellow
Write-Host ""
