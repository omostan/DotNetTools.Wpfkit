# DotNetTools.Wpfkit

[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/download)
[![NuGet](https://img.shields.io/nuget/v/DotNetTools.Wpfkit.svg)](https://www.nuget.org/packages/DotNetTools.Wpfkit/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/DotNetTools.Wpfkit.svg)](https://www.nuget.org/packages/DotNetTools.Wpfkit/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![GitHub Release](https://img.shields.io/github/v/release/omostan/DotNetTools.Wpfkit)](https://github.com/omostan/DotNetTools.Wpfkit/releases)

A comprehensive WPF toolkit library that provides essential components for building modern Windows desktop applications with the MVVM pattern, logging capabilities, and configuration management.

## 📑 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Installation](#installation)
- [Requirements](#requirements)
- [Usage](#usage)
  - [MVVM Components](#mvvm-components)
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

### Logging Namespace

#### LogManager

```csharp

static ILogger GetCurrentClassLogger()
static ILogger Me(this ILogger logger, int sourceLineNumber = 0)

```

### Database Namespace

#### AppSettingsUpdater
```csharp
static void UpdateConnectionString(string connectionString)
```

## 🏗️ Architecture

### Project Structure

```
DotNetTools.Wpfkit/
├── MvvM/
|   ├── ObservableObject.cs          # Base observable implementation
|   ├── BaseViewModel.cs             # Rich view model base class
|   ├── ObservableRangeCollection.cs # Bulk operations collection
├── Logging/
|   ├── Extensions/
|   |   └── LogManager.cs            # Logger factory
|   |   └── UserName.cs              # Username helper
|   └── Enrichers/
|       └── UserNameEnricher.cs      # Serilog enricher
├── Database/
|   └── AppSettingsUpdater.cs        # Configuration management
└── DotNetTools.Wpfkit.csproj

```

### Design Principles
- **SOLID Principles**: Clean, maintainable code architecture
- **Separation of Concerns**: Each component has a single responsibility
- **Reusability**: Generic, flexible implementations
- **Performance**: Optimized bulk operations in collections
- **Type Safety**: Leverages nullable reference types

## 🤝 Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Coding Standards
- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments
- Include unit tests for new features
- Maintain the existing copyright header format

## 📜 License

Copyright © 2025 **Omotech Digital Solutions**  
Licensed under the [MIT License](LICENSE).

This project is open source software created by [Stanley Omoregie](mailto:stan@omotech.com).

## 📧 Contact

**Author**: [Stanley Omoregie](mailto:stan@omotech.com)  
**Organization**: Omotech Digital Solutions  
**Created**: November 20, 2025

For questions, issues, or feature requests, please open an issue on the repository.

---

## 🔖 Version History

### Version 1.0.0 (2025-11-20)
- Initial release
- MVVM pattern components (ObservableObject, BaseViewModel, ObservableRangeCollection)
- Serilog logging integration
- AppSettings configuration management
- .NET 10.0 support

---

## 📖 Learning Resources

### WPF & MVVM
- [Microsoft WPF Documentation](https://docs.microsoft.com/wpf)
- [MVVM Pattern Guide](https://docs.microsoft.com/archive/msdn-magazine/2009/february/patterns-wpf-apps-with-the-model-view-viewmodel-design-pattern)

### Serilog
- [Serilog Official Documentation](https://serilog.net/)
- [Structured Logging Concepts](https://nblumhardt.com/2016/06/structured-logging-concepts-in-net-series-1/)

### .NET 10
- [What's New in .NET 10](https://docs.microsoft.com/dotnet/core/whats-new/dotnet-10)

---

**Built with ❤️ using .NET 10.0 and modern C# features**
