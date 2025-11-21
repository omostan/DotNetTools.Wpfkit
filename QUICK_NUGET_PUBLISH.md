# Quick NuGet Publishing Commands

## One-Time Setup

1. **Get NuGet API Key**:
   - Visit: https://www.nuget.org/account/apikeys
   - Click "Create" and copy the key

2. **Store API Key Securely** (PowerShell):
```powershell
$env:NUGET_API_KEY = "your-api-key-here"
```

Or (Git Bash):
```bash
export NUGET_API_KEY="your-api-key-here"
```

## Publish Commands (Run from D:\Tutorials\DotNet)

```bash
# Navigate to project
cd DotNet.WpfToolkit

# Clean and build
dotnet clean
dotnet build -c Release

# Create package
dotnet pack -c Release -o ./nupkg

# View what's in the package (optional)
dotnet nuget verify nupkg/DotNet.WpfToolKit.1.0.0.nupkg

# Publish MAIN package to NuGet (required)
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
  --api-key $NUGET_API_KEY \
  --source https://api.nuget.org/v3/index.json

# Publish symbols (optional - only after main package succeeds)
# Uncomment if you want debugging support:
# dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.snupkg \
#   --api-key $NUGET_API_KEY \
#   --source https://api.nuget.org/v3/index.json
```

**?? Important:** 
- Push `.nupkg` (main package) FIRST
- Symbols (`.snupkg`) are optional and come SECOND
- Wait for "Your package was pushed" before pushing symbols

## All-in-One Command

```bash
cd DotNet.WpfToolkit && \
dotnet clean && \
dotnet build -c Release && \
dotnet pack -c Release -o ./nupkg && \
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
  --api-key $NUGET_API_KEY \
  --source https://api.nuget.org/v3/index.json

# Symbols are optional - only push if you need debugging support:
# && dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.snupkg \
#   --api-key $NUGET_API_KEY \
#   --source https://api.nuget.org/v3/index.json
```

## Verify Package

After publishing, check:
- https://www.nuget.org/packages/DotNet.WpfToolKit
- Wait 5-10 minutes for indexing

## For Next Releases

1. Update version in `DotNet.WpfToolkit.csproj`:
   - Change `<Version>1.0.0</Version>` to `<Version>1.0.1</Version>`
   - Update `<AssemblyVersion>` and `<FileVersion>`
   - Update `<PackageReleaseNotes>`

2. Run the publish commands above

## Test Installation

```bash
# In any test project
dotnet add package DotNet.WpfToolKit

# Or specific version
dotnet add package DotNet.WpfToolKit --version 1.0.0
