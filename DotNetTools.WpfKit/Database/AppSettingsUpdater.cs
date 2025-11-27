#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <               DotNet WPF Tool Kit             |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: DotNetTools.Wpfkit                                                *
*            Filename: AppSettingsUpdater.cs                                             *
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
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using TraceTool;

namespace DotNetTools.Wpfkit.Database;

/// <summary>
/// Provides utility methods for updating application settings in the appsettings.json file.
/// </summary>
public static class AppSettingsUpdater
{
    #region Fields

    /// <summary>
    /// Gets the logger for the current class
    /// </summary>
    private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

    #endregion Fields

    #region UpdateConnectionString

    /// <summary>
    /// Updates the connection string in the specified appsettings.json file.
    /// </summary>
    /// <usage>
    /// Make sue the variable to set the connection string in the appsettings is named "ConnectDatabase".
    /// </usage>
    /// <param name="connectionString">The new connection string to set.</param>
    public static void UpdateConnectionString(string connectionString)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return;
            }

            // Get the path to the appsettings.json file
            var baseDirectory = Path.GetDirectoryName(AppContext.BaseDirectory);

            // Ensure the base directory exists
            if (!Directory.Exists(baseDirectory))
            {
                Log.Me().Error("Base directory does not exist: {BaseDirectory}", baseDirectory);
                return;
            }

            // Combine the base directory with the appsettings.json filename
            var appsettings = Path.Combine(baseDirectory, "appsettings.json");

            // Trim the "Data Source=" prefix from the connection string
            var withoutDataSource = connectionString.Replace("Data Source=", string.Empty).TrimStart(' ', ';');

            // Read the existing appsettings.json file
            var jsonFile = File.ReadAllText(appsettings);

            // Parse the JSON content
            var jObject = JsonNode.Parse(jsonFile);

            // Update the ConnectDatabase property with the new connection string
            if (jObject != null)
            {
                jObject["ConnectDatabase"] = withoutDataSource;

                // Write the updated JSON back to the appsettings.json file
                File.WriteAllText(appsettings, jObject.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
            }

            // TTrace.Debug.Send($"jObject: {jObject}");
            Log.Me().Information("Connection string updated successfully in appsettings.json.");
        }
        catch (Exception ex)
        {
            Log.Me().Error("Failed to update connection string in appsettings.json: {ExceptionMessage}", ex.Message);
            TTrace.Debug.Send("Failed to update connection string in appsettings.json: ", ex.Message);
        }
    }

    #endregion UpdateConnectionString
}
