using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventNotifyRozy2.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class InfWinCommand : ICommand
    {
        public string Command => "infwin";
        public string[] Aliases => new string[] { };
        public string Description => "turned on/off godmode of windows";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!EventPlugin.plugin.Config.InfWCodeActive)
            {
                response = "This command are disabled on Config";
                return false;
            }
            var plugin = EventPlugin.Instance;

            plugin.WindowsActive = !plugin.WindowsActive;

            if (plugin.WindowsActive)
            {
                Exiled.Events.Handlers.Player.DamagingWindow += plugin.OnPlayerDamageWindow;
                response = "GodMode of windows are turned on!";
            }
            else
            {
                Exiled.Events.Handlers.Player.DamagingWindow -= plugin.OnPlayerDamageWindow;
                response = "GodMode of windows are turned off!";
            }

            return true;
        }
    }
}