# 🚀 DotNetTools.Wpfkit v1.0.2 - Command Infrastructure Release

## 📋 Overview
Major feature release of **DotNetTools.Wpfkit** introducing a comprehensive command infrastructure for MVVM applications.

**Release Date**: November 24, 2025  
**Version**: 1.0.2

## ✨ New Features

### Command Infrastructure

A complete set of command implementations for both synchronous and asynchronous operations in WPF MVVM applications.

#### 🔹 CommandBase
Abstract base class implementing `ICommand` interface:
- ✅ Provides foundation for custom command implementations
- ✅ Implements `CanExecuteChanged` event handling
- ✅ Virtual `CanExecute` method with default implementation
- ✅ Protected `OnCanExecuteChanged()` for triggering command re-evaluation

**Usage:**
```csharp
public class MyCustomCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        // Custom command logic
    }
    
    public override bool CanExecute(object? parameter)
    {
        return base.CanExecute(parameter) && /* custom logic */;
    }
}
```

#### 🔹 ActionCommand
Flexible command accepting action delegates with optional predicate validation:
- ✅ Parameter validation through predicate functions
- ✅ Automatic UI updates via `CommandManager.RequerySuggested`
- ✅ Null-checking with `ArgumentNullException` for safety
- ✅ Perfect for simple synchronous operations

**Usage:**
```csharp
public ICommand SaveCommand => new ActionCommand(
    action: param => Save(param),
    predicate: param => param != null && CanSave()
);
```

#### 🔹 RelayCommand
Internal implementation extending `ActionCommand`:
- ✅ Provides consistent command pattern
- ✅ Simplified command creation
- ✅ Same API as ActionCommand

#### 🔹 AsyncCommandBase
Abstract base for asynchronous command operations:
- ✅ **Automatic execution state management** (`IsExecuting`)
- ✅ **Prevents concurrent execution** - no double-clicking issues
- ✅ **Built-in exception handling** with TraceTool logging
- ✅ **Custom exception callbacks** for UI error handling
- ✅ Automatically updates `CanExecute` during operations

**Usage:**
```csharp
public class LoadDataCommand : AsyncCommandBase
{
    public LoadDataCommand(Action<Exception> onException) 
        : base(onException) { }
    
    protected override async Task ExecuteAsync(object parameter)
    {
        // Long-running async operation
        await LoadDataFromApiAsync();
    }
}
```

#### 🔹 AsyncRelayCommand
Concrete async command implementation:
- ✅ Ready-to-use async command
- ✅ Exception handling with custom callbacks
- ✅ No manual state management required
- ✅ Integrates seamlessly with `BaseViewModel.IsBusy`

**Usage:**
```csharp
public ICommand LoadDataCommand => new AsyncRelayCommand(
    callback: async () => await LoadDataAsync(),
    onException: ex => ShowError(ex.Message)
);
```

## 🎯 Key Benefits

### For Developers
- **Less Boilerplate**: No need to manually implement `ICommand` repeatedly
- **Type Safety**: Strongly-typed parameter support
- **Async/Await**: First-class async support with proper exception handling
- **State Management**: Automatic execution state tracking

### For Applications
- **Better UX**: Prevents double-execution and provides loading states
- **Error Handling**: Built-in exception handling with logging
- **Performance**: Efficient command execution without UI blocking
- **Maintainability**: Clean, reusable command patterns

## 📊 Command Comparison Matrix

| Feature | CommandBase | ActionCommand | AsyncCommandBase | AsyncRelayCommand |
|---------|-------------|---------------|------------------|-------------------|
| **Type** | Abstract | Concrete | Abstract | Concrete |
| **Execution** | Sync | Sync | Async | Async |
| **Parameter Support** | ✅ | ✅ | ✅ | ❌ |
| **CanExecute Predicate** | ✅ | ✅ | ✅ | ❌ |
| **Exception Handling** | Manual | Manual | ✅ Built-in | ✅ Built-in |
| **Execution State** | Manual | Manual | ✅ Automatic | ✅ Automatic |
| **Concurrent Prevention** | ❌ | ❌ | ✅ | ✅ |
| **Logging** | Manual | Manual | ✅ TraceTool | ✅ TraceTool |
| **Best For** | Custom commands | Simple sync ops | Custom async | Quick async ops |

## 💡 Complete Example

```csharp
using DotNetTools.Wpfkit.MvvM;
using DotNetTools.Wpfkit.Commands;
using System.Windows.Input;

public class CustomerViewModel : BaseViewModel
{
    private readonly ICustomerService _customerService;
    
    public ObservableRangeCollection<Customer> Customers { get; }
    
    // Synchronous command with parameter validation
    public ICommand SearchCommand { get; }
    
    // Asynchronous command with loading state
    public ICommand LoadCustomersCommand { get; }
    
    // Asynchronous command with complex operation
    public ICommand SaveCommand { get; }
    
    public CustomerViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
        Customers = new ObservableRangeCollection<Customer>();
        
        // Simple sync command
        SearchCommand = new ActionCommand(
            action: param => SearchCustomers((string)param),
            predicate: param => param is string text && !string.IsNullOrWhiteSpace(text)
        );
        
        // Async command with automatic state management
        LoadCustomersCommand = new AsyncRelayCommand(
            callback: LoadCustomersAsync,
            onException: HandleError
        );
        
        // Complex async command
        SaveCommand = new AsyncRelayCommand(
            callback: SaveCustomersAsync,
            onException: HandleError
        );
    }
    
    private void SearchCustomers(string searchText)
    {
        var filtered = _customerService.Search(searchText);
        Customers.ReplaceRange(filtered);
    }
    
    private async Task LoadCustomersAsync()
    {
        IsBusy = true; // Show loading indicator
        Title = "Loading...";
        
        try
        {
            var customers = await _customerService.GetAllAsync();
            Customers.ReplaceRange(customers);
            Title = $"Customers ({Customers.Count})";
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    private async Task SaveCustomersAsync()
    {
        IsBusy = true;
        try
        {
            await _customerService.SaveAllAsync(Customers);
            await ShowSuccessMessageAsync("Customers saved successfully");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    private void HandleError(Exception ex)
    {
        IsBusy = false;
        ShowErrorMessage($"Error: {ex.Message}");
    }
}
```

**XAML Binding:**
```xml
<StackPanel>
    <!-- Search with parameter binding -->
    <TextBox x:Name="SearchBox" />
    <Button Content="Search" 
            Command="{Binding SearchCommand}" 
            CommandParameter="{Binding Text, ElementName=SearchBox}"/>
    
    <!-- Async commands with automatic disabling during execution -->
    <Button Content="Load Customers" 
            Command="{Binding LoadCustomersCommand}"/>
    <Button Content="Save Changes" 
            Command="{Binding SaveCommand}"/>
    
    <!-- Loading indicator bound to IsBusy -->
    <ProgressBar IsIndeterminate="True" 
                 Visibility="{Binding IsBusy, Converter={StaticResource BoolToVisibilityConverter}}"/>
</StackPanel>
```

## 📚 Documentation Updates

- ✅ Comprehensive API documentation in `README.md`
- ✅ Added command examples to `QUICK_START.md`
- ✅ Command comparison matrix
- ✅ Best practices guide
- ✅ XAML binding examples
- ✅ Complete working examples with async/await patterns

## 🏗️ Architecture

### File Structure
```
DotNetTools.Wpfkit/Commands/
├── CommandBase.cs          # Abstract base implementing ICommand
├── ActionCommand.cs        # Parameterized synchronous command
├── RelayCommand.cs         # Internal relay implementation
├── AsyncCommandBase.cs     # Abstract async command with state management
└── AsyncRelayCommand.cs    # Concrete async command
```

### Design Patterns
- **Command Pattern**: Classic GoF implementation
- **Template Method**: AsyncCommandBase defines async execution template
- **Strategy Pattern**: Different command strategies for sync/async operations

## 📦 Installation

### Using .NET CLI
```bash
dotnet add package DotNetTools.Wpfkit --version 1.0.2
```

### Using Package Manager Console
```powershell
Install-Package DotNetTools.Wpfkit -Version 1.0.2
```

### Package Reference
```xml
<PackageReference Include="DotNetTools.Wpfkit" Version="1.0.2" />
```

## ⚙️ Requirements

- **.NET 10.0** or later
- **Windows OS** (for WPF support)
- **Serilog 4.3.0** or later (for logging features)
- **TraceTool.DotNet.Api 14.0.0** or later (for async command logging)

## 🔄 Migration Guide

### Upgrading from v1.0.1

**No breaking changes!** The command infrastructure is entirely additive.

**Recommended Changes:**

Replace manual `ICommand` implementations:
```csharp
// OLD - Manual implementation
private ICommand _saveCommand;
public ICommand SaveCommand => _saveCommand ??= new DelegateCommand(
    execute: () => Save(),
    canExecute: () => CanSave()
);

// NEW - Using ActionCommand
public ICommand SaveCommand => new ActionCommand(
    action: param => Save(),
    predicate: param => CanSave()
);
```

Replace manual async command implementations:
```csharp
// OLD - Manual async with state management
private bool _isLoading;
public ICommand LoadCommand => new DelegateCommand(async () =>
{
    if (_isLoading) return;
    _isLoading = true;
    try
    {
        await LoadDataAsync();
    }
    catch (Exception ex)
    {
        HandleError(ex);
    }
    finally
    {
        _isLoading = false;
    }
});

// NEW - Using AsyncRelayCommand
public ICommand LoadCommand => new AsyncRelayCommand(
    callback: LoadDataAsync,
    onException: HandleError
);
```

## 🐛 Bug Fixes

None - this is a feature-only release.

## ⚠️ Breaking Changes

None - fully backward compatible with v1.0.1 and v1.0.0.

## 🎯 Future Enhancements

Planned for future releases:
- Generic `ActionCommand<T>` with strongly-typed parameters
- `AsyncRelayCommand<T>` with parameter support
- `CancelableAsyncCommand` with cancellation token support
- Command pipeline/chaining support
- Undo/Redo command history

## 📖 Additional Resources

### Documentation
- [Complete API Reference](./DotNetTools.WpfKit/README.md)
- [Quick Start Guide](./QUICK_START.md)
- [Command Best Practices](#command-best-practices)

### Learning Resources
- [WPF Commands Overview](https://docs.microsoft.com/dotnet/desktop/wpf/advanced/commanding-overview)
- [MVVM Pattern Guide](https://docs.microsoft.com/archive/msdn-magazine/2009/february/patterns-wpf-apps-with-the-model-view-viewmodel-design-pattern)
- [Async/Await Best Practices](https://docs.microsoft.com/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)

## 👥 Contributors

- **Stanley Omoregie** - Initial implementation and documentation
- **Omotech Digital Solutions** - Project sponsor

## 📜 License

This project is licensed under the MIT License.

## 🆘 Support

- **Issues**: https://github.com/omostan/DotNetTools.Wpfkit/issues
- **Email**: stan@omotech.com
- **Documentation**: https://github.com/omostan/DotNetTools.Wpfkit

---

## 🎉 Thank You!

Thank you for using **DotNetTools.Wpfkit**! We hope the new command infrastructure makes your MVVM development faster and more enjoyable.

**Questions? Feedback?** We'd love to hear from you!

---

**Full Changelog**: https://github.com/omostan/DotNetTools.Wpfkit/compare/v1.0.1...v1.0.2

**Download**: https://www.nuget.org/packages/DotNetTools.Wpfkit/1.0.2

---

*Built with ❤️ by Omotech Digital Solutions*  
*November 24, 2025*
