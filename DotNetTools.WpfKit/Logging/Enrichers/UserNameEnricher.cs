#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <               DotNet WPF Tool Kit             |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: DotNetTools.Wpfkit                                                *
*            Filename: UserNameEnricher.cs                                               *
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

using Serilog.Core;
using Serilog.Events;

namespace DotNetTools.Wpfkit.Logging.Enrichers;

public class UserNameEnricher : ILogEventEnricher
{
    private LogEventProperty? _cachedProperty;
    private const string PropertyName = "UserName";

    /// <summary>
    /// Enriches the log event by adding the username as a property.
    /// </summary>
    /// <ref>
    /// https://www.ctrlaltdan.com/2018/08/14/custom-serilog-enrichers/
    /// </ref>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));
    }

    private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
    {
        if (_cachedProperty != null)
        {
            return _cachedProperty;
        }

        _cachedProperty = CreateProperty(propertyFactory);
        return _cachedProperty;
    }

    private static LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
    {
        string value = Environment.GetEnvironmentVariable("UserName")?.ToLower() ?? string.Empty;
        return propertyFactory.CreateProperty(PropertyName, value);
    }
}
