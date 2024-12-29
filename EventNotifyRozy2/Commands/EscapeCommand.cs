using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventNotifyRozy2.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EscapeCommand : ICommand
    {
        public string Command => "escape";
        public string[] Aliases => new string[] { "e" };
        public string Description => "On/Off escape.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!EventPlugin.plugin.Config.EscapeCodeActive)
            {
                response = "This command are off in Config";
                return false;
            }
            var plugin = EventPlugin.Instance;

            plugin.EscapeActive = !plugin.EscapeActive;

            if (plugin.EscapeActive)
            {
                Exiled.Events.Handlers.Player.Escaping += plugin.OnEscaping;
                response = "Escape are turned off!";
            }
            else
            {
                Exiled.Events.Handlers.Player.Escaping -= plugin.OnEscaping;
                response = "Escape are turned on!";
            }

            return true;
        }
    }
}