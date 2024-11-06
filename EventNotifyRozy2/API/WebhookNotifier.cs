using Exiled.API.Features;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventNotifyRozy2.API
{
    public static class WebhookNotifier
    {
        private static readonly HttpClient httpClient = new HttpClient();


        public static async Task SendAdminCommandNotification(string webhookMessage)
        {
            string webhookUrl = EventPugin.plugin.Config.WebhookUrlAbus;

            var embed = new
            {
                embeds = new[]
                {
                    new
                    {
                        title = "Abuse",
                        description = webhookMessage,
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
            catch (System.Exception ex)
            {
                Log.Error($"Exception caught while sending webhook message: {ex}");
            }
        }
    }
}
