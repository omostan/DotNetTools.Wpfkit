# Release Notes - v1.0.5 (Documentation Update)

**Release Date:** November 2025  
**Package:** DotNetTools.Wpfkit  
**Version:** 1.0.5  
**Type:** Documentation Update

---

## 📝 Documentation Update

### Overview

Version 1.0.5 is a **documentation-only release**. There are **no code changes** from v1.0.4. This release corrects a date error in the package documentation.

### What Changed

**Documentation:**
- ✅ Corrected release date in README.md from "January 2025" to "November 2025"
- ✅ Updated package metadata

**Code:**
- ❌ No changes - Functionally identical to v1.0.4

---

## 🔄 Migration Guide

### From v1.0.4 to v1.0.5

**Should you update?**
- **Optional** - There are no functional differences
- **Update if:** You want the most accurate documentation
- **Skip if:** You're satisfied with v1.0.4 functionality

**How to update:**
```bash
# Update package
dotnet add package DotNetTools.Wpfkit --version 1.0.5
```

**Code changes required:**
```
None - Drop-in replacement
```

### From v1.0.3 or earlier to v1.0.5

**Should you update?**
- **YES - CRITICAL** - v1.0.3 and earlier contain a critical NullReferenceException bug

**How to update:**
```bash
# Update package
dotnet add package DotNetTools.Wpfkit --version 1.0.5
```

**Benefits:**
- ✅ Fixes critical NullReferenceException in ActionCommand
- ✅ Adds proper null handling
- ✅ Includes 187 comprehensive unit tests
- ✅ No breaking changes

---

## 📋 Version History

| Version | Date | Type | Changes |
|---------|------|------|---------|
| 1.0.5 | Nov 2025 | Documentation | README date correction |
| 1.0.4 | Nov 2025 | Critical Hotfix | NullReferenceException fix |
| 1.0.3 | Nov 2025 | Bug Fix | RelayCommand accessibility |
| 1.0.2 | Nov 2025 | Feature | Command infrastructure |
| 1.0.1 | Nov 2025 | Patch | Minor fixes |
| 1.0.0 | Nov 2025 | Initial | First release |

---

## 🔍 What's the Same as v1.0.4?

Everything! This release includes:

### Critical Bug Fixes (from v1.0.4)
- ✅ NullReferenceException in ActionCommand.CanExecute
- ✅ Proper null handling for command parameters
- ✅ Try-catch block for graceful error recovery
- ✅ Constructor parameter validation in AsyncCommandBase
- ✅ Constructor parameter validation in AsyncRelayCommand

### Test Coverage (from v1.0.4)
- ✅ ActionCommandTests.cs (50+ tests)
- ✅ RelayCommandTests.cs (40+ tests)
- ✅ AsyncRelayCommandTests.cs (60+ tests)
- ✅ Total: 187 tests, >95% coverage

### Features
- ✅ ObservableObject with INotifyPropertyChanged
- ✅ BaseViewModel with common UI properties
- ✅ ObservableRangeCollection for bulk operations
- ✅ CommandBase abstract class
- ✅ ActionCommand with predicates
- ✅ RelayCommand for MVVM
- ✅ AsyncCommandBase with state management
- ✅ AsyncRelayCommand with exception handling
- ✅ Serilog integration
- ✅ LogManager with context-aware logging
- ✅ AppSettingsUpdater for configuration

---

## 📊 Package Information

**NuGet Package ID:** DotNetTools.Wpfkit  
**Version:** 1.0.5  
**Target Framework:** .NET 10.0  
**Dependencies:**
- Serilog (v4.3.0+)
- Tracetool.DotNet.Api (v14.0.0+)

**Installation:**
```bash
dotnet add package DotNetTools.Wpfkit --version 1.0.5
```

**Package URL:**
https://www.nuget.org/packages/DotNetTools.Wpfkit/1.0.5

**Repository:**
https://github.com/omostan/DotNetTools.Wpfkit

---

## ❓ FAQ

### Why v1.0.5 instead of v1.0.4.1?

Semantic versioning best practices:
- **Patch versions (x.x.X)** are for bug fixes
- Documentation corrections are considered patches
- v1.0.5 is clearer than v1.0.4.1

### Do I need to update from v1.0.4?

**No** - It's optional. v1.0.5 is identical functionally. Update only if you want the most accurate documentation.

### What if I'm on v1.0.3?

**Yes - Update immediately!** v1.0.3 contains a critical bug. Both v1.0.4 and v1.0.5 fix it.

### Are there breaking changes?

**No** - Zero breaking changes. It's a drop-in replacement.

### Will there be more updates?

This is strictly a documentation correction. Future versions (v1.1.0+) will include new features.

---

## 🐛 Known Issues

None. This release maintains the same stability as v1.0.4.

---

## 📞 Support

**Issues:** https://github.com/omostan/DotNetTools.Wpfkit/issues  
**Repository:** https://github.com/omostan/DotNetTools.Wpfkit  
**NuGet:** https://www.nuget.org/packages/DotNetTools.Wpfkit/  
**Contact:** stan@omotech.com

---

## 🙏 Acknowledgments

Thank you to all users who reported the documentation inconsistency and provided feedback.

---

**Release Status:** ✅ Available on NuGet  
**Recommendation:** Optional update for documentation accuracy  
**Previous Version:** v1.0.4 (functionally identical)  
**Next Expected Version:** v1.1.0 (new features)

---

**Prepared by:** Stanley Omoregie  
**Company:** Omotech Digital Solutions  
**Date:** November 2025  
**License:** MIT
