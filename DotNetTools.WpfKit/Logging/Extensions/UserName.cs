#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <               DotNet WPF Tool Kit             |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: DotNetTools.Wpfkit                                                *
*            Filename: UserName.cs                                                       *
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

using DotNetTools.Wpfkit.Logging.Enrichers;
using Serilog;
using Serilog.Configuration;

namespace DotNetTools.Wpfkit.Logging.Extensions;

/// <summary>
/// Provides extension methods for enriching Serilog logs with user name information.
/// </summary>
public static class UserName
{
    #region WithUserName

    /// <summary>
    /// Enriches log events with the current user's name using the <see cref="UserNameEnricher"/>
    /// </summary>
    /// <param name="enrich">The logger enrichment configuration to extend.</param>
    /// <returns>The logger configuration for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enrich"/> is null.</exception>
    public static LoggerConfiguration WithUserName(this LoggerEnrichmentConfiguration enrich)
    {
        return enrich == null ? throw new ArgumentNullException(nameof(enrich)) : enrich.With<UserNameEnricher>();
    }

    #endregion
}
