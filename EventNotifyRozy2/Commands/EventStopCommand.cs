using CommandSystem;
using Exiled.Permissions.Extensions;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventNotifyRozy2.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EventStopCommand : ICommand
    {
        public bool SanitizeResponse => false;
        public string Command => "Eventstop";
        public string[] Aliases { get; } = { "Evstop" };
        public string Description => "Stop the event";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("rozy.command"))
            {
                if (!EventPlugin.plugin.Config.EventCodeActive)
                {
                    response = "The command is unavailable as it is disabled in the configuration.";
                    return false;
                }
                response = "You do not have the 'rozy.command' permission.";
                return false;
            }

            if (EventPlugin.EventMode || EventPlugin.EventPreparation)
            {
                EventPlugin.EventMode = false;
                EventPlugin.EventPreparation = false;
                Timing.KillCoroutines(EventPlugin.hintCoroutine);
                EventPlugin.time = new TimeSpan(0, 0, 0);
                response = "The event has been concluded.";
            }
            else
            {
                response = "No active events found to stop.";
            }

            return true;
        }
    }
}
