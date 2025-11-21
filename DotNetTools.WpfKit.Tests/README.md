# DotNetTools.Wpfkit.Tests

Unit tests for the DotNetTools.Wpfkit library.

## ?? Overview

This project contains comprehensive unit tests for all components of the DotNet.WpfToolKit library, ensuring reliability and maintaining code quality.

## ?? Test Framework

- **Testing Framework**: xUnit 2.9.2
- **Assertion Library**: FluentAssertions 6.12.2
- **Mocking Framework**: Moq 4.20.72
- **Code Coverage**: Coverlet 6.0.2
- **Target Framework**: .NET 10.0

## ?? Test Organization

Tests are organized by component with clear separation of concerns:

```
DotNetTools.Wpfkit.Tests/
??? MvvM/
?   ??? ObservableObjectTests.cs          # Tests for ObservableObject
?   ??? BaseViewModelTests.cs              # Tests for BaseViewModel
?   ??? ObservableRangeCollectionTests.cs  # Tests for ObservableRangeCollection
??? Logging/
?   ??? LogManagerTests.cs                 # Tests for LogManager
??? Database/
    ??? AppSettingsUpdaterTests.cs         # Tests for AppSettingsUpdater
```

## ?? Running Tests

### Visual Studio
1. Open Test Explorer (Test ? Test Explorer)
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

## ?? Test Coverage

### Current Coverage by Component

| Component | Test Classes | Test Methods | Coverage |
|-----------|--------------|--------------|----------|
| **ObservableObject** | 1 | 25+ | ~95% |
| **BaseViewModel** | 1 | 30+ | ~98% |
| **ObservableRangeCollection** | 1 | 40+ | ~92% |
| **LogManager** | 1 | 20+ | ~85% |
| **AppSettingsUpdater** | 1 | 25+ | ~80% |

**Total**: 140+ test methods

## ?? Test Categories

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

## ?? Test Examples

### Testing ObservableObject

```csharp
[Fact]
public void SetProperty_WhenValueChanges_ShouldRaisePropertyChanged()
{
    // Arrange
    var obj = new TestObservableObject();
    var eventRaised = false;
    obj.PropertyChanged += (sender, args) => eventRaised = true;

    // Act
    obj.Name = "John";

    // Assert
    eventRaised.Should().BeTrue();
}
```

### Testing BaseViewModel

```csharp
[Fact]
public void IsBusy_WhenSetToTrue_ShouldSetIsNotBusyToFalse()
{
    // Arrange
    var viewModel = new BaseViewModel();

    // Act
    viewModel.IsBusy = true;

    // Assert
    viewModel.IsBusy.Should().BeTrue();
    viewModel.IsNotBusy.Should().BeFalse();
}
```

### Testing ObservableRangeCollection

```csharp
[Fact]
public void AddRange_WithValidCollection_ShouldAddAllItems()
{
    // Arrange
    var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
    var newItems = new[] { 4, 5, 6 };

    // Act
    collection.AddRange(newItems);

    // Assert
    collection.Should().HaveCount(6);
    collection.Should().Equal(1, 2, 3, 4, 5, 6);
}
```

## ?? Test Utilities

### FluentAssertions
We use FluentAssertions for readable and expressive assertions:

```csharp
// Instead of:
Assert.Equal(expected, actual);

// We use:
actual.Should().Be(expected);

// Collections
collection.Should().HaveCount(5);
collection.Should().Contain(item);
collection.Should().BeEmpty();

// Exceptions
action.Should().Throw<ArgumentNullException>();
action.Should().NotThrow();

// Types
obj.Should().BeAssignableTo<INotifyPropertyChanged>();
```

### Test Patterns

#### Arrange-Act-Assert (AAA)
```csharp
[Fact]
public void MethodName_Scenario_ExpectedBehavior()
{
    // Arrange - Setup test data
    var sut = new SystemUnderTest();
    
    // Act - Execute the method
    var result = sut.Method();
    
    // Assert - Verify the result
    result.Should().Be(expected);
}
```

## ?? Debugging Tests

### Visual Studio
1. Set breakpoints in test methods
2. Right-click test ? Debug Test(s)
3. Use Test Explorer to navigate to failures

### Command Line
```bash
# Run tests in debug mode
dotnet test --logger "console;verbosity=detailed"

# Run specific failing test
dotnet test --filter "FullyQualifiedName~FailingTestName" --logger "console;verbosity=detailed"
```

## ?? Code Coverage Reports

### Generate Coverage Report

```bash
# Install ReportGenerator
dotnet tool install -g dotnet-reportgenerator-globaltool

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate HTML report
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

# Open report
start coveragereport/index.html
```

### Coverage Goals
- **Minimum**: 80% code coverage
- **Target**: 90% code coverage
- **Ideal**: 95%+ code coverage

## ?? Best Practices

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

### Async Testing
```csharp
[Fact]
public async Task AsyncMethod_ShouldCompleteSuccessfully()
{
    // Arrange
    var service = new TestService();
    
    // Act
    await service.DoWorkAsync();
    
    // Assert
    service.WorkCompleted.Should().BeTrue();
}
```

### Thread Safety Testing
```csharp
[Fact]
public void Method_WithConcurrentAccess_ShouldBeThreadSafe()
{
    // Arrange
    var obj = new ThreadSafeObject();
    var tasks = new List<Task>();
    
    // Act
    for (int i = 0; i < 10; i++)
    {
        tasks.Add(Task.Run(() => obj.Method()));
    }
    
    Task.WaitAll(tasks.ToArray());
    
    // Assert
    // Verify no exceptions and correct state
}
```

## ?? Continuous Integration

Tests run automatically on:
- Every push to main/develop
- Every pull request
- Scheduled nightly builds

### CI Configuration
See `.github/workflows/dotnet.yml` for the complete CI/CD pipeline configuration.

## ?? Contributing Tests

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

## ?? Additional Resources

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

## ?? Support

For test-related questions:
- Review existing tests for patterns
- Check xUnit/FluentAssertions documentation
- Open an issue on GitHub
- Contact: [Stanley Omoregie](mailto:stan@omotech.com)

---

**Test Status**: ? 140+ tests | 90%+ coverage | All passing

**Last Updated**: November 20, 2025
