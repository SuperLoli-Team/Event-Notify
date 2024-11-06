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
        public class TeslaCommand : ICommand
        {
            public string Command => "tesla";
            public string[] Aliases => new string[] { };
            public string Description => "Turned on/off of Tesla Gates";

            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
            {
                var plugin = EventPugin.Instance;

                plugin.TeslaActive = !plugin.TeslaActive;

                if (plugin.TeslaActive)
                {
                    Exiled.Events.Handlers.Player.TriggeringTesla += plugin.OnTriggeringTesla;
                    response = "Tesla Gates are Turned off";
                }
                else
                {
                    Exiled.Events.Handlers.Player.TriggeringTesla -= plugin.OnTriggeringTesla;
                    response = "Tesla Gates are Turned on!";
                }

                return true;
            }
        }
    }
}