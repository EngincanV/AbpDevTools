﻿using System.Diagnostics;
using AbpDevTools.Configuration;

namespace AbpDevTools.Notifications;
public class MacCatalystNotificationManager : INotificationManager
{
    public async Task SendAsync(string title, string message = null, string icon = null)
    {
        if(!NotificationConfiguration.GetOptions().Enabled){
            return;
        }

        var process = Process.Start("osascript", $"-e \"display notification \\\"{message}\\\" with title \\\"{title}\\\"\"");

        await process.WaitForExitAsync();
    }
}
