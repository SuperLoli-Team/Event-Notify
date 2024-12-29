using CommandSystem;
using EventNotifyRozy2;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventNotifyRozy2.Commands
{

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EventPomCommand : ICommand, IUsageProvider
    {
        public bool SanitizeResponse => false;

        public string Command => "addhelper";

        public string[] Aliases => new string[] { "ah" };

        public string Description => "Adds a player to the helpers list by their ID.";

        public string[] Usage => new string[] { "playerID" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 1)
            {
                response = "Usage: addhelper <playerID>";
                return false;
            }

            string playerId = arguments.At(0).Trim();
            Player target = Player.Get(playerId);

            if (target == null)
            {
                response = "Player with that ID was not found.";
                return false;
            }

            if (EventPlugin.EventHelpers.Contains(target.Id))
            {
                response = $"Player {target.Nickname} is already a helper.";
                return false;
            }

            EventPlugin.EventHelpers.Add(target.Id);
            response = string.Format("Player has been added to the helpers list.");
            target.Broadcast(5, "<color=yellow>You've been assigned as an Helper</color>");
            return true;
        }
    }
}