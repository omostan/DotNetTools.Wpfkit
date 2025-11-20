# ?? NuGet Publishing Checklist

## Before Publishing

- [ ] All code changes committed to Git
- [ ] All tests passing (`dotnet test`)
- [ ] Build successful in Release mode (`dotnet build -c Release`)
- [ ] README.md updated with current version info
- [ ] Version number updated in `.csproj` file
- [ ] Release notes written (optional: `release-notes.md`)
- [ ] Package icon created (optional: `icon.png` 128x128)
- [ ] License file exists (if using PackageLicenseFile instead of Expression)

## NuGet Account Setup (One-Time)

- [ ] Created account at https://www.nuget.org/
- [ ] Email verified
- [ ] Generated API key at https://www.nuget.org/account/apikeys
- [ ] API key saved securely (NOT in source control!)

## Publishing Process

### Option 1: Using Automated Script (Recommended)

#### Git Bash:
```bash
chmod +x publish-nuget.sh
./publish-nuget.sh YOUR_API_KEY
```

#### PowerShell:
```powershell
.\publish-nuget.ps1 -ApiKey "YOUR_API_KEY"
```

### Option 2: Manual Commands

```bash
# 1. Navigate to project
cd DotNet.WpfToolkit

# 2. Clean
dotnet clean

# 3. Build
dotnet build -c Release

# 4. Pack
dotnet pack -c Release -o ./nupkg

# 5. Publish
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.0.nupkg \
  --api-key YOUR_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

## After Publishing

- [ ] Wait 5-10 minutes for package to process
- [ ] Verify package appears at https://www.nuget.org/packages/DotNet.WpfToolKit
- [ ] Test installation: `dotnet add package DotNet.WpfToolKit`
- [ ] Check package metadata displays correctly
- [ ] README renders correctly on NuGet page
- [ ] Create GitHub release (v1.0.0) matching NuGet version
- [ ] Update documentation with NuGet installation instructions
- [ ] Announce release (Twitter, blog, etc.)

## Version Management

### Current Version: 1.0.0

### For Next Release:

1. **Update version in `DotNet.WpfToolkit.csproj`:**
   ```xml
   <Version>1.0.1</Version>
   <AssemblyVersion>1.0.1.0</AssemblyVersion>
   <FileVersion>1.0.1.0</FileVersion>
   ```

2. **Update release notes:**
   ```xml
   <PackageReleaseNotes>Bug fixes and improvements...</PackageReleaseNotes>
   ```

3. **Create Git tag:**
   ```bash
   git tag -a v1.0.1 -m "Release v1.0.1"
   git push origin v1.0.1
   ```

4. **Publish to NuGet** (follow publishing process above)

## Semantic Versioning Guide

- **MAJOR** (1.x.x): Breaking changes
- **MINOR** (x.1.x): New features, backward compatible
- **PATCH** (x.x.1): Bug fixes, backward compatible

Examples:
- 1.0.0 ? 1.0.1: Bug fix
- 1.0.1 ? 1.1.0: New feature added
- 1.1.0 ? 2.0.0: Breaking change

## Troubleshooting

### Package doesn't appear in search
- ?? Wait 15-60 minutes for search indexing
- ? Check direct URL: https://www.nuget.org/packages/DotNet.WpfToolKit
- ?? Clear NuGet cache: `dotnet nuget locals all --clear`

### "Package already exists" error
- ?? Cannot overwrite published versions
- ? Must increment version number
- ?? Update Version in .csproj

### API Key invalid
- ?? Verify key hasn't expired
- ? Check key has "Push" permissions
- ?? Generate new key if needed

### Build/Pack failures
- ?? Run `dotnet clean`
- ?? Check for compilation errors
- ?? Ensure all dependencies are restored

## Package Maintenance

### Regular Tasks

- [ ] Monitor download statistics
- [ ] Respond to issues on GitHub
- [ ] Keep dependencies updated
- [ ] Address security vulnerabilities
- [ ] Maintain backward compatibility
- [ ] Update documentation

### When to Publish Update

- ? Critical bug fix
- ? Security vulnerability fix
- ? New features ready
- ? Dependency updates
- ? Performance improvements

### Before Publishing Update

- [ ] All tests pass
- [ ] Breaking changes documented
- [ ] Migration guide written (if needed)
- [ ] Changelog updated
- [ ] Version number incremented appropriately

## Security Checklist

- [ ] API key NOT in source control
- [ ] API key NOT in scripts committed to Git
- [ ] Use environment variables for API key
- [ ] API key has minimal required permissions
- [ ] API key has expiration date set
- [ ] Dependencies scanned for vulnerabilities
- [ ] No sensitive data in package

## Success Metrics

### After 24 Hours:
- [ ] Package appears in NuGet search
- [ ] Can install without errors
- [ ] README displays correctly
- [ ] License displays correctly
- [ ] All metadata correct

### After 1 Week:
- [ ] Monitor download count
- [ ] Check for issues/feedback
- [ ] Verify usage statistics
- [ ] Review package ranking

## Resources

- ?? Full Guide: `NUGET_PUBLISHING_GUIDE.md`
- ? Quick Reference: `QUICK_NUGET_PUBLISH.md`
- ?? Summary: `NUGET_PUBLISHING_SUMMARY.md`
- ?? NuGet.org: https://www.nuget.org/
- ?? Documentation: https://docs.microsoft.com/nuget/

## Notes

- Keep this checklist updated with each release
- Add lessons learned from each publishing experience
- Document any issues encountered and solutions
- Share knowledge with team members

---

**Last Updated**: Ready for v1.0.0 initial release
**Next Version**: 1.0.1 (when ready)

? **Ready to publish when all checklist items are complete!**
