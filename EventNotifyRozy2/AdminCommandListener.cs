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
    public class AdminCommandListener
    {
        [HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery))]
        public class AdminCommandPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref string q, CommandSender sender)
            {
                try
                {
                    if (!q.StartsWith("$") && !q.StartsWith("@"))
                    {

                        string adminNickname = sender is CommandSender commandSender
                            ? commandSender.Nickname
                            : "Unknown administrator";
                        Player ply = (Player)Player.List.FirstOrDefault(player =>
                            player.Nickname == EventPlugin.EventMaster);
                        Player player = Player.List.FirstOrDefault(p =>
                            p.Nickname.Equals(adminNickname, StringComparison.OrdinalIgnoreCase));
                        if (EventPlugin.EventMode)
                        {
                            if (player != null && !EventPlugin.EventHelpers.Any(helper => helper.Equals(player.Id)) &&
                                player.Nickname != EventPlugin.EventMaster)
                            {
                                var time = DateTime.Now;
                                string webhookMessage = $"[{time}]\n{adminNickname} used a command {q} on the Event {EventPlugin.EventName}";
                                string Message = $"<b><color=yellow>[{time}]</color>\n<color=#FF69B4>{adminNickname}</color> used a command <color=red>{q}</color></b>";
                                Send(webhookMessage);
                                if (ply != null)
                                {
                                    ply.Broadcast(5, Message);
                                }
                            }
                            return true;
                        }
                        return true;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error($"Error in AdminCommandPatch: {ex}");
                    return true;
                }
            }
            private static async void Send(string webhookMessage)
            {
                await WebhookNotifier.SendAdminCommandNotification(webhookMessage);
            }
        }
    }
}