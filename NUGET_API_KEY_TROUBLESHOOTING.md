# ?? NuGet API Key Troubleshooting Guide

## Error: 403 - API Key Invalid

You're seeing this error:
```
Response status code does not indicate success: 403 
(The specified API key is invalid, has expired, or does not have 
permission to access the specified package.)
```

## Common Causes and Solutions

### 1. ? Verify Your API Key is Correct

#### Problem: Key was copied incorrectly
- Extra spaces at beginning/end
- Missing characters
- Wrong key copied

#### Solution:
1. Go to https://www.nuget.org/account/apikeys
2. Find your key (it will show as `***************`)
3. Click "Regenerate" to get a new key
4. Copy it **immediately** and carefully
5. Store it in a text file temporarily to verify

```bash
# Test: Echo your key to verify it's correct (BE CAREFUL - only do this locally!)
echo $NUGET_API_KEY
```

### 2. ? Check if API Key Has Expired

#### Problem: Keys expire after set duration

#### Solution:
1. Go to https://www.nuget.org/account/apikeys
2. Check the "Expires" column
3. If expired, click "Regenerate"
4. Update your stored key

### 3. ?? Verify API Key Permissions

#### Problem: Key doesn't have push permissions

#### Solution:
1. Go to https://www.nuget.org/account/apikeys
2. Check your key's **Scopes**
3. Should include: "Push new packages and package versions"
4. If not, create a new key with correct permissions:
   - **Name**: `DotNet.WpfToolkit`
   - **Select Packages**: All packages or specific pattern
   - **Glob Pattern**: `DotNet.WpfToolKit*`
   - **Scopes**: Check "Push new packages and package versions"
   - **Expiration**: 365 days (or your preference)

### 4. ?? Package Name Mismatch

#### Problem: API key is restricted to certain package patterns

#### Solution:
Check if your API key has a glob pattern restriction:

1. Go to https://www.nuget.org/account/apikeys
2. Look at "Packages" column
3. If it shows a pattern, make sure `DotNet.WpfToolKit` matches it

Common patterns:
- `*` - All packages (unrestricted)
- `DotNet.*` - All packages starting with "DotNet."
- `DotNet.WpfToolKit*` - Only this package and variations

### 5. ?? First Time Publishing This Package

#### Problem: API key created BEFORE package exists

Some API key configurations require the package to exist first.

#### Solution A: Use unrestricted key for first publish
1. Create a new API key
2. Set **Select Packages** to "All packages"
3. Use this for first publish
4. After successful publish, create a restricted key

#### Solution B: Use correct glob pattern
```
DotNet.WpfToolKit*
```
This matches: DotNet.WpfToolKit, DotNet.WpfToolKit.Core, etc.

### 6. ?? Network/Proxy Issues

#### Problem: Corporate firewall or proxy blocking request

#### Solution:
```bash
# Test connectivity to NuGet.org
curl -I https://api.nuget.org/v3/index.json

# If behind proxy, configure:
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
  --api-key YOUR_API_KEY \
  --source https://api.nuget.org/v3/index.json \
  --no-http-cache
```

## Step-by-Step Fix

### Method 1: Create Fresh API Key (Recommended)

1. **Delete old key** (optional but clean):
   - Go to https://www.nuget.org/account/apikeys
   - Click trash icon next to old key

2. **Create new key**:
   - Click "Create"
   - Fill in details:
     ```
     Key Name: DotNet.WpfToolkit-Publish
     Expiration: 365 days
     Select Packages: All packages
     Scopes: ? Push new packages and package versions
     ```
   - Click "Create"
   - **COPY THE KEY IMMEDIATELY**

3. **Store the key securely**:
   ```bash
   # Git Bash
   export NUGET_API_KEY="your-new-key-here"
   
   # PowerShell
   $env:NUGET_API_KEY = "your-new-key-here"
   ```

4. **Test the key**:
   ```bash
   # Try publishing again
   cd D:/Tutorials/DotNet/DotNet.WpfToolkit
   dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
     --api-key $NUGET_API_KEY \
     --source https://api.nuget.org/v3/index.json \
     --skip-duplicate
   ```

### Method 2: Verify Current Key

```bash
# Check if key is stored in environment
echo $NUGET_API_KEY    # Bash
echo $env:NUGET_API_KEY # PowerShell

# If empty or looks wrong, set it again
export NUGET_API_KEY="paste-your-key-here"  # Bash
$env:NUGET_API_KEY = "paste-your-key-here"  # PowerShell
```

### Method 3: Use Key Directly in Command

Instead of using environment variable, paste key directly:

```bash
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
  --api-key "oy2abc...xyz123" \
  --source https://api.nuget.org/v3/index.json \
  --skip-duplicate
```

**?? Warning**: Only do this in a private terminal, not in scripts committed to Git!

## Verification Checklist

Before trying again, verify:

- [ ] You're logged into NuGet.org with the correct account
- [ ] API key exists and hasn't expired
- [ ] API key has "Push" permission
- [ ] Package name matches any glob pattern restrictions
- [ ] Key is copied correctly (no extra spaces)
- [ ] Using the correct source URL: `https://api.nuget.org/v3/index.json`
- [ ] Package file exists: `nupkg/DotNet.WpfToolKit.1.0.0.nupkg`
- [ ] You have internet connectivity

## Testing Your API Key

Create a simple test to verify your key works:

```bash
# Method 1: List your packages (requires key with "Unlist" permission)
dotnet nuget list source

# Method 2: Try pushing with verbose output
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
  --api-key YOUR_KEY \
  --source https://api.nuget.org/v3/index.json \
  --skip-duplicate \
  --verbosity detailed
```

## Alternative: Use NuGet.org Web Upload

If API key issues persist, you can upload manually:

1. Go to https://www.nuget.org/packages/manage/upload
2. Click "Browse" and select your `.nupkg` file
3. Upload the file
4. Verify and publish

## Common Mistakes to Avoid

? **DON'T**:
- Use expired keys
- Use keys with wrong permissions
- Copy keys with extra whitespace
- Commit keys to Git
- Share keys publicly

? **DO**:
- Create fresh keys with full permissions
- Store keys in environment variables
- Test keys immediately after creation
- Regenerate keys if unsure
- Use unique key names for different projects

## Still Having Issues?

### Check NuGet Status
- Visit: https://status.nuget.org/
- Verify NuGet.org services are operational

### Review NuGet Account
- Visit: https://www.nuget.org/account
- Verify email is confirmed
- Check if any restrictions on account

### Contact Support
- If all else fails: https://www.nuget.org/policies/Contact
- Provide error message and steps taken

## Success Criteria

You'll know it's working when you see:

```
Pushing DotNet.WpfToolKit.1.0.0.nupkg to 'https://api.nuget.org/v3/index.json'...
  PUT https://api.nuget.org/v3/index.json
  Created https://api.nuget.org/v3/index.json 5678ms
Your package was pushed.
```

## Quick Fix Summary

**Most Common Solution**:
1. Go to https://www.nuget.org/account/apikeys
2. Click "Create"
3. Set "All packages" and "Push" permission
4. Copy the new key
5. Try again immediately:
   ```bash
   export NUGET_API_KEY="new-key-here"
   dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
     --api-key $NUGET_API_KEY \
     --source https://api.nuget.org/v3/index.json
   ```

---

**Need more help?** Check the main guide: `NUGET_PUBLISHING_GUIDE.md`
