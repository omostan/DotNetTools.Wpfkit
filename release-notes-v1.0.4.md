# Release Notes - v1.0.4 (CRITICAL HOTFIX)

**Release Date:** January 2025  
**Package:** DotNetTools.Wpfkit  
**Version:** 1.0.4  
**Type:** Critical Hotfix

## 🚨 Critical Bug Fix

### NullReferenceException in ActionCommand.CanExecute

**Severity:** CRITICAL  
**Impact:** Production applications crash when commands are bound to UI elements with null parameters

**Issue:** The `ActionCommand.CanExecute` method was throwing `NullReferenceException` when:
- Command parameter is `null`
- A predicate is provided that attempts to access properties on the parameter
- WPF's command infrastructure calls `CanExecute` during UI initialization
- DataGrid columns bind to commands during layout

**Stack Trace Location:**
```
at DotNetTools.Wpfkit.Commands.ActionCommand.CanExecute(Object parameter)
at System.Windows.Controls.Primitives.ButtonBase.UpdateCanExecute()
```

**Root Cause:**
The original implementation didn't properly handle nullable parameters and used the null-forgiving operator without adequate safety:

```csharp
// OLD v1.0.3 - BROKEN CODE
public class ActionCommand(Action<object> action, Predicate<object>? predicate = default)
{
    public bool CanExecute(object? parameter)
    {
        return _predicate == default || _predicate(parameter!);
    }
}
```

This passed `null` to predicates that expected non-null values, causing crashes in production.

### Resolution

**Fixed Implementation:**
```csharp
// NEW v1.0.4 - FIXED CODE
public class ActionCommand(Action<object?> action, Predicate<object?>? predicate = null)
{
    public bool CanExecute(object? parameter)
    {
        // If no predicate is provided, command can always execute
        if (_predicate == null)
        {
            return true;
        }

        // Safely evaluate the predicate with exception handling
        try
        {
            return _predicate(parameter);
        }
        catch
        {
            // Return false on any exception to prevent crashes
            return false;
        }
    }

    public void Execute(object? parameter)
    {
        _action(parameter);
    }
}
```

**Key Improvements:**
1. ✅ Changed parameter types from `Action<object>` to `Action<object?>` for proper null handling
2. ✅ Changed predicate from `Predicate<object>?` to `Predicate<object?>?` for consistency
3. ✅ Explicit null check for predicate (changed from `default` to `null`)
4. ✅ Try-catch block for graceful error handling
5. ✅ Returns `false` on any exception, preventing UI crashes
6. ✅ Maintains backward compatibility
7. ✅ Comprehensive XML documentation added

## 🔧 Technical Details

### Changed Files
1. **DotNetTools.WpfKit/Commands/ActionCommand.cs**
   - Changed `Action<object>` to `Action<object?>`
   - Changed `Predicate<object>?` to `Predicate<object?>?`
   - Improved null handling in `CanExecute`
   - Added comprehensive XML documentation

2. **DotNetTools.WpfKit/Commands/RelayCommand.cs**
   - Updated to match ActionCommand signature
   - Added XML documentation

3. **Test Files (NEW)**
   - ActionCommandTests.cs
   - RelayCommandTests.cs
   - AsyncRelayCommandTests.cs

### Affected Classes
- ✅ `ActionCommand` - Direct fix applied
- ✅ `RelayCommand` - Inherits fix from `ActionCommand`
- ✅ `AsyncCommandBase` - No changes needed (already safe)
- ✅ `AsyncRelayCommand` - No changes needed (already safe)

### Behavior Changes
**Before (v1.0.3):**
- ❌ Crashes when parameter is null and predicate accesses parameter properties
- ❌ No error recovery
- ❌ Application crashes during UI rendering
- ❌ DataGrid causes NullReferenceException

**After (v1.0.4):**
- ✅ Handles null parameters gracefully
- ✅ Returns `false` on any predicate exception
- ✅ UI remains responsive and stable
- ✅ No breaking changes to existing code
- ✅ Proper nullable reference type annotations

## 📊 Impact Assessment

### Who Should Update Immediately
- **CRITICAL:** All users running v1.0.3 in production
- **HIGH:** Applications using ActionCommand or RelayCommand with predicates
- **HIGH:** Applications with DataGrid or complex UI layouts
- **MEDIUM:** All WPF applications using the toolkit

### Symptoms You May Have Experienced
- Application crashes on startup
- NullReferenceException during UI initialization
- Crashes when binding commands to DataGrid columns
- Intermittent crashes during view loading
- Unhandled exceptions in command evaluation

## 🔄 Migration Guide

### Update Instructions

**1. Update NuGet Package:**
```sh
# Via .NET CLI
dotnet add package DotNetTools.Wpfkit --version 1.0.4

# Via Package Manager Console
Update-Package DotNetTools.Wpfkit -Version 1.0.4

# Or edit .csproj
<PackageReference Include="DotNetTools.Wpfkit" Version="1.0.4" />
```

**2. No Code Changes Required:**
This is a drop-in replacement. Your existing code will work without modifications.

**3. Test Your Application:**
```sh
dotnet clean
dotnet build
dotnet run
```

**4. Verify Commands Work:**
- Test commands with null parameters
- Test DataGrid commands
- Test predicates that check parameter properties

### Breaking Changes
**None** - This is a backward-compatible hotfix with improved type safety.

## 📝 Best Practices for Command Usage

### ✅ DO: Handle Null Parameters Safely
```csharp
// Good - Null-safe predicate
SaveCommand = new ActionCommand(
    param => Save(param),
    param => param != null && IsValid(param)
);
```

### ✅ DO: Use Pattern Matching
```csharp
// Good - Type-safe with pattern matching
DeleteCommand = new ActionCommand(
    param => Delete((int)param),
    param => param is int id && id > 0
);
```

### ✅ DO: Check for Null First
```csharp
// Good - Explicit null check first
SaveCommand = new ActionCommand(
    param => Save(param),
    param =>
    {
        if (param == null) return false;
        if (param is not string str) return false;
        return !string.IsNullOrWhiteSpace(str);
    }
);
```

### ❌ DON'T: Assume Parameters Are Non-Null
```csharp
// Bad - Assumes parameter is never null
SaveCommand = new ActionCommand(
    action: param => Save(param),
    predicate: param => param.ToString().Length > 0  // Will crash if param is null
);
```

### ❌ DON'T: Access Properties Without Checking
```csharp
// Bad - Direct property access
DeleteCommand = new ActionCommand(
    action: param => Delete(param),
    predicate: param => ((Entity)param).Id > 0  // Will crash if param is null or wrong type
);
```

## 📦 Package Information

| Property | Value |
|----------|-------|
| Package ID | DotNetTools.Wpfkit |
| Version | 1.0.4 |
| Previous Version | 1.0.3 (DEPRECATED) |
| Target Framework | .NET 10.0 |
| Release Type | Critical Hotfix |
| Test Coverage | >95% |

## 🔗 Links

- [GitHub Repository](https://github.com/omostan/DotNetTools.Wpfkit)
- [NuGet Package](https://www.nuget.org/packages/DotNetTools.Wpfkit/1.0.4)
- [Report Issues](https://github.com/omostan/DotNetTools.Wpfkit/issues)
- [Documentation](https://github.com/omostan/DotNetTools.Wpfkit/blob/main/DotNetTools.WpfKit/README.md)
- [Test Results](./TEST_RESULTS_v1.0.4.md)

## 📞 Support

If you experience issues with this hotfix:
1. Check that you're using version 1.0.4
2. Clean and rebuild your solution
3. Run included test scripts: `./run-tests-v1.0.4.sh` or `.\run-tests-v1.0.4.ps1`
4. Report issues at: https://github.com/omostan/DotNetTools.Wpfkit/issues

## 🙏 Acknowledgments

Thank you to the users who reported this critical issue and provided detailed reproduction steps. Your feedback was invaluable in identifying and fixing this bug!

## ⚠️ Deprecation Notice

**v1.0.3 is now DEPRECATED** due to this critical bug.  
**DO NOT USE v1.0.3 in production!**

All users must upgrade to v1.0.4 immediately.

---

**Copyright © 2025 Omotech Digital Solutions**  
**Author:** Stanley Omoregie  
**Release Type:** Critical Hotfix  
**Priority:** URGENT - Update Immediately  
**Test Coverage:** 150+ tests, >95% coverage
