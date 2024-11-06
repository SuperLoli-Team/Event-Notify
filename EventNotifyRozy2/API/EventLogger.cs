using Exiled.API.Features;
using System;
using System.IO;

namespace EventNotifyRozy2.API
{
    public static class EventLogger
    {
        public static void LogAdminCommand(string adminName, string command)
        {
            string logMessage = $"{adminName} used a command: {command}";
            var time = DateTime.Now;
            Log.Info(logMessage);

            // Запись в файл (необязательно)
            string logPath = Path.Combine(Paths.Log, "AdminAbuse.log");
            File.AppendAllText(logPath, $"({time}) {logMessage}\n");
        }
    }
}