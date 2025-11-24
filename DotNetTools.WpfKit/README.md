# DotNetTools.Wpfkit

[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/download)
[![NuGet](https://img.shields.io/nuget/v/DotNetTools.Wpfkit.svg)](https://www.nuget.org/packages/DotNetTools.Wpfkit/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/DotNetTools.Wpfkit.svg)](https://www.nuget.org/packages/DotNetTools.Wpfkit/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![GitHub Release](https://img.shields.io/github/v/release/omostan/DotNetTools.Wpfkit)](https://github.com/omostan/DotNetTools.Wpfkit/releases)

A comprehensive WPF toolkit library that provides essential components for building modern Windows desktop applications with the MVVM pattern, command infrastructure, logging capabilities, and configuration management.

## 📑 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Installation](#installation)
- [Requirements](#requirements)
- [Usage](#usage)
  - [MVVM Components](#mvvm-components)
  - [Command Infrastructure](#command-infrastructure)
  - [Logging](#logging)
  - [Configuration Management](#configuration-management)
- [API Reference](#api-reference)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## 📋 Overview

**DotNetTools.Wpfkit** is a modern .NET library designed to accelerate WPF application development by providing reusable, production-ready components. Built on .NET 10.0, it embraces modern C# features including nullable reference types, implicit usings, and follows best practices for WPF development.

## ✨ Features

### MVVM Pattern Support
- **ObservableObject**: Base class implementing `INotifyPropertyChanged` with helper methods
- **BaseViewModel**: Feature-rich view model base class with common UI properties
- **ObservableRangeCollection<T>**: Enhanced observable collection supporting bulk operations

### Command Infrastructure
- **CommandBase**: Abstract base class implementing `ICommand` interface
- **RelayCommand**: Synchronous command with action and predicate support
- **ActionCommand**: Flexible command implementation with parameter support
- **AsyncCommandBase**: Abstract base for asynchronous command operations
- **AsyncRelayCommand**: Async command with built-in exception handling

### Logging Infrastructure
- **Serilog Integration**: Built-in support for structured logging
- **LogManager**: Simplified logger creation with context-aware logging
- **UserName Enricher**: Custom enrichers for enhanced log metadata

### Configuration Management
- **AppSettingsUpdater**: Utility for runtime appsettings.json manipulation
- **Connection String Management**: Easy database connection string updates

## 📦 Installation

### NuGet Package
```bash
dotnet add package DotNetTools.Wpfkit
```

Or via Package Manager Console in Visual Studio:
```bash
Install-Package DotNetTools.Wpfkit
```

Or add directly to your `.csproj`:
```xml
<PackageReference Include="DotNetTools.Wpfkit" Version="1.0.0" />
```

### Manual Installation
1. Clone the repository
2. Build the project
3. Reference the DLL in your WPF application

```bash
git clone https://github.com/omostan/DotNetTools.Wpfkit
cd DotNetTools.Wpfkit
dotnet build
```

## 📋 Requirements

- **.NET 10.0 or later**
- **Windows OS** (for WPF support)
- **Visual Studio 2022** or later (recommended)

### Dependencies
- **Serilog** (v4.3.0+): Structured logging
- **Tracetool.DotNet.Api** (v14.0.0+): Advanced tracing capabilities

## 🚀 Usage

### MVVM Components

#### ObservableObject
Base class for implementing property change notifications:


```csharp
using DotNetTools.Wpfkit.MvvM;

public class MyModel : ObservableObject
{
    private string _name;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    
    private int _age;
    public int Age
    {
        get => _age;
        set => SetProperty(ref _age, value, onChanged: () => {
            // Execute when age changes
            OnPropertyChanged(nameof(IsAdult));
        });
    }
    
    public bool IsAdult => Age >= 18;
}
```

#### BaseViewModel
Rich view model base class with common UI properties:

```csharp
using DotNetTools.Wpfkit.MvvM;

public class MainViewModel : BaseViewModel
{
    public MainViewModel()
    {
        Title = "My Application";
        Subtitle = "Welcome Screen";
        Icon = "icon.png";
    }
    
    public async Task LoadDataAsync()
    {
        IsBusy = true;
        try
        {
            // Load data
        }
        finally
        {
            IsBusy = false;
        }
    }
}
```

**Available Properties:**
- `Title`: Main title text
- `Subtitle`: Secondary descriptive text
- `Icon`: Icon path or resource
- `IsBusy`: Indicates loading state
- `IsNotBusy`: Inverse of IsBusy
- `CanLoadMore`: Pagination support
- `Header`: Header content
- `Footer`: Footer content

#### ObservableRangeCollection<T>
Enhanced collection with bulk operations:

```csharp
using DotNetTools.Wpfkit.MvvM;

var collection = new ObservableRangeCollection<string>();

// Add multiple items efficiently
var items = new[] { "Item1", "Item2", "Item3" };
collection.AddRange(items);

// Replace entire collection
collection.ReplaceRange(newItems);

// Remove multiple items
collection.RemoveRange(itemsToRemove);

// Replace with single item
collection.Replace(singleItem);
```

**AddRange Notification Modes:**
- `NotifyCollectionChangedAction.Add`: Notify for each added item (default)
- `NotifyCollectionChangedAction.Reset`: Single reset notification

### Command Infrastructure

The toolkit provides a comprehensive set of command implementations for both synchronous and asynchronous operations in MVVM applications.

#### CommandBase
Abstract base class that implements `ICommand`:

```csharp
using DotNetTools.Wpfkit.Commands;

public class MyCustomCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        // Your command logic here
    }
    
    public override bool CanExecute(object? parameter)
    {
        // Return true if command can execute
        return base.CanExecute(parameter);
    }
    
    public void RaiseCanExecuteChanged()
    {
        OnCanExecuteChanged();
    }
}
```
**Features:**
- Implements `ICommand` interface
- Provides `CanExecuteChanged` event
- Virtual `CanExecute` method (returns `true` by default)
- Abstract `Execute` method for derived classes
- Protected `OnCanExecuteChanged()` method to trigger re-evaluation

#### ActionCommand
Flexible command that accepts an action delegate and optional predicate:

```csharp
using DotNetTools.Wpfkit.Commands;

public class MyViewModel : BaseViewModel
{
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    
    public MyViewModel()
    {
        // Simple command
        SaveCommand = new ActionCommand(
            action: param => Save(param),
            predicate: param => param != null && CanSave
        );
        
        // Command with validation
        DeleteCommand = new ActionCommand(
            action: param => Delete((string)param),
            predicate: param => param is string id && !string.IsNullOrEmpty(id)
        );
    }
    
    private bool CanSave => !string.IsNullOrEmpty(DataToSave);
    private string DataToSave { get; set; }
    
    private void Save(object data)
    {
        // Save logic
    }
    
    private void Delete(string id)
    {
        // Delete logic
    }
}
```

**Constructor Parameters:**
- `action`: `Action<object>` - The method to execute (required)
- `predicate`: `Predicate<object>?` - Condition to check if command can execute (optional)

**Features:**
- Integrates with `CommandManager.RequerySuggested` for automatic UI updates
- Parameter validation through predicate
- Throws `ArgumentNullException` if action is null

#### RelayCommand
Internal implementation extending `ActionCommand` with the same functionality:

```csharp
using DotNetTools.Wpfkit.Commands;

// RelayCommand is internally used but provides the same API
var command = new RelayCommand(
    action: param => Console.WriteLine($"Executed with {param}"),
    predicate: param => param != null
);
```

#### AsyncCommandBase
Abstract base class for asynchronous command operations with automatic execution state management:

```csharp
using DotNetTools.Wpfkit.Commands;

public class LoadDataCommand : AsyncCommandBase
{
    private readonly DataService _dataService;
    
    public LoadDataCommand(DataService dataService, Action<Exception> onException) 
        : base(onException)
    {
        _dataService = dataService;
    }
    
    protected override async Task ExecuteAsync(object parameter)
    {
        // Long-running async operation
        var data = await _dataService.LoadDataAsync();
        
        // Process data
        await ProcessDataAsync(data);
    }
}
```

**Features:**
- Prevents multiple concurrent executions (`IsExecuting` state)
- Automatically disables command during execution
- Built-in exception handling and logging (using TraceTool)
- Invokes custom exception handler
- Updates `CanExecute` state automatically

**Constructor Parameters:**
- `onException`: `Action<Exception>` - Callback invoked when an exception occurs

#### AsyncRelayCommand
Concrete implementation of async command for quick usage:

```csharp
using DotNetTools.Wpfkit.Commands;

public class MyViewModel : BaseViewModel
{
    public ICommand LoadDataCommand { get; }
    public ICommand SaveDataCommand { get; }
    
    public MyViewModel()
    {
        // Simple async command
        LoadDataCommand = new AsyncRelayCommand(
            callback: async () => await LoadDataAsync(),
            onException: ex => ShowError(ex.Message)
        );
        
        // Async command with complex logic
        SaveDataCommand = new AsyncRelayCommand(
            callback: async () => 
            {
                IsBusy = true;
                try
                {
                    await SaveToServerAsync();
                    await SaveToLocalAsync();
                    ShowSuccess("Data saved successfully");
                }
                finally
                {
                    IsBusy = false;
                }
            },
            onException: ex => 
            {
                Logger.Error(ex, "Failed to save data");
                ShowError($"Save failed: {ex.Message}");
            }
        );
    }
    
    private async Task LoadDataAsync()
    {
        // Load data from API
        var data = await _apiClient.GetDataAsync();
        Items.ReplaceRange(data);
    }
    
    private async Task SaveToServerAsync()
    {
        await _apiClient.SaveAsync(Data);
    }
    
    private async Task SaveToLocalAsync()
    {
        await _localDb.SaveAsync(Data);
    }
    
    private void ShowError(string message) { /* ... */ }
    private void ShowSuccess(string message) { /* ... */ }
}
```

**Constructor Parameters:**
- `callback`: `Func<Task>` - The async method to execute
- `onException`: `Action<Exception>` - Exception handler

**Key Benefits:**
- No need to manage `IsExecuting` state manually
- Built-in exception handling
- Automatic `CanExecute` management
- Prevents rapid clicking/double execution
- Integrated logging for errors

#### Complete ViewModel Example with Commands

```csharp
using DotNetTools.Wpfkit.MvvM;
using DotNetTools.Wpfkit.Commands;
using System.Windows.Input;

public class CustomerViewModel : BaseViewModel
{
    private readonly ICustomerService _customerService;
    private readonly IDialogService _dialogService;
    
    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }
    
    private Customer _selectedCustomer;
    public Customer SelectedCustomer
    {
        get => _selectedCustomer;
        set => SetProperty(ref _selectedCustomer, value);
    }
    
    public ObservableRangeCollection<Customer> Customers { get; }
    
    // Commands
    public ICommand LoadCustomersCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand RefreshCommand { get; }
    
    public CustomerViewModel(ICustomerService customerService, IDialogService dialogService)
    {
        _customerService = customerService;
        _dialogService = dialogService;
        
        Customers = new ObservableRangeCollection<Customer>();
        
        // Async command for loading data
        LoadCustomersCommand = new AsyncRelayCommand(
            callback: LoadCustomersAsync,
            onException: HandleException
        );
        
        // Sync command with parameter validation
        SearchCommand = new ActionCommand(
            action: param => SearchCustomers((string)param),
            predicate: param => param is string text && !string.IsNullOrWhiteSpace(text)
        );
        
        // Async command with condition
        SaveCommand = new AsyncRelayCommand(
            callback: SaveCustomerAsync,
            onException: HandleException
        );
        
        // Sync command with predicate
        DeleteCommand = new ActionCommand(
            action: param => DeleteCustomer(),
            predicate: param => SelectedCustomer != null
        );
        
        // Async refresh command
        RefreshCommand = new AsyncRelayCommand(
            callback: RefreshCustomersAsync,
            onException: HandleException
        );
    }
    
    private async Task LoadCustomersAsync()
    {
        IsBusy = true;
        Title = "Loading Customers...";
        
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
    
    private void SearchCustomers(string searchText)
    {
        var filtered = Customers.Where(c => 
            c.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
            c.Email.Contains(searchText, StringComparison.OrdinalIgnoreCase)
        ).ToList();
        
        Customers.ReplaceRange(filtered);
    }
    
    private async Task SaveCustomerAsync()
    {
        if (SelectedCustomer == null) return;
        
        IsBusy = true;
        try
        {
            await _customerService.SaveAsync(SelectedCustomer);
            await _dialogService.ShowMessageAsync("Success", "Customer saved successfully");
            await RefreshCustomersAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    private void DeleteCustomer()
    {
        if (SelectedCustomer == null) return;
        
        Customers.Remove(SelectedCustomer);
        SelectedCustomer = null;
    }
    
    private async Task RefreshCustomersAsync()
    {
        await LoadCustomersAsync();
    }
    
    private void HandleException(Exception ex)
    {
        IsBusy = false;
        _dialogService.ShowError("Error", ex.Message);
    }
}
```

**XAML Binding Example:**
```xml
<Window x:Class="MyApp.CustomerView"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <!-- Search Bar -->
        <StackPanel Grid.Row="0" Orientation="Horizontal" Margin="10">
            <TextBox x:Name="SearchBox" Width="200" Margin="0,0,10,0"/>
            <Button Content="Search" 
                    Command="{Binding SearchCommand}" 
                    CommandParameter="{Binding Text, ElementName=SearchBox}"/>
            <Button Content="Refresh" 
                    Command="{Binding RefreshCommand}" 
                    Margin="10,0,0,0"/>
        </StackPanel>
        
        <!-- Customer List -->
        <DataGrid Grid.Row="1" 
                  ItemsSource="{Binding Customers}"
                  SelectedItem="{Binding SelectedCustomer}"
                  AutoGenerateColumns="True"
                  Margin="10"/>
        
        <!-- Action Buttons -->
        <StackPanel Grid.Row="2" Orientation="Horizontal" Margin="10">
            <Button Content="Load Customers" 
                    Command="{Binding LoadCustomersCommand}" 
                    Width="120" 
                    Margin="0,0,10,0"/>
            <Button Content="Save" 
                    Command="{Binding SaveCommand}" 
                    Width="80" 
                    Margin="0,0,10,0"/>
            <Button Content="Delete" 
                    Command="{Binding DeleteCommand}" 
                    Width="80"/>
        </StackPanel>
        
        <!-- Loading Indicator -->
        <ProgressBar Grid.Row="1" 
                     IsIndeterminate="True" 
                     Visibility="{Binding IsBusy, Converter={StaticResource BoolToVisibilityConverter}}"
                     Height="5" 
                     VerticalAlignment="Top"/>
    </Grid>
</Window>
```

#### Command Best Practices

1. **Choose the Right Command Type:**
   - Use `ActionCommand` for simple synchronous operations
   - Use `AsyncRelayCommand` for async operations (API calls, database operations)
   - Extend `CommandBase` or `AsyncCommandBase` for complex custom logic

2. **Exception Handling:**
   - Always provide an exception handler for async commands
   - Log exceptions appropriately
   - Show user-friendly error messages
   - Consider retry logic for transient failures

3. **UI State Management:**
   - Use `IsBusy` property during long operations
   - `AsyncCommandBase` automatically prevents concurrent execution
   - Update UI elements to reflect command state

4. **Parameter Validation:**
   - Use predicates to validate command parameters
   - Return `false` from `CanExecute` to disable UI elements
   - Validate parameter types before casting

5. **Memory Management:**
   - Commands hold references to delegates and view models
   - Be careful with closures capturing large objects
   - Consider weak references for event handlers if needed

### Logging

#### Setting Up the Logger

```csharp
using DotNetTools.Wpfkit.Logging.Extensions;
using Serilog;

public class MyService
{
    // Get logger for current class
    private static readonly ILogger Log = LogManager.GetCurrentClassLogger();
    
    public void DoWork()
    {
        Log.Me().Information("Starting work at line {LineNumber}");
        
        try
        {
            // Your code here
            Log.Me().Debug("Processing item");
        }
        catch (Exception ex)
        {
            Log.Me().Error(ex, "Failed to process item");
        }
    }
}
```

**LogManager Features:**
- `GetCurrentClassLogger()`: Automatically creates logger with calling class context
- `Me()` extension: Adds line number information to log entries

#### Serilog Configuration Example

```csharp
using Serilog;
using DotNetTools.Wpfkit.Logging.Enrichers;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.With<UserNameEnricher>()
    .WriteTo.Console()
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();
```

### Configuration Management

#### Updating Connection Strings

```csharp
using DotNetTools.Wpfkit.Database;

// Update connection string in appsettings.json
string connectionString = "Data Source=myserver;Initial Catalog=mydb;";
AppSettingsUpdater.UpdateConnectionString(connectionString);
```

**Features:**
- Automatically locates appsettings.json in application directory
- Safely updates `ConnectDatabase` property
- Handles "Data Source=" prefix trimming
- Comprehensive error handling and logging
- Writes formatted JSON (indented)

**appsettings.json Structure:**
```json
{
  "ConnectDatabase": "path/to/database.db",
  "OtherSettings": "..."
}
```
## 📚 API Reference

### MvvM Namespace

#### ObservableObject
```csharp
protected bool SetProperty<T>(
    ref T backingStore, 
    T value,
    string propertyName = "",
    Action onChanged = null,
    Func<T, T, bool> validateValue = null)
```
- **backingStore**: Reference to the backing field
- **value**: New value to set
- **propertyName**: Property name (auto-filled via CallerMemberName)
- **onChanged**: Optional callback when value changes
- **validateValue**: Optional validation function
- **Returns**: `true` if property changed, `false` otherwise

#### BaseViewModel Properties

| Property    | Type   | Description                           |
|-------------|--------|---------------------------------------|
| Title       | string | Main title text                       |
| Subtitle    | string | Secondary descriptive text            |
| Icon        | string | Icon path or resource identifier      |
| IsBusy      | bool   | Indicates if operation is in progress |
| IsNotBusy   | bool   | Inverse of IsBusy (auto-synchronized) |
| CanLoadMore | bool   | Supports pagination scenarios         |
| Header      | string | Header content                        |
| Footer      | string | Footer content                        |

#### ObservableRangeCollection<T> Methods

```csharp
void AddRange(IEnumerable<T> collection, NotifyCollectionChangedAction notificationMode = Add)
void RemoveRange(IEnumerable<T> collection, NotifyCollectionChangedAction notificationMode = Reset)
void Replace(T item)
void ReplaceRange(IEnumerable<T> collection)

```

### Commands Namespace

#### CommandBase

```csharp
public abstract class CommandBase : ICommand
{
    public event EventHandler? CanExecuteChanged;
    public virtual bool CanExecute(object? parameter);
    public abstract void Execute(object? parameter);
    protected void OnCanExecuteChanged();
}
```

#### ActionCommand

```csharp
public class ActionCommand : ICommand
{
    public ActionCommand(Action<object> action, Predicate<object>? predicate = null);
    public bool CanExecute(object? parameter);
    public void Execute(object? parameter);
    public event EventHandler? CanExecuteChanged;
}
```

#### AsyncCommandBase

```csharp
public abstract class AsyncCommandBase : CommandBase
{
    public AsyncCommandBase(Action<Exception> onException);
    protected abstract Task ExecuteAsync(object parameter);
    public override bool CanExecute(object? parameter);
    public override void Execute(object? parameter);
}
```

#### AsyncRelayCommand

```csharp
public class AsyncRelayCommand : AsyncCommandBase
{
    public AsyncRelayCommand(Func<Task> callback, Action<Exception> onException);
    protected override Task ExecuteAsync(object parameter);
}
```

**Command Comparison Table:**
| Command Type | Sync/Async | Use Case | Exception Handling | Concurrent Execution Prevention |
|--------------|------------|----------|--------------------|---------------------------------|
| CommandBase | Sync | Base for custom commands | Manual | No |
| ActionCommand | Sync | Simple actions with parameters | Manual | No |
| RelayCommand | Sync | Internal relay implementation | Manual | No |
| AsyncCommandBase | Async | Base for async commands | Built-in | Yes |
| AsyncRelayCommand | Async | Async operations | Built-in | Yes |
