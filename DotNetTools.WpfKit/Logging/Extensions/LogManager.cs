#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <               DotNet WPF Tool Kit             |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: DotNetTools.Wpfkit                                                *
*            Filename: LogManager.cs                                                     *
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

using Serilog;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DotNetTools.Wpfkit.Logging.Extensions;

/// <summary>
/// Provides logging management utilities for creating and configuring Serilog loggers.
/// </summary>
public static class LogManager
{
    #region GetCurrentClassLogger

    /// <summary>
    /// Gets the logger for the current class. Ensure this is set to a static field on the class.
    /// </summary>
    /// <ref>https://github.com/serilog/serilog/issues/886#issuecomment-265063611</ref>
    /// <returns>An instance of <see cref="T:Serilog.ILogger" /></returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static ILogger GetCurrentClassLogger()
    {
        return Log.ForContext(new StackFrame(1, false).GetMethod()?.DeclaringType!);
    }

    #endregion

    #region Me

    public static ILogger Me(this ILogger logger,
        //[CallerMemberName] string memberName = "",
        //[CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        return logger
            //.ForContext("MemberName", memberName)
            //.ForContext("FilePath", sourceFilePath)
            .ForContext("LineNumber", sourceLineNumber);
    }

    #endregion
}
