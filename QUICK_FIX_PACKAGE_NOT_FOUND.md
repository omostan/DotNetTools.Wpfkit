# ?? Fix: Package Not Found After Publishing

## Error Message
```
error: There are no versions available for the package 'DotNet.WpfToolKit'.
```

## Common Causes & Solutions

### 1. ?? Package Still Being Indexed (Most Common)

**Wait Time**: 15-60 minutes after publishing

**Check Status:**
1. Visit: https://www.nuget.org/packages/DotNet.WpfToolKit
2. If you see "This package has not been indexed yet", wait longer
3. If you see package details, it should be installable

**Solution:** Wait a bit longer, then try:
```sh
# Clear cache and retry
dotnet nuget locals all --clear
dotnet add package DotNet.WpfToolKit
```

### 2. ?? Case Sensitivity Issue

NuGet package names are **case-sensitive**!

**Check Exact Package Name:**
1. Go to: https://www.nuget.org/packages/DotNet.WpfToolKit
2. Look at the exact package ID shown on the page
3. Use that EXACT casing when installing

**Possible name variations:**
```sh
# Try these different casings:
dotnet add package DotNet.WpfToolKit    # Current attempt
dotnet add package DotNet.WpfToolkit    # Lowercase 'k'
dotnet add package dotnet.wpftoolkit    # All lowercase
```

### 3. ??? Clear NuGet Cache

Your local cache might be outdated:

```sh
# Clear all NuGet caches
dotnet nuget locals all --clear

# Specifically clear http-cache
dotnet nuget locals http-cache --clear

# Try installing again
dotnet add package DotNet.WpfToolKit
```

### 4. ? Verify Package Source

Ensure you're using the correct NuGet source:

```sh
# List configured sources
dotnet nuget list source

# Should show:
# Registered Sources:
#   1. nuget.org [Enabled]
#      https://api.nuget.org/v3/index.json

# If nuget.org is disabled, enable it:
dotnet nuget enable source nuget.org

# Or add it if missing:
dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org
```

### 5. ?? Search for Your Package

Test if NuGet can find your package:

```sh
# Search for your package
dotnet package search DotNet.WpfToolKit --exact-match

# Or search more broadly
dotnet package search WpfToolKit
```

### 6. ?? Check Package Status on NuGet.org

**Direct URL:** https://www.nuget.org/packages/DotNet.WpfToolKit

Look for:
- ? Package shows "Listed" status
- ? Version 1.0.0 appears
- ? No validation errors
- ? If showing "Unlisted", it won't appear in searches

### 7. ?? Try Different Installation Methods

#### Method 1: Specify Version Explicitly
```sh
dotnet add package DotNet.WpfToolKit --version 1.0.0
```

#### Method 2: Use Package Manager Console (Visual Studio)
```powershell
Install-Package DotNet.WpfToolKit -Version 1.0.0
```

#### Method 3: Edit .csproj Directly
Add this to your project file:
```xml
<ItemGroup>
  <PackageReference Include="DotNet.WpfToolKit" Version="1.0.0" />
</ItemGroup>
```

Then run:
```sh
dotnet restore
```

#### Method 4: Use NuGet.exe
```sh
# Download NuGet.exe
curl -o nuget.exe https://dist.nuget.org/win-x86-commandline/latest/nuget.exe

# Install package
nuget.exe install DotNet.WpfToolKit -Version 1.0.0
```

### 8. ?? Verify Package ID on NuGet.org

The actual package ID might be different from what you think:

1. Visit: https://www.nuget.org/profiles/YOUR_USERNAME
2. Find your package in the list
3. Click on it
4. Note the **exact Package ID** shown at the top

**Common mistakes:**
- `DotNet.WpfToolKit` vs `DotNet.WpfToolkit` (capital K vs lowercase k)
- Extra spaces or special characters
- Typos in the name

## Step-by-Step Troubleshooting

### Step 1: Verify Package Exists
```sh
# Open browser
start https://www.nuget.org/packages/DotNet.WpfToolKit

# Or use curl
curl -I https://www.nuget.org/packages/DotNet.WpfToolKit
```

**Expected:** Should see your package page, not 404

### Step 2: Check Exact Package Name
Look at the package page and note the exact casing of the package ID.

### Step 3: Clear Everything
```sh
# Close Visual Studio if open
# Clear all caches
dotnet nuget locals all --clear

# Remove any local package sources that might interfere
dotnet nuget list source
# Remove any test sources:
# dotnet nuget remove source LocalPackages
```

### Step 4: Try Fresh Install
```sh
# Create new test project
mkdir TestInstall
cd TestInstall
dotnet new console

# Try installing with exact casing from Step 2
dotnet add package DotNet.WpfToolKit --version 1.0.0 --source https://api.nuget.org/v3/index.json
```

### Step 5: Check Restore Logs
```sh
# Restore with detailed logging
dotnet restore --verbosity detailed
```

Look for errors related to package resolution.

## Quick Fix Commands

Run these in order:

```sh
# 1. Clear cache
dotnet nuget locals all --clear

# 2. Verify source
dotnet nuget list source

# 3. Enable nuget.org if needed
dotnet nuget enable source nuget.org

# 4. Try installing with explicit version and source
dotnet add package DotNet.WpfToolKit --version 1.0.0 --source https://api.nuget.org/v3/index.json

# 5. If still failing, restore with verbose output
dotnet restore --verbosity detailed
```

## If Package Name is Wrong

If you discover the package ID is actually different (e.g., `DotNet.WpfToolkit` with lowercase 'k'):

You **cannot rename** a published package. Your options:

1. **Unlist current version and republish with correct name** (requires version bump)
2. **Live with current name** (most common choice)
3. **Create new package with correct name** (not recommended)

### To Check Your Actual Package ID:

Check your `.csproj` file:
```xml
<PackageId>DotNet.WpfToolKit</PackageId>
```

This is the exact name that was published.

## Timing Guidelines

| Time After Publishing | What to Expect |
|----------------------|----------------|
| 0-5 minutes | Package appears on NuGet.org but not searchable |
| 5-15 minutes | Package searchable via direct URL |
| 15-30 minutes | Package appears in NuGet search |
| 30-60 minutes | Package fully indexed, cache updated |
| 1-2 hours | Statistics start showing |

## Still Not Working?

### Contact NuGet Support
If after 2 hours your package still can't be installed:

1. Check NuGet Status: https://status.nuget.org/
2. Contact Support: https://www.nuget.org/policies/Contact
3. Check GitHub: https://github.com/NuGet/Home/issues

### Package May Be Unlisted
Check if package is unlisted:
1. Go to: https://www.nuget.org/packages/DotNet.WpfToolKit/manage
2. Check "Listed" checkbox status
3. If unlisted, check "List in search results" box

## Working Installation Example

Once the package is ready, you should see:

```sh
$ dotnet add package DotNet.WpfToolKit
  Determining projects to restore...
  Writing C:\Users\...\obj\project.assets.json with tools.
  Restore succeeded.
```

## Summary

**Most likely cause:** Package still being indexed (wait 15-60 minutes)

**Quick fix:**
```sh
# Wait, then run:
dotnet nuget locals all --clear
dotnet add package DotNet.WpfToolKit --version 1.0.0
```

**Verify package exists:**
```
https://www.nuget.org/packages/DotNet.WpfToolKit
```

---

**Need more help?** Check the main troubleshooting guide: `NUGET_PUBLISHING_GUIDE.md`
