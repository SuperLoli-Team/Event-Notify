using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventNotifyRozy2.Commands
{
    using CommandSystem;
    using EventNotifyRozy2;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static PlayerRoles.Spectating.SpectatableModuleBase;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EventAdminCommand : ICommand, IUsageProvider
    {
        public bool SanitizeResponse => false;
        public string Command => "removehelper";

        public string[] Aliases => new string[] { "rh" };

        public string Description => "Removes a player from the helpers list by their ID.";
        public string[] Usage => new string[] { "playerID" };
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 1)
            {
                response = "Usage: removehelper <playerID>";
                return false;
            }

            string playerId = arguments.At(0).Trim();
            Player target = Player.Get(playerId);

            if (target == null)
            {
                response = "Player with that ID was not found.";
                return false;
            }

            if (!EventPlugin.EventHelpers.Contains(target.Id))
            {
                response = $"Player {target.Nickname} is not a helper.";
                return false;
            }

            EventPlugin.EventHelpers.Remove(target.Id);
            response = string.Format("Player has been removed from the helpers list.", target.Nickname);
            target.Broadcast(5, "<color=yellow>You were removed from the Helper</color>");
            return true;
        }
    }
}