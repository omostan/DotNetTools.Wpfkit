#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <               DotNet WPF Tool Kit             |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: DotNetTools.Wpfkit                                                *
*            Filename: AsyncCommandBase.cs                                               *
*              Author: Stanley Omoregie                                                  *
*        Created Date: 24.11.2025                                                        *
*       Modified Date: 24.11.2025                                                        *
*          Created By: Stanley Omoregie                                                  *
*    Last Modified By: Stanley Omoregie                                                  *
*           CopyRight: copyright © 2025 Omotech Digital Solutions                        *
*                  .oooO  Oooo.                                                          *
*                  (   )  (   )                                                          *
* ------------------\ (----) /---------------------------------------------------------- *
*                    \_)  (_/                                                            *
*****************************************************************************************/

#endregion copyright

using TraceTool;

namespace DotNetTools.Wpfkit.Commands;

/// <summary>
/// Abstract base class for asynchronous command operations with automatic execution state management.
/// </summary>
public abstract class AsyncCommandBase(Action<Exception> onException) : CommandBase
{
    #region Properties

    /// <summary>
    /// An instance of the exception handler
    /// </summary>
    private readonly Action<Exception> _onException = onException ?? throw new ArgumentNullException(nameof(onException), "Exception handler cannot be null.");

    /// <summary>
    /// When implemented in a derived class, executes the command logic asynchronously.
    /// </summary>
    /// <param name="parameter">The parameter passed to the command.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task ExecuteAsync(object parameter);

    /// <summary>
    /// Gets the status of the caller method
    /// </summary>
    private bool _isExecuting;

    private bool IsExecuting
    {
        get => _isExecuting;
        set
        {
            _isExecuting = value;
            OnCanExecuteChanged();
        }
    }

    #endregion Properties

    #region CanExecute

    /// <summary>
    /// Determines whether the command can execute.
    /// Returns false while the command is executing to prevent concurrent execution.
    /// </summary>
    public override bool CanExecute(object? parameter)
    {
        return !IsExecuting;
    }

    #endregion CanExecute

    #region Execute

    /// <summary>
    /// Executes the command asynchronously, ensuring that only one execution occurs at a time.
    /// </summary>
    public override void Execute(object? parameter)
    {
        if (_isExecuting) return;
        IsExecuting = true;

        _ = ExecuteInternal(parameter);
    }

    #endregion Execute

    #region ExecuteInternal

    /// <summary>
    /// Executes the command logic asynchronously.
    /// Handles exceptions and updates the execution state.
    /// </summary>
    private async Task ExecuteInternal(object? parameter)
    {
        try
        {
            await ExecuteAsync(parameter!);
        }
        catch (Exception ex)
        {
            TTrace.Error.Send(ex.Message);
            _onException.Invoke(ex);
        }
        finally
        {
            IsExecuting = false;
        }
    }

    #endregion ExecuteInternal
}
