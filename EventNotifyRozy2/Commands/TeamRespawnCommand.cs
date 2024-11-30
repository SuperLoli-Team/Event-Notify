using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventNotifyRozy2.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TeamRespawnCommand : ICommand
    {
        public string Command => "teamresp";
        public string[] Aliases => new string[] { "respawn" };
        public string Description => "turned on/off respawn team";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!EventPlugin.Instance.Config.RespawnCodeActive)
            {
                response = "The command is unavailable as it is disabled in the configuration";
                return false;
            }
            var plugin = EventPlugin.Instance;

            plugin.TeamrespawnActive = !plugin.TeamrespawnActive;

            if (plugin.WindowsActive)
            {
                Exiled.Events.Handlers.Server.RespawningTeam += plugin.OnRespawnedTeam;
                response = "Respawn Team are turned on!";
            }
            else
            {
                Exiled.Events.Handlers.Server.RespawningTeam -= plugin.OnRespawnedTeam;
                response = "Respawn Team are turned off!";
            }

            return true;
        }
    }
}