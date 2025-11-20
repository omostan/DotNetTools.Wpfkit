# Publishing DotNet.WpfToolkit to NuGet

## Prerequisites

1. **NuGet Account**:
   - Create an account at https://www.nuget.org/
   - Verify your email address

2. **API Key**:
   - Go to https://www.nuget.org/account/apikeys
   - Click "Create"
   - Give it a name (e.g., "DotNet.WpfToolkit")
   - Select scope: "Push new packages and package versions"
   - Set expiration (e.g., 365 days)
   - Click "Create"
   - **Copy the API key immediately** (you won't see it again!)

## Step-by-Step Publishing Process

### Step 1: Create Package Icon (Optional)

Creating a package icon is **optional** but **recommended** for better visibility on NuGet.org.

If you want to add an icon:

1. Create a 128x128 PNG file
2. Save it as `DotNet.WpfToolkit/icon.png`
3. Uncomment these lines in `DotNet.WpfToolkit.csproj`:

```xml
<!-- In PropertyGroup section -->
<PackageIcon>icon.png</PackageIcon>

<!-- In ItemGroup section -->
<None Include="icon.png" Pack="true" PackagePath="\" />
```

You can use any image editor or online tool:
- https://www.canva.com/ (create custom designs)
- https://www.photopea.com/ (free Photoshop alternative)
- Microsoft Paint (simple editing)
- https://placeholder.com/ (generate placeholder icons)

**Note:** You can publish without an icon - it will just use a default placeholder on NuGet.org.

### Step 2: Build the Package

Open Git Bash or PowerShell in your project directory:

```bash
# Navigate to the toolkit project
cd DotNet.WpfToolkit

# Clean previous builds
dotnet clean

# Build in Release mode
dotnet build -c Release

# Create the NuGet package
dotnet pack -c Release -o ./nupkg
```

This will create:
- `nupkg/DotNet.WpfToolKit.1.0.0.nupkg` - The main package
- `nupkg/DotNet.WpfToolKit.1.0.0.snupkg` - Symbol package (for debugging)

### Step 3: Test the Package Locally (Optional)

Before publishing, test it locally:

```bash
# Add a local package source
dotnet nuget add source D:\Tutorials\DotNet\DotNet.WpfToolkit\nupkg -n LocalPackages

# In a test project
dotnet add package DotNet.WpfToolKit --version 1.0.0 --source LocalPackages

# Remove local source after testing
dotnet nuget remove source LocalPackages
```

### Step 4: Publish to NuGet.org

```bash
# Set your API key (one-time setup)
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg --api-key YOUR_API_KEY_HERE --source https://api.nuget.org/v3/index.json

# If you want to push symbols too
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.snupkg --api-key YOUR_API_KEY_HERE --source https://api.nuget.org/v3/index.json
```

**Replace `YOUR_API_KEY_HERE` with your actual API key from Step 2.**

### Step 5: Verify Publication

1. Go to https://www.nuget.org/packages/DotNet.WpfToolKit
2. It may take 5-10 minutes to appear and be indexed
3. Initially shows as "Validating" or "Listed" status

## Alternative: Using nuget.exe

If you prefer using the standalone NuGet CLI:

### Download nuget.exe
```bash
# Download nuget.exe
curl -o nuget.exe https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
```

### Publish with nuget.exe
```bash
nuget.exe push nupkg/DotNet.WpfToolKit.1.0.0.nupkg YOUR_API_KEY_HERE -Source https://api.nuget.org/v3/index.json
```

## Complete Commands (All in One)

Run these commands from the project root `D:\Tutorials\DotNet`:

```bash
# 1. Navigate to toolkit project
cd DotNet.WpfToolkit

# 2. Clean and build
dotnet clean
dotnet build -c Release

# 3. Create package
dotnet pack -c Release -o ./nupkg

# 4. Publish to NuGet (replace YOUR_API_KEY_HERE)
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
  --api-key YOUR_API_KEY_HERE \
  --source https://api.nuget.org/v3/index.json \
  --skip-duplicate

# 5. Publish symbols (optional)
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.snupkg \
  --api-key YOUR_API_KEY_HERE \
  --source https://api.nuget.org/v3/index.json \
  --skip-duplicate
```

## After Publishing

### Installation by Users

Once published, users can install your package:

```bash
# .NET CLI
dotnet add package DotNet.WpfToolKit

# Package Manager Console (Visual Studio)
Install-Package DotNet.WpfToolKit

# PackageReference in .csproj
<PackageReference Include="DotNet.WpfToolKit" Version="1.0.0" />
```

### Updating Package Statistics

- View stats at: https://www.nuget.org/packages/DotNet.WpfToolKit/1.0.0
- Downloads appear after ~1 hour
- Search indexing takes 15-60 minutes

## Publishing Future Versions

### Step 1: Update Version Number

Edit `DotNet.WpfToolkit/DotNet.WpfToolkit.csproj`:

```xml
<Version>1.0.1</Version>
<AssemblyVersion>1.0.1.0</AssemblyVersion>
<FileVersion>1.0.1.0</FileVersion>
<PackageReleaseNotes>Bug fixes and improvements. See release notes at GitHub.</PackageReleaseNotes>
```

### Step 2: Create and Publish

```bash
dotnet clean
dotnet build -c Release
dotnet pack -c Release -o ./nupkg
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.1.nupkg --api-key YOUR_API_KEY_HERE --source https://api.nuget.org/v3/index.json
```

## Troubleshooting

### Error: 403 - API Key Invalid/Expired/No Permission

**Full Error**: `Response status code does not indicate success: 403 (The specified API key is invalid, has expired, or does not have permission to access the specified package.)`

#### Quick Fix:
1. Go to https://www.nuget.org/account/apikeys
2. Check if your key has expired (look at "Expires" column)
3. Verify key has "Push new packages and package versions" permission
4. If in doubt, click "Regenerate" or create a new key
5. For first-time publishing, ensure "Select Packages" is set to "All packages"

#### Detailed Solutions:
See **`NUGET_API_KEY_TROUBLESHOOTING.md`** for comprehensive troubleshooting steps.

**Common causes**:
- ? Key expired
- ? Key doesn't have push permission
- ? Key restricted to specific package pattern that doesn't match
- ? Key copied incorrectly (extra spaces)
- ? Wrong key used (copied from different account)

**Quick test**:
```bash
# Verify key is stored correctly
echo $NUGET_API_KEY    # Bash
echo $env:NUGET_API_KEY # PowerShell

# Create new key and try immediately
export NUGET_API_KEY="your-fresh-key-here"
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
  --api-key $NUGET_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

### Error: Package already exists
- You cannot overwrite a published version
- You must increment the version number
- Use `--skip-duplicate` flag if needed

### Error: Invalid API Key
- Ensure you copied the key correctly
- Check if the key has expired
- Verify the key has "Push" permissions

### Error: Package validation failed
- Check that all required metadata is present
- Ensure README.md exists
- Verify the package builds without errors

### Package not appearing in search
- Wait 15-60 minutes for indexing
- Check: https://www.nuget.org/packages/DotNet.WpfToolKit directly
- Clear NuGet cache: `dotnet nuget locals all --clear`

## Best Practices

1. **Always test locally** before publishing
2. **Use semantic versioning**: MAJOR.MINOR.PATCH
3. **Include release notes** in each version
4. **Tag releases** in Git to match package versions
5. **Never delete** packages from NuGet (unlist instead)
6. **Keep API keys secure** - don't commit them to Git
7. **Include XML documentation** for IntelliSense support
8. **Provide samples** and good documentation

## Security Notes

### Storing API Key Securely

**DON'T** commit API keys to source control!

#### Option 1: Environment Variable (Recommended)
```bash
# Set environment variable (PowerShell)
$env:NUGET_API_KEY = "your-api-key-here"

# Use in command
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg --api-key $env:NUGET_API_KEY --source https://api.nuget.org/v3/index.json
```

#### Option 2: NuGet Config (Local)
```bash
# Add API key to config (stored encrypted)
dotnet nuget update source nuget.org --username your-username --password YOUR_API_KEY

# Then push without specifying key
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg --source https://api.nuget.org/v3/index.json
```

## CI/CD Integration (GitHub Actions)

Create `.github/workflows/publish-nuget.yml`:

```yaml
name: Publish to NuGet

on:
  release:
    types: [published]

jobs:
  publish:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'
      
      - name: Restore dependencies
        run: dotnet restore
      
      - name: Build
        run: dotnet build -c Release --no-restore
      
      - name: Pack
        run: dotnet pack DotNet.WpfToolkit/DotNet.WpfToolkit.csproj -c Release -o ./nupkg
      
      - name: Push to NuGet
        run: dotnet nuget push nupkg/*.nupkg --api-key ${{secrets.NUGET_API_KEY}} --source https://api.nuget.org/v3/index.json --skip-duplicate
```

**Add your API key** to GitHub Secrets:
1. Go to: https://github.com/omostan/DotNet.WpfToolkit/settings/secrets/actions
2. Click "New repository secret"
3. Name: `NUGET_API_KEY`
4. Value: Your NuGet API key
5. Click "Add secret"

## Resources

- **NuGet Gallery**: https://www.nuget.org/
- **NuGet Documentation**: https://learn.microsoft.com/en-us/nuget/
- **Package Best Practices**: https://learn.microsoft.com/en-us/nuget/create-packages/package-authoring-best-practices
- **Semantic Versioning**: https://semver.org/

## Summary Checklist

- [ ] Create NuGet.org account
- [ ] Generate API key
- [ ] Add package metadata to .csproj
- [ ] Create package icon (optional)
- [ ] Build in Release mode
- [ ] Create NuGet package with `dotnet pack`
- [ ] Test package locally
- [ ] Push to NuGet.org
- [ ] Verify package appears on NuGet.org
- [ ] Create GitHub release (v1.0.0)
- [ ] Update documentation with installation instructions
- [ ] Announce release

---

**Ready to publish?** Follow the steps above and your package will be live on NuGet! ??
