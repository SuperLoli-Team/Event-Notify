using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Interactables.Interobjects.DoorUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventNotifyRozy2
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class DClassDoorCommand : ICommand, IUsageProvider
    {
        public string Command { get; } = "dclassdoor";
        public string[] Aliases { get; } = new string[] { "dcd" };
        public string Description { get; } = "Doors in D-Block";

        public string[] Usage => new string[] { "lock", "unlock", "open", "close" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!EventPugin.plugin.Config.DCDCodeActive)
            {
                response = "This Command are off in Config";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Usage: doord lock/unlock/open/close";
                return false;
            }

            switch (arguments.At(0).ToLower())
            {
                case "lock":
                    LockDClassDoors();
                    response = "Doors in D-Block are locked";
                    return true;
                case "unlock":
                    UnlockDClassDoors();
                    response = "Doors in D-Block are unlocked";
                    return true;
                case "open":
                    OpenDClassDoors();
                    response = "Doors in D-Block are opened";
                    return true;
                case "close":
                    CloseDClassDoors();
                    response = "Doors in D-Block are closed";
                    return true;
                default:
                    response = "Usage: dcd lock/unlock/open/close";
                    return false;
            }
        }

        private void LockDClassDoors()
        {
            foreach (var door in Door.List.Where(d => d.Type == DoorType.PrisonDoor))
            {
                door.ChangeLock(DoorLockType.AdminCommand);
            }
        }

        private void UnlockDClassDoors()
        {
            foreach (var door in Door.List.Where(d => d.Type == DoorType.PrisonDoor))
            {
                door.ChangeLock(DoorLockType.AdminCommand);
            }
        }


        private void OpenDClassDoors()
        {
            foreach (var door in Door.List.Where(d => d.Type == DoorType.PrisonDoor))
            {
                door.IsOpen = true;
            }
        }

        private void CloseDClassDoors()
        {
            foreach (var door in Door.List.Where(d => d.Type == DoorType.PrisonDoor))
            {
                door.IsOpen = false;
            }
        }
    }
}
