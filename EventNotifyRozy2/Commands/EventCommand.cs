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

namespace EventNotifyRozy2
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EventCommand : ICommand, IUsageProvider
    {
        public bool SanitizeResponse => false;
        private readonly EventPugin plugin;
        public string Command => "Eventotp";
        public string[] Aliases { get; } = { "Evotp" };
        public string Description => "Start Event";

        public string[] Usage => new string[] { "RP", "Name of Event"};

        private static readonly HttpClient httpClient = new HttpClient();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("rozy.command"))
            {
                if (!EventPugin.plugin.Config.EventCodeActive)
                {
                    response = "This command are off in Config";
                    return false;
                }
                response = "You`re not have permissions rozy.command";
                return false;
            }

            EventPugin.EventMaster = ((CommandSender)sender).Nickname;
            EventPugin.EventMasterGroup = EventPugin.GetUserGroup((CommandSender)sender);

            if (arguments.Array.Length > 1)
            {
                EventPugin.EventRP = arguments.Array[1];
            }

            if (arguments.Array.Length > 2)
            {
                EventPugin.EventName = "";
                for (int i = 2; i < arguments.Array.Length; i++)
                {
                    EventPugin.EventName = EventPugin.EventName + arguments.Array[i] + " ";
                }
            }

            int playerCount = Player.List.Count();
            TimeSpan preparationTime = EventPugin.time;

            if (EventPugin.EventMode)
            {
                EventPugin.EventMode = false;
                Timing.KillCoroutines(EventPugin.hintCoroutine);
                response = "Event ended.";
                EventPugin.EventRP = "Unknown";
                EventPugin.EventName = "Unknown";
                EventPugin.EventMaster = "Unknown";
                EventPugin.EventMasterGroup = "Unknown";
                EventPugin.time = new TimeSpan(0, 0, 0);
            }
            else
            {
                EventPugin.EventMode = true;
                EventPugin.EventPreparation = false;
                Timing.KillCoroutines(EventPugin.hintCoroutine);
                EventPugin.time = new TimeSpan(0, 0, 0);
                EventPugin.hintCoroutine = Timing.RunCoroutine(EventPugin.HintCoroutine());
                response = "Event start!";
                //char min = (char)preparationTime.Minutes;
                
                string webhookUrl = EventPugin.plugin.Config.WebhookUrl;
                string eventName = EventPugin.EventName.Trim();
                string eventRP = EventPugin.EventRP;
                string eventMaster = EventPugin.EventMaster;
                string eventMasterGroup = EventPugin.EventMasterGroup;
                //string Mess = EventPugin.Instance.Config.AnouncmentEvent.Replace("%NAME%", eventName).Replace("%RP%", eventRP).Replace("%MASTER%", eventMaster);
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