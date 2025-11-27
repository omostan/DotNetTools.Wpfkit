#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <               DotNet WPF Tool Kit             |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: DotNetTools.Wpfkit                                                *
*            Filename: ActionCommand.cs                                                  *
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

using System.Windows.Input;

namespace DotNetTools.Wpfkit.Commands;

/// <summary>
/// Represents a command that executes an action with an optional condition.
/// This command implements <see cref="ICommand"/> and integrates with WPF's command infrastructure.
/// </summary>
/// <param name="action">The action to execute when the command is invoked. Must not be null.</param>
/// <param name="predicate">An optional predicate that determines whether the command can execute. If null, the command can always execute.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is null.</exception>
public class ActionCommand(Action<object?> action, Predicate<object?>? predicate = null) : ICommand
{
    #region Fields

    private readonly Action<object?> _action = action ?? throw new ArgumentNullException(nameof(action), "You must specify an Action<T>.");

    private readonly Predicate<object?>? _predicate = predicate;

    #endregion

    #region Implementation of ICommand

    /// <summary>
    /// Determines whether the command can execute.
    /// Returns true if no predicate is provided, or if the predicate evaluates to true.
    /// Returns false if any exception occurs during predicate evaluation.
    /// </summary>
    public bool CanExecute(object? parameter)
    {
        // If no predicate is provided, command can always execute
        if (_predicate == null)
        {
            return true;
        }

        // Safely evaluate the predicate with exception handling
        try
        {
            return _predicate(parameter);
        }
        catch
        {
            // Return false on any exception to prevent crashes
            // This handles null references, invalid casts, etc.
            return false;
        }
    }

    /// <summary>
    /// Executes the command action with the provided parameter.
    /// </summary>
    public void Execute(object? parameter)
    {
        _action(parameter);
    }

    /// <summary>
    /// Occurs when changes occur that affect whether the command should execute.
    /// Integrates with WPF's CommandManager for automatic UI updates.
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    #endregion Implementation of ICommand
}
