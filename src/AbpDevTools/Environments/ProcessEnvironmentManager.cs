﻿using AbpDevTools.Configuration;
using CliFx.Exceptions;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Unidecode.NET;

namespace AbpDevTools.Environments;
public class ProcessEnvironmentManager : IProcessEnvironmentManager
{
    private static Dictionary<string, Func<string, string>> replacements = new Dictionary<string, Func<string, string>>()
    {
        { "{Today}", (_) => DateTime.Today.ToString("yyyyMMdd") },
        { "{AppName}", FindAppName }
    };

    public void SetEnvironment(string environment, string directory)
    {
        var options = EnvironmentConfiguration.GetOptions();

        if (!options.TryGetValue(environment, out var env))
        {
            var environments = string.Join('\n', options.Keys.Select(x => "\t- " + x));
            throw new CommandException("Environment not found! Check environments by 'abpdev env config' command.\nAvailable environments:\n" + environments);
        }

        foreach (var variable in env.Variables)
        {
            Environment.SetEnvironmentVariable(variable.Key, PrepareValue(variable.Value, directory), EnvironmentVariableTarget.Process);
        }
    }

    public void SetEnvironmentForProcess(string environment, ProcessStartInfo process)
    {
        var options = EnvironmentConfiguration.GetOptions();

        if (!options.TryGetValue(environment, out var env))
        {
            var environments = string.Join('\n', options.Keys.Select(x => "\t- " + x));
            throw new CommandException("Environment not found! Check environments by 'abpdev env config' command.\nAvailable environments:\n" + environments);
        }

        foreach (var variable in env.Variables)
        {
            process.EnvironmentVariables[variable.Key] = PrepareValue(variable.Value, process.WorkingDirectory);
        }
    }

    protected virtual string PrepareValue(string value, string directory = null)
    {
        var finalResult = value;
        foreach (var item in replacements)
        {
            finalResult = finalResult.Replace(item.Key, item.Value(directory));
        }

        return finalResult;
    }

    private static string FindAppName(string directory)
    {
        var dir = directory ?? Directory.GetCurrentDirectory();

        var folderName = new DirectoryInfo(dir).Name;
        if (folderName.Contains("."))
        {
            var appName = folderName.Split('.').First();

            dir = dir.Replace(folderName, appName);
        }

        var normalized = Regex.Replace(dir.Unidecode().ToLowerInvariant(), @"[^a-z0-9]", string.Empty);

        var result = TruncateStart(normalized, 116);

        return result;
    }

    private static string TruncateStart(string value, int maximumLength)
    {
        if (value.Length <= maximumLength)
        {
            return value;
        }

        return value.Substring(value.Length - maximumLength);
    }
}
