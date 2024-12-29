using CommandSystem;
using EventNotifyRozy2;
using MEC;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;
using System.Linq;

namespace EventNotifyRozy2.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EventCommand : ICommand, IUsageProvider
    {
        public bool SanitizeResponse => false;
        private readonly EventPlugin plugin;
        public string Command => "Eventotp";
        public string[] Aliases { get; } = { "Evotp" };
        public string Description => "Start Event";

        public string[] Usage => new string[] { "RP", "Name of Event"};

        private static readonly HttpClient httpClient = new HttpClient();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("rozy.command"))
            {
                if (!EventPlugin.plugin.Config.EventCodeActive)
                {
                    response = "This command are off in Config";
                    return false;
                }
                response = "You`re not have permissions rozy.command";
                return false;
            }

            EventPlugin.EventMaster = ((CommandSender)sender).Nickname;
            EventPlugin.EventMasterGroup = EventPlugin.GetUserGroup((CommandSender)sender);

            if (arguments.Array.Length > 1)
            {
                EventPlugin.EventRP = arguments.Array[1];
            }

            if (arguments.Array.Length > 2)
            {
                EventPlugin.EventName = "";
                for (int i = 2; i < arguments.Array.Length; i++)
                {
                    EventPlugin.EventName = EventPlugin.EventName + arguments.Array[i] + " ";
                }
            }

            int playerCount = Player.List.Count();
            TimeSpan preparationTime = EventPlugin.time;

            if (EventPlugin.EventMode)
            {
                EventPlugin.EventMode = false;
                Timing.KillCoroutines(EventPlugin.hintCoroutine);
                response = "Event ended.";
                EventPlugin.EventRP = "Unknown";
                EventPlugin.EventName = "Unknown";
                EventPlugin.EventMaster = "Unknown";
                EventPlugin.EventMasterGroup = "Unknown";
                EventPlugin.time = new TimeSpan(0, 0, 0);
            }
            else
            {
                EventPlugin.EventMode = true;
                EventPlugin.EventPreparation = false;
                Timing.KillCoroutines(EventPlugin.hintCoroutine);
                EventPlugin.time = new TimeSpan(0, 0, 0);
                EventPlugin.hintCoroutine = Timing.RunCoroutine(EventPlugin.HintCoroutine());
                response = "Event start!";
                
                string webhookUrl = EventPlugin.plugin.Config.WebhookUrl;
                string eventName = EventPlugin.EventName.Trim();
                string eventRP = EventPlugin.EventRP;
                string eventMaster = EventPlugin.EventMaster;
                string eventMasterGroup = EventPlugin.EventMasterGroup;
                //string Mess = EventPlugin.Instance.Config.AnouncmentEvent.Replace("%NAME%", eventName).Replace("%RP%", eventRP).Replace("%MASTER%", eventMaster);
                //$"\n*On server started an event* **{eventName}** \n*with a RP level* **{eventRP}**.\n*Event Master:* **{eventMaster}** (Group of Event Master: **{eventMasterGroup}**)\n *preparation lasted* **{preparationTime.Minutes} min {preparationTime.Seconds} sec**.\n*Count of player on start event:* **{playerCount}**.";
                string embedMessage = $"\n*On server started an event* **{eventName}** \n*with a RP level* **{eventRP}**.\n*Event Master:* **{eventMaster}** (Group of Event Master: **{eventMasterGroup}**)\n *preparation lasted* **{preparationTime.Minutes} min {preparationTime.Seconds} sec**.\n*Count of player on start event:* **{playerCount}**."; ;
                SendWebhookMessage(webhookUrl, embedMessage, eventName, eventRP, eventMaster, playerCount).ConfigureAwait(false);
            }

            return true;
        }

        private static async Task SendWebhookMessage(string webhookUrl, string message, string eventName, string eventRP, string eventMaster, int playerCount)
        {
            var embed = new
            {
                embeds = new[]
                {
                new
                {
                    title = "Анонс Ивента",
                    description = message,
                    color = 5814783
                }
            }
            };
            var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(embed);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync(webhookUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    Log.Error($"Failed to send webhook message: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Exception caught while sending webhook message: {ex}");
            }
        }
    }
}