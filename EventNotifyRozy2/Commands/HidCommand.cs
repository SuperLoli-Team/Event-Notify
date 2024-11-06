using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventNotifyRozy2
{

    namespace EventNotifyRozy2
    {
        [CommandHandler(typeof(RemoteAdminCommandHandler))]
        public class InfWinCommand : ICommand
        {
            public string Command => "infhid";
            public string[] Aliases => new string[] { };
            public string Description => "turned on/off of infinite MicroHid.";

            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
            {
                if (!EventPugin.plugin.Config.HidCodeActive)
                {
                    response = "This Command are disabled on Config";
                    return false;
                }
                var plugin = EventPugin.Instance;

                plugin.HidActive = !plugin.HidActive;

                if (plugin.HidActive)
                {
                    Exiled.Events.Handlers.Player.UsingMicroHIDEnergy += plugin.OnUsingMicroHIDEnergy;
                    response = "infinite MicroHid are turned on!";
                }
                else
                {
                    Exiled.Events.Handlers.Player.UsingMicroHIDEnergy -= plugin.OnUsingMicroHIDEnergy;
                    response = "infinite MicroHid are turned off!";
                }

                return true;
            }
        }
    }
}