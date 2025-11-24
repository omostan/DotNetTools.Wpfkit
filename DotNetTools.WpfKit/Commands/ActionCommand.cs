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

public class ActionCommand(Action<object> action, Predicate<object>? predicate = null) : ICommand
{
    private readonly Action<object> _action = action ?? throw new ArgumentNullException(nameof(action), "You must specify an Action<T>.");

    private readonly Predicate<object> _predicate = predicate!;

    #region Implementation of ICommand

    public bool CanExecute(object? parameter)
    {
        return parameter != null && _predicate(parameter);
    }

    public void Execute(object? parameter)
    {
        _action(parameter!);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    #endregion Implementation of ICommand
}
