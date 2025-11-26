#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <               DotNet WPF Tool Kit             |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: DotNetTools.Wpfkit.Tests                                          *
*            Filename: AsyncRelayCommandTests.cs                                         *
*              Author: Stanley Omoregie                                                  *
*        Created Date: 25.01.2025                                                        *
*       Modified Date: 25.01.2025                                                        *
*          Created By: Stanley Omoregie                                                  *
*    Last Modified By: Stanley Omoregie                                                  *
*           CopyRight: copyright © 2025 Omotech Digital Solutions                        *
*                  .oooO  Oooo.                                                          *
*                  (   )  (   )                                                          *
* ------------------\ (----) /---------------------------------------------------------- *
*                    \_)  (_/                                                            *
*****************************************************************************************/

#endregion copyright

using DotNetTools.Wpfkit.Commands;
using System.Windows.Input;

namespace DotNetTools.Wpfkit.Tests.Commands;

/// <summary>
/// Comprehensive unit tests for the AsyncRelayCommand class.
/// Tests async execution, exception handling, and concurrent execution prevention.
/// </summary>
public class AsyncRelayCommandTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidCallback_ShouldCreateCommand()
    {
        // Arrange & Act
        var command = new AsyncRelayCommand(
            async () => await Task.CompletedTask,
            _ => { }
        );

        // Assert
        command.Should().NotBeNull();
        command.Should().BeAssignableTo<ICommand>();
    }

    [Fact]
    public void Constructor_WithNullCallback_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new AsyncRelayCommand(null!, ex => { });

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_WithNullExceptionHandler_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new AsyncRelayCommand(async () => await Task.CompletedTask, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region CanExecute Tests

    [Fact]
    public void CanExecute_WhenNotExecuting_ShouldReturnTrue()
    {
        // Arrange
        var command = new AsyncRelayCommand(
            async () => await Task.CompletedTask,
            ex => { }
        );

        // Act
        var result = command.CanExecute(null);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CanExecute_WhileExecuting_ShouldReturnFalse()
    {
        // Arrange
        var tcs = new TaskCompletionSource<bool>();
        var command = new AsyncRelayCommand(
            async () => await tcs.Task,
            ex => { }
        );

        // Act
        command.Execute(null);
        await Task.Delay(50); // Give it time to start

        var canExecuteWhileRunning = command.CanExecute(null);

        tcs.SetResult(true);
        await Task.Delay(100); // Give it time to complete

        var canExecuteAfterComplete = command.CanExecute(null);

        // Assert
        canExecuteWhileRunning.Should().BeFalse();
        canExecuteAfterComplete.Should().BeTrue();
    }

    #endregion

    #region Execute Tests

    [Fact]
    public async Task Execute_ShouldInvokeCallback()
    {
        // Arrange
        var executed = false;
        var command = new AsyncRelayCommand(
            async () =>
            {
                await Task.Delay(10);
                executed = true;
            },
            ex => { }
        );

        // Act
        command.Execute(null);
        await Task.Delay(100); // Wait for async execution

        // Assert
        executed.Should().BeTrue();
    }

    [Fact]
    public async Task Execute_MultipleTimes_ShouldPreventConcurrentExecution()
    {
        // Arrange
        var executionCount = 0;
        var tcs = new TaskCompletionSource<bool>();

        var command = new AsyncRelayCommand(
            async () =>
            {
                executionCount++;
                await tcs.Task;
            },
            ex => { }
        );

        // Act
        command.Execute(null);
        await Task.Delay(50);

        // Try to execute again while first is running
        command.Execute(null);
        command.Execute(null);
        command.Execute(null);

        tcs.SetResult(true);
        await Task.Delay(100);

        // Assert
        executionCount.Should().Be(1); // Only first execution should run
    }

    [Fact]
    public async Task Execute_WithException_ShouldInvokeExceptionHandler()
    {
        // Arrange
        Exception? capturedException = null;
        var expectedMessage = "Test exception";

        var command = new AsyncRelayCommand(
            async () =>
            {
                await Task.Delay(10);
                throw new InvalidOperationException(expectedMessage);
            },
            ex => capturedException = ex
        );

        // Act
        command.Execute(null);
        await Task.Delay(150); // Wait for async execution and exception handling

        // Assert
        capturedException.Should().NotBeNull();
        capturedException.Should().BeOfType<InvalidOperationException>();
        capturedException!.Message.Should().Be(expectedMessage);
    }

    [Fact]
    public async Task Execute_AfterException_ShouldResetExecutionState()
    {
        // Arrange
        var executionCount = 0;
        var command = new AsyncRelayCommand(
            async () =>
            {
                await Task.Delay(10);
                executionCount++;
                if (executionCount == 1)
                    throw new InvalidOperationException();
            },
            ex => { }
        );

        // Act
        command.Execute(null);
        await Task.Delay(100);

        // Should be able to execute again after exception
        var canExecuteAfterError = command.CanExecute(null);
        command.Execute(null);
        await Task.Delay(100);

        // Assert
        canExecuteAfterError.Should().BeTrue();
        executionCount.Should().Be(2);
    }

    #endregion

    #region Exception Handling Tests

    [Fact]
    public async Task Execute_WithNullReferenceException_ShouldHandleGracefully()
    {
        // Arrange
        Exception? capturedException = null;
        string? nullString = null;

        var command = new AsyncRelayCommand(
            async () =>
            {
                await Task.Delay(10);
                var length = nullString!.Length; // Will throw NullReferenceException
            },
            ex => capturedException = ex
        );

        // Act
        command.Execute(null);
        await Task.Delay(150);

        // Assert
        capturedException.Should().BeOfType<NullReferenceException>();
    }

    [Fact]
    public async Task Execute_WithMultipleExceptions_ShouldHandleEach()
    {
        // Arrange
        var exceptions = new List<Exception>();
        var attemptCount = 0;

        var command = new AsyncRelayCommand(
            async () =>
            {
                await Task.Delay(10);
                attemptCount++;
                throw new InvalidOperationException($"Attempt {attemptCount}");
            },
            ex => exceptions.Add(ex)
        );

        // Act
        command.Execute(null);
        await Task.Delay(100);

        command.Execute(null);
        await Task.Delay(100);

        command.Execute(null);
        await Task.Delay(100);

        // Assert
        exceptions.Should().HaveCount(3);
        exceptions[0].Message.Should().Contain("Attempt 1");
        exceptions[1].Message.Should().Contain("Attempt 2");
        exceptions[2].Message.Should().Contain("Attempt 3");
    }

    #endregion

    #region Real-World MVVM Scenarios

    [Fact]
    public async Task MVVM_Scenario_LoadDataCommand_ShouldWork()
    {
        // Arrange
        var data = new List<string>();

        var command = new AsyncRelayCommand(
            async () =>
            {
                _ = true;
                await Task.Delay(50); // Simulate API call
                data.AddRange(["Item1", "Item2", "Item3"]);
                _ = false;
            },
            ex => _ = false
        );

        // Act
        command.CanExecute(null).Should().BeTrue();
        command.Execute(null);

        await Task.Delay(25);
        command.CanExecute(null).Should().BeFalse(); // Should be disabled while loading

        await Task.Delay(100);

        // Assert
        data.Should().HaveCount(3);
        command.CanExecute(null).Should().BeTrue();
    }

    [Fact]
    public async Task MVVM_Scenario_SaveCommand_WithErrorHandling_ShouldWork()
    {
        // Arrange
        var saveAttempts = 0;
        var errorOccurred = false;
        string? errorMessage = null;

        var command = new AsyncRelayCommand(
            async () =>
            {
                saveAttempts++;
                await Task.Delay(20);
                if (saveAttempts == 1)
                    throw new InvalidOperationException("Network error");
                // Second attempt succeeds
            },
            ex =>
            {
                errorOccurred = true;
                errorMessage = ex.Message;
            }
        );

        // Act - First attempt fails
        command.Execute(null);
        await Task.Delay(100);

        var firstAttemptError = errorOccurred;

        // Reset error state
        errorOccurred = false;

        // Second attempt succeeds
        command.Execute(null);
        await Task.Delay(100);

        // Assert
        saveAttempts.Should().Be(2);
        firstAttemptError.Should().BeTrue();
        errorMessage.Should().Contain("Network error");
        errorOccurred.Should().BeFalse(); // Second attempt didn't throw
    }

    [Fact]
    public async Task MVVM_Scenario_RefreshCommand_PreventDoubleClick_ShouldWork()
    {
        // Arrange
        var refreshCount = 0;

        var command = new AsyncRelayCommand(
            async () =>
            {
                refreshCount++;
                await Task.Delay(100); // Simulate slow operation
            },
            ex => { }
        );

        // Act - Simulate rapid button clicks
        command.Execute(null);
        command.Execute(null);
        command.Execute(null);

        await Task.Delay(200);

        // Assert - Should only execute once
        refreshCount.Should().Be(1);
    }

    #endregion

    #region Thread Safety Tests

    [Fact]
    public async Task Execute_FromMultipleThreads_ShouldBeThreadSafe()
    {
        // Arrange
        var counter = 0;
        var command = new AsyncRelayCommand(
            async () =>
            {
                await Task.Delay(10);
                Interlocked.Increment(ref counter);
            },
            ex => { }
        );

        // Act
        var tasks = Enumerable.Range(0, 10)
            .Select(_ => Task.Run(() => command.Execute(null)))
            .ToArray();

        await Task.Delay(200); // Wait for all to complete

        // Assert - Due to concurrent execution prevention, not all will execute
        counter.Should().BeGreaterThan(0).And.BeLessOrEqualTo(10);
    }

    #endregion

    #region CanExecuteChanged Event Tests

    [Fact]
    public async Task CanExecuteChanged_ShouldFireWhenExecutionStateChanges()
    {
        // Arrange
        var tcs = new TaskCompletionSource<bool>();
        var canExecuteChangedCount = 0;

        var command = new AsyncRelayCommand(
            async () => await tcs.Task,
            ex => { }
        );

        command.CanExecuteChanged += (s, e) => canExecuteChangedCount++;

        // Act
        command.Execute(null);
        await Task.Delay(50);

        var countWhileExecuting = canExecuteChangedCount;

        tcs.SetResult(true);
        await Task.Delay(100);

        var countAfterComplete = canExecuteChangedCount;

        // Assert
        countWhileExecuting.Should().BeGreaterThan(0);
        countAfterComplete.Should().BeGreaterThan(countWhileExecuting);
    }

    #endregion

    #region Complex Scenarios

    [Fact]
    public async Task ComplexScenario_ChainedAsyncOperations_ShouldWork()
    {
        // Arrange
        var operations = new List<string>();

        var command = new AsyncRelayCommand(
            async () =>
            {
                operations.Add("Start");
                await Task.Delay(20);

                operations.Add("LoadData");
                await Task.Delay(20);

                operations.Add("ProcessData");
                await Task.Delay(20);

                operations.Add("SaveData");
                await Task.Delay(20);

                operations.Add("Complete");
            },
            ex => operations.Add($"Error: {ex.Message}")
        );

        // Act
        command.Execute(null);
        await Task.Delay(200);

        // Assert
        operations.Should().ContainInOrder("Start", "LoadData", "ProcessData", "SaveData", "Complete");
    }

    [Fact]
    public async Task ComplexScenario_CancellationDuringExecution_ShouldHandle()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var wasCancelled = false;

        var command = new AsyncRelayCommand(
            async () =>
            {
                await Task.Delay(50);
                cts.Token.ThrowIfCancellationRequested();
                await Task.Delay(50);
            },
            ex =>
            {
                if (ex is OperationCanceledException)
                    wasCancelled = true;
            }
        );

        // Act
        command.Execute(null);
        await Task.Delay(25);
        cts.Cancel();
        await Task.Delay(100);

        // Assert
        wasCancelled.Should().BeTrue();
    }

    #endregion

    #region Performance Tests

    [Fact]
    public async Task Performance_ManySequentialExecutions_ShouldComplete()
    {
        // Arrange
        var executionCount = 0;
        var command = new AsyncRelayCommand(
            async () =>
            {
                await Task.Delay(5);
                executionCount++;
            },
            ex => { }
        );

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        for (int i = 0; i < 10; i++)
        {
            command.Execute(null);
            await Task.Delay(20); // Wait for completion
        }

        stopwatch.Stop();

        // Assert
        executionCount.Should().Be(10);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(500);
    }

    #endregion
}
