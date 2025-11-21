# DotNetTools Solution

A modern .NET 10.0 solution containing the **DotNetTools.Wpfkit** library.

## ?? Projects

### DotNetTools.Wpfkit
A comprehensive WPF toolkit library providing essential components for building modern Windows desktop applications.

**Key Features:**
- MVVM pattern components (ObservableObject, BaseViewModel, ObservableRangeCollection)
- Serilog logging integration with context-aware extensions
- Configuration management utilities
- .NET 10.0 support with modern C# features

[View Library README](./DotNetTools.WpfKit/README.md)

## ?? Requirements

- **.NET 10.0 SDK or later**
- **Visual Studio 2022** or later (recommended)
- **Windows OS** (for WPF projects)

## ?? Getting Started

### Build the Solution

```bash
# Restore dependencies
dotnet restore

# Build all projects
dotnet build

# Build in Release mode
dotnet build -c Release
```

### Run Tests (if applicable)
```bash
dotnet test
```

## ?? Solution Structure

```
DotNetTools/
??? DotNetTools.slnx               # Solution file
??? DotNetTools.WpfKit/            # WPF Toolkit library
?   ??? MvvM/                      # MVVM components
?   ??? Logging/                   # Logging utilities
?   ??? Database/                  # Configuration management
?   ??? README.md                  # Library documentation
??? DotNetTools.WpfKit.Tests/     # Unit tests
??? .editorconfig                  # Code style configuration
??? README.md                      # This file
```

## ?? Technology Stack

- **.NET 10.0** - Latest .NET framework
- **C# 13** - Modern language features
- **WPF** - Windows Presentation Foundation
- **Serilog** - Structured logging
- **SDK-style projects** - Modern project format

## ??? Development

### Prerequisites
- Visual Studio 2022 (17.12 or later)
- .NET 10.0 SDK
- Git

### Building from Source
1. Clone the repository
2. Open `DotNetTools.slnx` in Visual Studio
3. Restore NuGet packages
4. Build the solution (Ctrl+Shift+B)

### Code Style
This solution uses `.editorconfig` to maintain consistent code style across the codebase. Please ensure your IDE respects these settings.

## ?? Documentation

- [DotNetTools.Wpfkit Library Documentation](./DotNetTools.WpfKit/README.md)

## ?? Contributing

Contributions are welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Follow existing code style and conventions
4. Add tests for new features
5. Submit a pull request

## ?? License

Copyright © 2025 Omotech Digital Solutions  
All rights reserved.

## ?? Contact

**Author**: Stanley Omoregie  
**Organization**: Omotech Digital Solutions

---

**Built with modern .NET technologies** ??
