using System;
using System.IO;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using HarmonyLib;
using System.Reflection;
using RemoteAdmin;
using EventNotifyRozy2;

namespace EventNotifyRozy2.API
{
    public class CommandPatch
    {
        [HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery))]
        public class AdminCommandPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref string q, CommandSender sender)
            {
                string adminNickname = sender is CommandSender commandSender ? commandSender.Nickname : "Unknow administrator";
                Player ply = (Player)Player.List.Where(player => player.Nickname == EventPugin.EventMaster);
                Player player = Player.List.FirstOrDefault(p => p.Nickname.Equals(adminNickname, StringComparison.OrdinalIgnoreCase));

                if (player != null && !EventPugin.EventHelpers.Any(helper => helper.Equals(player.Id)) && player.Nickname != EventPugin.EventMaster)
                {

                    string webhookMessage = $"{adminNickname} used a command {q} on the Event";
                    WebhookNotifier.SendAdminCommandNotification(webhookMessage);
                    if (ply != null)
                    {
                        ply.Broadcast(10, webhookMessage, Broadcast.BroadcastFlags.AdminChat);
                    }
                }
                    return true;
                }
            }
        }
    }