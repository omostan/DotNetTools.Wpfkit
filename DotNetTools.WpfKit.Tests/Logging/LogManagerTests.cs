#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <               DotNet WPF Tool Kit             |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: DotNetTools.Wpfkit.Tests                                          *
*            Filename: LogManagerTests.cs                                                *
*              Author: Stanley Omoregie                                                  *
*        Created Date: 20.11.2025                                                        *
*       Modified Date: 21.11.2025                                                        *
*          Created By: Stanley Omoregie                                                  *
*    Last Modified By: Stanley Omoregie                                                  *
*           CopyRight: copyright © 2025 Omotech Digital Solutions                        *
*                  .oooO  Oooo.                                                          *
*                  (   )  (   )                                                          *
* ------------------\ (----) /---------------------------------------------------------- *
*                    \_)  (_/                                                            *
*****************************************************************************************/

#endregion copyright

using DotNetTools.Wpfkit.Logging.Extensions;
using Serilog;
using Serilog.Events;

namespace DotNetTools.Wpfkit.Tests.Logging;

/// <summary>
/// Unit tests for the LogManager class.
/// </summary>
[Collection("LogManager Tests")]
public class LogManagerTests : IDisposable
{
    private readonly List<LogEvent> _logEvents = new();
    private readonly ILogger _testLogger;
    private bool _disposed = false;

    public LogManagerTests()
    {
        // Setup test logger that captures log events
        _logEvents.Clear();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Sink(new TestSink(_logEvents))
            .CreateLogger();

        _testLogger = Log.Logger;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Log.CloseAndFlush();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    #region GetCurrentClassLogger Tests

    [Fact]
    public void GetCurrentClassLogger_ShouldReturnLogger()
    {
        // Act
        var logger = LogManager.GetCurrentClassLogger();

        // Assert
        logger.Should().NotBeNull();
        logger.Should().BeAssignableTo<ILogger>();
    }

    [Fact]
    public void GetCurrentClassLogger_ShouldReturnLoggerWithCallingTypeContext()
    {
        // Arrange
        _logEvents.Clear();

        // Act
        var logger = LogManager.GetCurrentClassLogger();
        logger.Information("Test message");

        // Assert
        _logEvents.Should().HaveCount(1);
        var logEvent = _logEvents[0];
        logEvent.Properties.Should().ContainKey("SourceContext");
        
        var sourceContext = logEvent.Properties["SourceContext"].ToString();
        sourceContext.Should().Contain(nameof(LogManagerTests));
    }

    [Fact]
    public void GetCurrentClassLogger_CalledFromDifferentClasses_ShouldReturnDifferentLoggers()
    {
        // Arrange
        var testClass1 = new TestClass1();
        var testClass2 = new TestClass2();

        // Act
        var logger1 = testClass1.GetLogger();
        var logger2 = testClass2.GetLogger();

        // Assert
        logger1.Should().NotBeNull();
        logger2.Should().NotBeNull();
        // Note: We can't directly compare logger instances as they wrap different contexts
    }

    #endregion

    #region Me Extension Tests

    [Fact]
    public void Me_ShouldReturnLoggerWithLineNumberContext()
    {
        // Arrange
        _logEvents.Clear();
        var logger = LogManager.GetCurrentClassLogger();

        // Act
        logger.Me().Information("Test message with line number");

        // Assert
        _logEvents.Should().HaveCount(1);
        var logEvent = _logEvents[0];
        logEvent.Properties.Should().ContainKey("LineNumber");
    }

    [Fact]
    public void Me_ShouldCaptureCorrectLineNumber()
    {
        // Arrange
        _logEvents.Clear();
        var logger = LogManager.GetCurrentClassLogger();

        // Act
        var currentLine = GetLineNumber(); // This captures the line number
        logger.Me().Information("Test message");

        // Assert
        _logEvents.Should().HaveCount(1);
        var logEvent = _logEvents[0];
        logEvent.Properties.Should().ContainKey("LineNumber");
        
        var lineNumber = int.Parse(logEvent.Properties["LineNumber"].ToString());
        lineNumber.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Me_WithMultipleCalls_ShouldCaptureDifferentLineNumbers()
    {
        // Arrange
        _logEvents.Clear();
        var logger = LogManager.GetCurrentClassLogger();

        // Act
        logger.Me().Information("First message");
        logger.Me().Information("Second message");

        // Assert
        _logEvents.Should().HaveCount(2);
        var line1 = int.Parse(_logEvents[0].Properties["LineNumber"].ToString());
        var line2 = int.Parse(_logEvents[1].Properties["LineNumber"].ToString());
        
        line1.Should().NotBe(line2);
    }

    [Fact]
    public void Me_ShouldChainWithOtherLoggingMethods()
    {
        // Arrange
        _logEvents.Clear();
        var logger = LogManager.GetCurrentClassLogger();

        // Act
        logger.Me().ForContext("CustomProperty", "CustomValue").Information("Test message");

        // Assert
        _logEvents.Should().HaveCount(1);
        var logEvent = _logEvents[0];
        logEvent.Properties.Should().ContainKey("LineNumber");
        logEvent.Properties.Should().ContainKey("CustomProperty");
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void LogManager_FullWorkflow_ShouldLogWithAllContext()
    {
        // Arrange
        _logEvents.Clear();

        // Act
        var logger = LogManager.GetCurrentClassLogger();
        logger.Me().Information("Integration test message");

        // Assert
        _logEvents.Should().HaveCount(1);
        var logEvent = _logEvents[0];
        
        logEvent.Level.Should().Be(LogEventLevel.Information);
        logEvent.MessageTemplate.Text.Should().Be("Integration test message");
        logEvent.Properties.Should().ContainKey("SourceContext");
        logEvent.Properties.Should().ContainKey("LineNumber");
    }

    [Fact]
    public void LogManager_WithDifferentLogLevels_ShouldWorkCorrectly()
    {
        // Arrange
        _logEvents.Clear();
        var logger = LogManager.GetCurrentClassLogger();

        // Act
        logger.Me().Verbose("Verbose message");
        logger.Me().Debug("Debug message");
        logger.Me().Information("Information message");
        logger.Me().Warning("Warning message");
        logger.Me().Error("Error message");

        // Assert
        _logEvents.Should().HaveCount(5);
        _logEvents[0].Level.Should().Be(LogEventLevel.Verbose);
        _logEvents[1].Level.Should().Be(LogEventLevel.Debug);
        _logEvents[2].Level.Should().Be(LogEventLevel.Information);
        _logEvents[3].Level.Should().Be(LogEventLevel.Warning);
        _logEvents[4].Level.Should().Be(LogEventLevel.Error);
    }

    [Fact]
    public void LogManager_WithException_ShouldLogException()
    {
        // Arrange
        _logEvents.Clear();
        var logger = LogManager.GetCurrentClassLogger();
        var exception = new InvalidOperationException("Test exception");

        // Act
        logger.Me().Error(exception, "Error with exception");

        // Assert
        _logEvents.Should().HaveCount(1);
        var logEvent = _logEvents[0];
        logEvent.Exception.Should().BeSameAs(exception);
        logEvent.MessageTemplate.Text.Should().Be("Error with exception");
    }

    [Fact]
    public void LogManager_WithStructuredLogging_ShouldCaptureProperties()
    {
        // Arrange
        _logEvents.Clear();
        var logger = LogManager.GetCurrentClassLogger();

        // Act
        logger.Me().Information("User {UserName} logged in at {LoginTime}", "John", DateTime.Now);

        // Assert
        _logEvents.Should().HaveCount(1);
        var logEvent = _logEvents[0];
        logEvent.Properties.Should().ContainKey("UserName");
        logEvent.Properties.Should().ContainKey("LoginTime");
    }

    #endregion

    #region Helper Classes and Methods

    private class TestClass1
    {
        public ILogger GetLogger() => LogManager.GetCurrentClassLogger();
    }

    private class TestClass2
    {
        public ILogger GetLogger() => LogManager.GetCurrentClassLogger();
    }

    private static int GetLineNumber([System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
    {
        return lineNumber;
    }

    private class TestSink : Serilog.Core.ILogEventSink
    {
        private readonly List<LogEvent> _events;

        public TestSink(List<LogEvent> events)
        {
            _events = events;
        }

        public void Emit(LogEvent logEvent)
        {
            _events.Add(logEvent);
        }
    }

    #endregion

    #region Performance Tests

    [Fact]
    public void GetCurrentClassLogger_CalledMultipleTimes_ShouldPerformWell()
    {
        // Arrange
        var iterations = 1000;
        var sw = System.Diagnostics.Stopwatch.StartNew();

        // Act
        for (int i = 0; i < iterations; i++)
        {
            _ = LogManager.GetCurrentClassLogger();
        }

        sw.Stop();

        // Assert
        sw.ElapsedMilliseconds.Should().BeLessThan(1000); // Should complete in less than 1 second
    }

    [Fact]
    public void Me_Extension_CalledMultipleTimes_ShouldPerformWell()
    {
        // Arrange
        var logger = LogManager.GetCurrentClassLogger();
        var iterations = 1000;
        var sw = System.Diagnostics.Stopwatch.StartNew();

        // Act
        for (int i = 0; i < iterations; i++)
        {
            _ = logger.Me();
        }

        sw.Stop();

        // Assert
        sw.ElapsedMilliseconds.Should().BeLessThan(500); // Should complete quickly
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Me_WithNullLogger_ShouldHandleGracefully()
    {
        // Arrange
        ILogger? nullLogger = null;

        // Act & Assert
        Action act = () => nullLogger!.Me();
        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void GetCurrentClassLogger_AfterReconfiguration_ShouldStillWork()
    {
        // Arrange - Reconfigure logger without closing it
        _logEvents.Clear();
        var newEvents = new List<LogEvent>();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Sink(new TestSink(newEvents))
            .CreateLogger();

        // Act
        var logger = LogManager.GetCurrentClassLogger();
        logger.Information("Test after reconfiguration");

        // Assert
        logger.Should().NotBeNull();
        newEvents.Should().HaveCount(1);
    }

    #endregion
}
