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

public class AsyncRelayCommand(Func<Task> callback, Action<Exception> onException) : AsyncCommandBase(onException)
{
    protected override async Task ExecuteAsync(object parameter)
    {
        await callback();
    }
}
