# DotNetTools.Wpfkit.Tests

Unit tests for the DotNetTools.Wpfkit library.

## 📋 Overview

This project contains comprehensive unit tests for all components of the DotNetTools.Wpfkit library, ensuring reliability and maintaining code quality.

## 🧪 Test Framework

- **Testing Framework**: xUnit 2.9.2
- **Assertion Library**: FluentAssertions 6.12.2
- **Mocking Framework**: Moq 4.20.72
- **Code Coverage**: Coverlet 6.0.2
- **Target Framework**: .NET 10.0

## 📁 Test Organization

Tests are organized by component with clear separation of concerns:

```
DotNetTools.Wpfkit.Tests/
├── MvvM/
|   ├── ObservableObjectTests.cs          # Tests for ObservableObject
|   ├── BaseViewModelTests.cs             # Tests for BaseViewModel
|   ├── ObservableRangeCollectionTests.cs # Tests for ObservableRangeCollection
├── Logging/
|   ├── Extensions/
|   |   └── LogManagerTests.cs            # Tests for LogManager
|   |   └── UserNameTests.cs              # Tests for Username helper
|   └── Enrichers/
|       └── UserNameEnricherTests.cs      # Tests for Serilog enricher
├── Database/
|   └── AppSettingsUpdaterTests.cs        # Tests for AppSettingsUpdater
└── DotNetTools.Wpfkit.csproj
```

## 🚀 Running Tests

### Visual Studio
1. Open Test Explorer (Test → Test Explorer)
2. Click "Run All" to execute all tests
3. View results in Test Explorer window

### Command Line


```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity detailed

# Run specific test class
dotnet test --filter "FullyQualifiedName~ObservableObjectTests"

# Run specific test method
dotnet test --filter "FullyQualifiedName~ObservableObjectTests.SetProperty_WhenValueChanges_ShouldReturnTrue"

# Run tests with code coverage
dotnet test --collect:"XPlat Code Coverage"

# Run tests and generate coverage report
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults
```

### CI/CD
Tests are automatically run in the GitHub Actions pipeline on every push and pull request.

## 📊 Test Coverage

### Current Coverage by Component

| Component | Test Classes | Test Methods | Coverage |
|-----------|--------------|--------------|----------|
| **ObservableObject** | 1 | 25+ | ~95% |
| **BaseViewModel** | 1 | 30+ | ~98% |
| **ObservableRangeCollection** | 1 | 40+ | ~92% |
| **LogManager** | 1 | 20+ | ~85% |
| **AppSettingsUpdater** | 1 | 25+ | ~80% |

**Total**: 140+ test methods

## 📝 Test Categories

### 1. Unit Tests
- Test individual methods and properties
- Verify correct behavior with valid inputs
- Test edge cases and boundary conditions

### 2. Integration Tests
- Test component interactions
- Verify end-to-end workflows
- Test async operations

### 3. Property Tests
- Verify property getters and setters
- Test PropertyChanged events
- Validate default values

### 4. Edge Case Tests
- Null/empty input handling
- Thread safety scenarios
- Error conditions

### 5. Performance Tests
- Bulk operation efficiency
- Response time validation
- Resource usage checks

## 💡 Best Practices

### Test Naming
- Use descriptive names: `MethodName_Scenario_ExpectedBehavior`
- Be specific about what's being tested
- Include the expected outcome

### Test Structure
- One logical assertion per test
- Keep tests focused and simple
- Use helper methods for common setup

### Test Data
- Use realistic test data
- Test edge cases (null, empty, boundary values)
- Test both success and failure paths

## 🤝 Contributing Tests

When adding new features to DotNetTools.Wpfkit:

1. **Write tests first** (TDD approach recommended)
2. **Cover all scenarios**: happy path, edge cases, errors
3. **Maintain naming conventions**
4. **Update this README** if adding new test categories
5. **Ensure tests pass** before submitting PR
6. **Aim for 80%+ coverage** of new code

### Test Checklist
- [ ] Happy path scenario tested
- [ ] Edge cases covered (null, empty, boundary values)
- [ ] Error conditions tested
- [ ] PropertyChanged events verified (for MVVM)
- [ ] Async operations tested properly
- [ ] Thread safety considered
- [ ] Performance acceptable
- [ ] Tests are independent (no shared state)
- [ ] Tests are deterministic (no random failures)
- [ ] Test names are descriptive

## 📚 Additional Resources

### xUnit
- [xUnit Documentation](https://xunit.net/)
- [xUnit Samples](https://github.com/xunit/samples)

### FluentAssertions
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [Assertion Scope](https://fluentassertions.com/introduction#assertion-scopes)

### Moq
- [Moq Documentation](https://github.com/moq/moq4)
- [Moq Quickstart](https://github.com/moq/moq4/wiki/Quickstart)

### Code Coverage
- [Coverlet Documentation](https://github.com/coverlet-coverage/coverlet)
- [ReportGenerator](https://github.com/danielpalme/ReportGenerator)

## 📧 Support

For test-related questions:
- Review existing tests for patterns
- Check xUnit/FluentAssertions documentation
- Open an issue on GitHub
- Contact: [Stanley Omoregie](mailto:stan@omotech.com)

---

**Test Status**: ✅ 140+ tests | 90%+ coverage | All passing

**Last Updated**: November 20, 2025
