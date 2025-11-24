#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <               DotNet WPF Tool Kit             |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: DotNetTools.Wpfkit                                                *
*            Filename: CommandBase.cs                                                    *
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
/// Extends the ICommand and implements all basic commands.
/// </summary>
public abstract class CommandBase : ICommand
{
    /// <summary>
    /// Occurs when changes occur that affect whether the command should execute.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Determines whether the command can execute in its current
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns>true if this command can be executed; otherwise, false.</returns>
    public virtual bool CanExecute(object? parameter)
    {
        return true;
    }

    /// <summary>
    /// Defines the method to be called when the command is invoked.
    /// </summary>
    /// <param name="parameter"></param>
    public abstract void Execute(object? parameter);

    /// <summary>
    /// Invoked the command when the command can execute.
    /// </summary>
    protected void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
