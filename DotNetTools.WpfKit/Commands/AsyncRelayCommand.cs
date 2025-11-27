#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <               DotNet WPF Tool Kit             |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: DotNetTools.Wpfkit                                                *
*            Filename: AsyncRelayCommand.cs                                              *
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

namespace DotNetTools.Wpfkit.Commands;

/// <summary>
/// Concrete implementation of async command for quick usage in MVVM applications.
/// Provides built-in exception handling and concurrent execution prevention.
/// </summary>
public class AsyncRelayCommand(Func<Task> callback, Action<Exception> onException) : AsyncCommandBase(onException)
{
    #region Fields

    private readonly Func<Task> _callback = callback ?? throw new ArgumentNullException(nameof(callback), "Callback cannot be null.");

    #endregion

    #region ExecuteAsync

    /// <summary>
    /// Executes the async callback provided in the constructor.
    /// </summary>
    /// <param name="parameter">The command parameter (not used by this implementation).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override async Task ExecuteAsync(object parameter)
    {
        await _callback();
    }

    #endregion
}
