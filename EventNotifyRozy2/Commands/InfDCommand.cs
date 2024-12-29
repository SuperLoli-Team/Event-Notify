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
    public class InfDCommand : ICommand
    {
        public string Command => "infd";
        public string[] Aliases => new string[] { "infinitydoors" };
        public string Description => "turned on/off godmode of doors";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!EventPlugin.plugin.Config.InfDCodeActive)
            {
                response = "This command are disabled on Config";
                return false;
            }
            var plugin = EventPlugin.Instance;

            plugin.DoorsActive = !plugin.DoorsActive;

            if (plugin.DoorsActive)
            {
                Exiled.Events.Handlers.Player.DamagingDoor += plugin.OnDamagingDoor;
                response = "godmode of doors are turned on!";
            }
            else
            {
                Exiled.Events.Handlers.Player.DamagingDoor -= plugin.OnDamagingDoor;
                response = "godmode of doors are turned off!";
            }

            return true;
        }
    }
}