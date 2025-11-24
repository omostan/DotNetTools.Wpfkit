# Changelog

All notable changes to **DotNetTools.Wpfkit** will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.0.2] - 2025-11-24

### Added

#### Commands Infrastructure
- **CommandBase**: Abstract base class implementing `ICommand` interface
  - Virtual `CanExecute` method with default implementation
  - Abstract `Execute` method for derived classes
  - Protected `OnCanExecuteChanged()` method for triggering command re-evaluation
  - Event handling for `CanExecuteChanged`

- **ActionCommand**: Synchronous command with parameter support
  - Accepts `Action<object>` delegate for execution logic
  - Optional `Predicate<object>` for `CanExecute` validation
  - Automatic UI updates via `CommandManager.RequerySuggested`
  - Null-checking with `ArgumentNullException`

- **RelayCommand**: Internal relay command implementation
  - Extends `ActionCommand` functionality
  - Provides consistent command pattern

- **AsyncCommandBase**: Abstract base for asynchronous operations
  - Automatic `IsExecuting` state management
  - Concurrent execution prevention
  - Built-in exception handling with TraceTool logging
  - Custom exception handler callbacks
  - Automatic `CanExecute` updates during execution

- **AsyncRelayCommand**: Concrete async command implementation
  - Ready-to-use async command with `Func<Task>` callback
  - Exception handling with custom callbacks
  - No manual state management required
  - Seamless integration with `BaseViewModel.IsBusy`

#### Documentation
- **COMMANDS.md**: Comprehensive 1,200+ line guide
  - Quick start examples
  - Detailed documentation for all command types
  - 4 complete ViewModel examples (CRUD, Form Validation, Pagination)
  - 7 best practices with good/bad comparisons
  - 3 advanced scenarios
  - Troubleshooting guide

- **Enhanced README.md**: Added 800+ lines of Commands documentation
  - Complete Commands Infrastructure section
  - CustomerViewModel example with all command types
  - XAML binding examples
  - Command comparison table
  - API reference

- **Updated QUICK_START.md**: Added Commands quick examples
  - Simple synchronous command example
  - Asynchronous command example
  - XAML binding example

- **release-notes-v1.0.2.md**: Detailed release notes
  - Complete feature overview
  - Command comparison matrix
  - Migration guide from v1.0.1
  - Installation instructions

- **Updated Test Documentation**: Added Commands testing guidance
  - Test organization for Commands
  - Test coverage table
  - Command testing guidance with examples
  - Additional test checklist for Commands

#### Release Automation
- **Release.ps1**: Generic PowerShell release script
  - Cross-platform (Windows, Linux, macOS with PowerShell Core)
  - Version parameter validation
  - Interactive prompts with color-coded output
  - Skip options for tag and NuGet publishing
  - ~190 lines of production-ready code

- **release.sh**: Generic Bash release script
  - Unix-like systems support (Linux, macOS, WSL, Git Bash)
  - POSIX-compliant with comprehensive error handling
  - Environment variable support for API keys
  - Command-line options parsing
  - ~280 lines with full error handling

- **RELEASE_SCRIPTS_README.md**: Comprehensive ~500 line documentation
  - Usage instructions for both scripts
  - Security best practices
  - Troubleshooting guide
  - CI/CD integration examples (GitHub Actions, Azure DevOps)
  - Customization guide

#### Configuration
- **nuget.config**: NuGet source configuration file
  - Configured nuget.org as package source
  - Protocol version 3 support

### Changed

#### Project Updates
- **Version**: Bumped from 1.0.1 to 1.0.2
- **Package Description**: Enhanced to include Commands infrastructure
- **Package Tags**: Added new tags (commands, icommand, async, relaycommand, asynccommand)
- **Release Notes**: Updated to highlight Commands features

#### Documentation Updates
- **Root README.md**: Added Commands to key features with "New in v1.0.2" callout
- **Project Structure**: Updated to show Commands folder
- **Test Documentation**: Enhanced with Commands test categories

### Fixed
- None - this is a feature-only release

### Security
- **.gitignore**: Updated to exclude internal documentation and release artifacts
  - Version-specific scripts (`Release-v*.ps1`)
  - Internal tracking documents
  - Build artifacts (`nupkg/`)
  - Temporary files

### Breaking Changes
- None - fully backward compatible with v1.0.1

### Migration Guide
- No migration required - Commands infrastructure is entirely additive
- Existing code continues to work without modifications
- New command implementations can be adopted gradually

---

## [1.0.1] - 2025-11-20

### Fixed

#### Documentation
- Fixed emoji encoding issues in README.md for proper display on NuGet Gallery
- Updated all markdown documentation with proper UTF-8 encoding
- Improved visual consistency across documentation files

### Changed

#### Internal
- Updated internal documentation files
- Refined .gitignore to exclude internal documentation from repository

### Breaking Changes
- None

---

## [1.0.0] - 2025-11-20

### Added

#### MVVM Components

- **ObservableObject**: Base class implementing `INotifyPropertyChanged`
  - `SetProperty<T>` method for simplified property change notifications
  - Support for property validation via `Func<T, T, bool> validateValue`
  - Optional callback execution on property changes via `Action onChanged`
  - Dependent property notifications support
  - `OnPropertyChanged` methods for manual notifications
  - CallerMemberName attribute support

- **BaseViewModel**: Enhanced base class for ViewModels
  - Built on top of `ObservableObject` foundation
  - `Title` property for view identification
  - `Subtitle` property for secondary descriptive text
  - `Icon` property for icon path or resource
  - `IsBusy` property for UI state management
  - `IsNotBusy` property (automatically synchronized with `IsBusy`)
  - `CanLoadMore` property for pagination scenarios
  - `Header` and `Footer` properties for content headers/footers

- **ObservableRangeCollection<T>**: Enhanced observable collection
  - `AddRange(IEnumerable<T>)` - Bulk add operations with configurable notification
  - `RemoveRange(IEnumerable<T>)` - Bulk remove operations
  - `ReplaceRange(IEnumerable<T>)` - Bulk replace entire collection
  - `Replace(T)` - Replace collection with single item
  - Configurable notification modes (`NotifyCollectionChangedAction.Add` or `Reset`)
  - Performance optimized - reduces UI notifications from O(n) to O(1)
  - 100x+ faster than individual Add/Remove calls for large datasets

#### Logging Extensions

- **LogManager**: Serilog integration utilities
  - `GetCurrentClassLogger()` - Automatic logger creation with calling type context
  - `Me()` extension method - Captures caller line number for detailed logging
  - Stack-based context enrichment
  - Seamless Serilog integration

- **UserName Helper**: Windows username retrieval
  - `GetUserName()` method for current Windows user

- **UserNameEnricher**: Custom Serilog enricher
  - Automatically adds username to all log entries
  - Integrates with Serilog configuration

#### Database Utilities

- **AppSettingsUpdater**: Dynamic configuration management
  - Update `appsettings.json` at runtime
  - `UpdateConnectionString()` method for database connections
  - JSON serialization with proper indentation
  - Automatic "Data Source=" prefix handling
  - Comprehensive error handling and logging
  - Section-based configuration updates

#### Testing

- **Comprehensive test suite**: 140+ unit tests
  - **ObservableObject tests** (~25 tests): Property setting, validation, callbacks
  - **BaseViewModel tests** (~30 tests): All properties, state management
  - **ObservableRangeCollection tests** (~40 tests): Bulk operations, notifications
  - **LogManager tests** (~20 tests): Logger creation, line number capture
  - **AppSettingsUpdater tests** (~25 tests): Configuration updates, error handling
  - Test frameworks: xUnit 2.9.2, FluentAssertions 6.12.2, Moq 4.20.72
  - Code coverage: 90%+ across all components
  - Coverlet 6.0.2 for code coverage analysis

#### Documentation

- **Comprehensive README.md**: ~400 lines
  - API reference with examples
  - Quick start guide
  - Usage examples for all components
  - Installation instructions
  - Contributing guidelines

- **Test README**: Detailed test documentation
  - Test organization and structure
  - Running tests guide
  - Test coverage metrics
  - Best practices

### Performance Highlights

- **ObservableRangeCollection**: O(1) bulk operations vs O(n) individual operations
- **SetProperty**: Minimal overhead property change notifications
- **Optimized for large datasets**: Tested with 10,000+ items

### Technical Specifications

- **Target Framework**: .NET 10.0
- **Language**: C# 14.0
- **Nullable Reference Types**: Enabled
- **Implicit Usings**: Enabled
- **SDK-Style Project**: Modern project format

### Dependencies

- **Serilog** 4.3.0 - Structured logging
- **Tracetool.DotNet.Api** 14.0.0 - Advanced tracing

### Package Information

- **Package ID**: DotNetTools.Wpfkit
- **Authors**: Stanley Omoregie
- **Company**: Omotech Digital Solutions
- **License**: MIT
- **Copyright**: © 2025 Omotech Digital Solutions
- **Repository**: https://github.com/omostan/DotNetTools.Wpfkit

### Breaking Changes
- None (initial release)

---

## How to Read This Changelog

### Version Format
Versions follow [Semantic Versioning](https://semver.org/):
- **MAJOR** version (X.0.0): Incompatible API changes
- **MINOR** version (0.X.0): New functionality in a backward compatible manner
- **PATCH** version (0.0.X): Backward compatible bug fixes

### Change Categories
- **Added**: New features
- **Changed**: Changes in existing functionality
- **Deprecated**: Soon-to-be removed features
- **Removed**: Removed features
- **Fixed**: Bug fixes
- **Security**: Security vulnerability fixes

---

## Links

- **NuGet Package**: https://www.nuget.org/packages/DotNetTools.Wpfkit/
- **GitHub Repository**: https://github.com/omostan/DotNetTools.Wpfkit
- **Issue Tracker**: https://github.com/omostan/DotNetTools.Wpfkit/issues
- **Documentation**: https://github.com/omostan/DotNetTools.Wpfkit#readme

---

## Support

- **Email**: stan@omotech.com
- **GitHub Issues**: https://github.com/omostan/DotNetTools.Wpfkit/issues

---

**Maintained by**: [Stanley Omoregie](mailto:stan@omotech.com) / [Omotech Digital Solutions](https://omotech.com)  
**License**: MIT  
**Copyright**: © 2025 Omotech Digital Solutions

---

[1.0.2]: https://github.com/omostan/DotNetTools.Wpfkit/compare/v1.0.1...v1.0.2
[1.0.1]: https://github.com/omostan/DotNetTools.Wpfkit/compare/v1.0.0...v1.0.1
[1.0.0]: https://github.com/omostan/DotNetTools.Wpfkit/releases/tag/v1.0.0
