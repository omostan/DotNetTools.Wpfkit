# ?? Quick Fix: 404 Error When Publishing Symbols Package

## Your Error
```
NotFound https://www.nuget.org/api/v2/symbolpackage/ 9129ms
error: Response status code does not indicate success: 404 
(A package with ID 'DotNet.WpfToolKit' and version '1.0.0' does not exist. 
Please upload the package before uploading its symbols.)
```

## What This Means

You're trying to push the **symbols package** (`.snupkg`) before the **main package** (`.nupkg`) has been successfully uploaded to NuGet.

## Solution: Push Main Package First

### Step 1: Push ONLY the Main Package

```bash
cd D:/Tutorials/DotNet/DotNet.WpfToolkit

# Push the main .nupkg file (NOT the .snupkg)
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
  --api-key $NUGET_API_KEY \
  --source https://api.nuget.org/v3/index.json \
  --skip-duplicate
```

**PowerShell:**
```powershell
cd D:\Tutorials\DotNet\DotNet.WpfToolkit

dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg `
  --api-key $env:NUGET_API_KEY `
  --source https://api.nuget.org/v3/index.json `
  --skip-duplicate
```

### Step 2: Wait for Success Message

You should see:
```
Pushing DotNet.WpfToolKit.1.0.0.nupkg to 'https://api.nuget.org/v3/index.json'...
  PUT https://api.nuget.org/v3/index.json
  Created https://api.nuget.org/v3/index.json 5678ms
Your package was pushed.
```

### Step 3: THEN Push Symbols (Optional)

**Only after Step 1 succeeds**, you can push symbols:

```bash
# Push symbols package
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.snupkg \
  --api-key $NUGET_API_KEY \
  --source https://api.nuget.org/v3/index.json \
  --skip-duplicate
```

**Note**: Pushing symbols is **completely optional**. Your package will work fine without it.

## Why This Happens

The symbols package (`.snupkg`) is a companion to the main package. NuGet requires:
1. ? Main package must exist first
2. ? Then symbols package can be uploaded

Think of it like this:
- Main package = The actual library users install
- Symbols package = Debug information for developers

You can't have debug symbols without the actual library!

## Recommended Publishing Order

```bash
# Step 1: Push main package
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
  --api-key $NUGET_API_KEY \
  --source https://api.nuget.org/v3/index.json

# Step 2: Wait for success (check for "Your package was pushed")

# Step 3: Optionally push symbols
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.snupkg \
  --api-key $NUGET_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

## Do You Need Symbols?

**Symbols package is optional!** Skip it if:
- ? You're just testing
- ? First time publishing
- ? Not sure what it's for
- ? Don't want debugging support

**Only push symbols if:**
- ? You want users to step into your code while debugging
- ? You want better stack traces in production
- ? Your package is used by many developers

## Complete Working Commands

### Git Bash (Main Package Only)
```bash
cd D:/Tutorials/DotNet/DotNet.WpfToolkit

# Ensure package exists
ls -la nupkg/*.nupkg

# Push main package
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
  --api-key $NUGET_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

### PowerShell (Main Package Only)
```powershell
cd D:\Tutorials\DotNet\DotNet.WpfToolkit

# Ensure package exists
dir nupkg\*.nupkg

# Push main package
dotnet nuget push nupkg\DotNet.WpfToolKit.1.0.0.nupkg `
  --api-key $env:NUGET_API_KEY `
  --source https://api.nuget.org/v3/index.json
```

## Troubleshooting

### Issue: Still getting 404
**Possible causes:**
1. Main package push failed (check previous error)
2. API key issues (see `QUICK_FIX_403_ERROR.md`)
3. Wrong package name or version

**Solution:**
```bash
# Check if package file exists
ls nupkg/

# Should show:
# DotNet.WpfToolKit.1.0.0.nupkg
# DotNet.WpfToolKit.1.0.0.snupkg

# Push ONLY .nupkg first
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
  --api-key $NUGET_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

### Issue: Package push succeeded but symbols still fails
**This is normal!** Wait a few minutes, then try:
```bash
# Wait 2-3 minutes after main package push
sleep 180  # Bash
Start-Sleep -Seconds 180  # PowerShell

# Then push symbols
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.snupkg \
  --api-key $NUGET_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

### Issue: Don't care about symbols
**Just skip it!** You don't need symbols for your package to work.

Only push the `.nupkg` file, ignore the `.snupkg` file.

## Updated Publishing Scripts

Your automation scripts should be updated to handle this properly:

### Updated publish-nuget.sh (Bash)
```bash
#!/bin/bash

# Push main package
echo "Publishing main package..."
dotnet nuget push "$PACKAGE_FILE" \
    --api-key "$NUGET_API_KEY" \
    --source https://api.nuget.org/v3/index.json \
    --skip-duplicate

# Check if main package push succeeded
if [ $? -eq 0 ]; then
    echo "Main package published successfully!"
    
    # Ask if user wants to push symbols
    echo "Do you want to publish symbols package? (y/n)"
    read -r PUSH_SYMBOLS
    
    if [ "$PUSH_SYMBOLS" = "y" ] && [ -f "$SYMBOLS_FILE" ]; then
        echo "Publishing symbols package..."
        sleep 5  # Wait a bit for main package to be processed
        dotnet nuget push "$SYMBOLS_FILE" \
            --api-key "$NUGET_API_KEY" \
            --source https://api.nuget.org/v3/index.json \
            --skip-duplicate
    fi
else
    echo "Main package push failed. Not pushing symbols."
    exit 1
fi
```

## Summary

**Quick Fix:**
1. Push **ONLY** the `.nupkg` file first
2. Wait for success message
3. Symbols (`.snupkg`) are optional - skip them if unsure
4. If you want symbols, push them AFTER main package succeeds

**Command to use right now:**
```bash
cd D:/Tutorials/DotNet/DotNet.WpfToolkit
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
  --api-key $NUGET_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

**That's it!** Forget about the symbols package for now. You can add it later in version 1.0.1 if needed.

---

**Related Guides:**
- `QUICK_FIX_403_ERROR.md` - If you get 403 errors
- `NUGET_API_KEY_TROUBLESHOOTING.md` - API key issues
- `NUGET_PUBLISHING_GUIDE.md` - Complete publishing guide
