﻿using AbpDevTools.Commands;
using AbpDevTools.Environments;
using AbpDevTools.Notifications;
using AbpDevTools.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;

namespace AbpDevTools;

public class Program
{
    public static async Task<int> Main() =>
        await new CliApplicationBuilder()
            .SetExecutableName("abpdev")
            .SetDescription("A set of tools to make development with ABP easier.")
            .SetTitle("Abp Dev Tools")
            .BuildServices()
            .Build()
            .RunAsync();
}

public static class Startup
{
    public static CliApplicationBuilder BuildServices(this CliApplicationBuilder builder)
    {
        var services = new ServiceCollection();

        var commands = new Type[]
        {
            typeof(BuildCommand),
            typeof(ConfigurationClearCommand),
            typeof(ReplacementConfigClearCommand),
            typeof(EnvironmentAppConfigClearCommand),
            typeof(RunConfigClearCommand),
            typeof(ReplaceConfigurationCommand),
            typeof(EnvironmentAppConfigurationCommand),
            typeof(RunConfigurationCommand),
            typeof(CleanConfigurationCommand),
            typeof(ConfigCommand),
            typeof(DisableNotificationsCommand),
            typeof(EnableNotificationsCommand),
            typeof(EnvironmentAppCommand),
            typeof(EnvironmentAppStartCommand),
            typeof(EnvironmentAppStopCommand),
            typeof(LogsClearCommand),
            typeof(LogsCommand),
            typeof(MigrateCommand),
            typeof(ReplaceCommand),
            typeof(RunCommand),
            typeof(EnvironmentCommand),
            typeof(EnvironmentConfigurationCommand),
            typeof(AbpBundleCommand),
            typeof(TestCommand),
            typeof(UpdateCheckCommand),
            typeof(CleanCommand)
        };

        foreach (var commandType in commands)
        {
            if (!commandType.IsAbstract && typeof(ICommand).IsAssignableFrom(commandType))
            {
                builder.AddCommand(commandType);
                services.AddSingleton(commandType);
            }
        }

        services.AddTransient<IProcessEnvironmentManager, ProcessEnvironmentManager>();
        services.AddTransient<UpdateChecker>();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            services.AddTransient<INotificationManager, WindowsNotificationManager>();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            services.AddTransient<INotificationManager, MacCatalystNotificationManager>();
        }
        else
        {
            services.AddTransient<INotificationManager, DefaultNotificationManager>();
        }

        var serviceProvider = services.BuildServiceProvider();

        return builder.UseTypeActivator(serviceProvider);
    }
}
