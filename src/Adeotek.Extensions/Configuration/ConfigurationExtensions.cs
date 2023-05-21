using Microsoft.Extensions.Configuration;

namespace Adeotek.Extensions.Configuration;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Reads the named connection string from the "ConnectionStrings" section within appsettings.json,
    /// with ability to substitute a user environment variable for the
    /// value stored in the settings JSON file.
    /// </summary>
    /// <param name="configuration">Configuration instance.</param>
    /// <param name="connectionName">Name of the connection key e.g. 'DefaultConnection'</param>
    /// <param name="envVarName">Environment variable name</param>
    /// <param name="allowEmpty">If false, an InvalidOperationException will be thrown
    /// when the connection string is empty or missing, otherwise an empty string will be returned</param>
    /// <returns>Connection string with environment variable substituting for server if present.</returns>
    public static string GetEnvConnectionString(this IConfiguration configuration, string connectionName, string envVarName, bool allowEmpty = false)
    {
        var connectionString = GetEnvironmentVariable(envVarName);
        if (!string.IsNullOrEmpty(connectionString))
        {
            return connectionString;
        }
        
        connectionString = configuration.GetConnectionString(connectionName);
        if (string.IsNullOrEmpty(connectionString) && !allowEmpty)
        {
            throw new InvalidOperationException($"Cannot find a connection string in appsettings.json matching: ConnectionStrings:{connectionName}");
        }

        return connectionString ?? "";
    }
    
    /// <summary>
    /// Return the value of a environment variable if present.
    /// Looks in priority order of Process, User, Machine.
    /// </summary>
    /// <returns>Value of environment variable.</returns>
    private static string? GetEnvironmentVariable(string name)
    {
        var value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        if (string.IsNullOrEmpty(value))
        {
            value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);
        }

        if (string.IsNullOrEmpty(value))
        {
            value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
        }
        
        return value;
    }
}