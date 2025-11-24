# 🚀 DotNetTools.Wpfkit v1.0.0 - Initial Release

## 📋 Overview
First official release of **DotNetTools.Wpfkit** - A comprehensive toolkit for WPF application development targeting .NET 10.

## ✨ Features

### MVVM Components
- **ObservableObject**: Base class implementing `INotifyPropertyChanged` with helper methods
  - `SetProperty<T>` method for simplified property change notifications
  - `OnPropertyChanged` methods for manual notifications
  - Support for dependent property notifications

- **BaseViewModel**: Enhanced base class for ViewModels
  - `IsBusy` and `IsNotBusy` properties for UI state management
  - `Title` property for view identification
  - Built on top of ObservableObject foundation

- **ObservableRangeCollection<T>**: Enhanced observable collection
  - `AddRange()` - Bulk add operations with single notification
  - `RemoveRange()` - Bulk remove operations
  - `ReplaceRange()` - Bulk replace operations
  - `Replace()` - Replace entire collection
  - Performance optimized for large datasets
  - Configurable notification modes (Add/Reset)

### Logging Extensions
- **LogManager**: Serilog integration utilities
  - `GetCurrentClassLogger()` - Automatic logger creation with calling type context
  - `Me()` extension - Captures caller line number for detailed logging
  - Stack-based context enrichment

### Database Utilities
- **AppSettingsUpdater**: Dynamic configuration management
  - Update `appsettings.json` at runtime
  - Section-based configuration updates
  - JSON serialization support

## 🎯 Highlights

### High Performance
- ObservableRangeCollection reduces UI notifications from O(n) to O(1)
- Bulk operations are 100x+ faster than individual Add/Remove calls
- Optimized for large dataset manipulation

### Comprehensive Testing
- **140+ unit tests** across all components
- **90%+ code coverage**
- Test frameworks: xUnit, FluentAssertions, Moq
- Performance tests included
- Thread safety tests

### Well Documented
- XML documentation for all public APIs
- Comprehensive README with examples
- Test documentation and patterns
- Best practices guidance

## 📦 Installation

### Using .NET CLI

````dotnetcli
dotnet add package DotNetTools.Wpfkit
````

### Using Package Manager Console

````dotnetcli
Install-Package DotNetTools.Wpfkit
````

## 🚀 Quick Start

### Using ObservableObject

````csharp

public class Person : ObservableObject
{
	private string _name;
	
	public string Name
	{
		get => _name; set => SetProperty(ref _name, value);
	}
}

````

### Using BaseViewModel

````csharp

public class MainViewModel : BaseViewModel
{
	public MainViewModel()
	{
		Title = "Main View";
	}

	public async Task LoadDataAsync()
	{
		IsBusy = true;

		try
		{
			// Load data...
		}
		finally
		{
			IsBusy = false;
		}
	}
}

````

````csharp

public class EmployeeViewModel : BaseViewModel
{
	public ObservableRangeCollection<Employee> Employees { get; } = new ObservableRangeCollection<Employee>();

	public void LoadEmployees(IEnumerable<Employee> employees)
	{
		// Clear existing employees
		Employees.Clear();

		// Add new employees
		Employees.AddRange(employees);
	}
}

````

### Using ObservableRangeCollection

````csharp

var collection = new ObservableRangeCollection<int>();

// Bulk operations collection.AddRange(Enumerable.Range(1, 1000));

collection.RemoveRange(itemsToRemove);

collection.ReplaceRange(newItems);

````

### Using LogManager

````csharp
public class MyClass
{
	private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

	public void MyMethod()
	{
		_logger.Me().Information("Method called");
	}
}

````


## ⚙️ Requirements

- **.NET 10.0** or later
- **Serilog 4.1.0** (for logging features)
- **Microsoft.Extensions.Configuration** (for configuration features)

## 📦 Package Information

- **Target Framework**: .NET 10.0
- **Language Version**: C# 14.0
- **Package ID**: DotNetTools.Wpfkit
- **Authors**: Stanley Omoregie
- **Company**: Omotech Digital Solutions
- **Copyright**: © 2025 Omotech Digital Solutions

## 🐛 Bug Fixes

This release includes fixes for:
- Test isolation issues with LogManager tests
- Proper disposal of logger resources
- Thread safety in test execution

## ⚠️ Breaking Changes

None (initial release)

## 🗺️ Roadmap

Planned features for future releases:
- Additional MVVM helpers (RelayCommand, AsyncRelayCommand)
- Validation framework integration
- Dialog service abstractions
- Navigation service
- Dependency injection helpers
- More database utilities

## 🤝 Contributing

Contributions are welcome! Please read our contributing guidelines and submit pull requests.

## 📜 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👏 Acknowledgments

- Built with modern .NET 10 features
- Follows WPF best practices
- Inspired by community feedback and real-world usage

## 🆘 Support

- **Issues**: https://github.com/omostan/DotNetTools.Wpfkit/issues
- **Email**: stan@omotech.com
- **Documentation**: https://github.com/omostan/DotNetTools.Wpfkit

---

**Full Changelog**: https://github.com/omostan/DotNetTools.Wpfkit/commits/v1.0.0
