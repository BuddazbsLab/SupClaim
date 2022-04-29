using Newtonsoft.Json;
using NLog;
using SupClaim.Model;
using Telegram.Bot;

namespace SupClaim
{
    internal class SupportMessage : Message
    {
        public SupportMessage(string resultHendler, TelegramBotClient botInit, UsingBotConfig botSettings, Logger logger)
            : base(resultHendler, botInit, botSettings, logger)
        {
        }

        public async Task<string> SendSupportMessage()
        {
            ResponeStruct deserializationObject = JsonConvert.DeserializeObject<ResponeStruct>(ResponseApi.ToString());

            // Support Обращения
            int supportAllClaims = deserializationObject.Support.Count;
            int supportHighClaim = deserializationObject.Support.Count(j => j.Priority.StartsWith("High"));
            int supportAverageClaim = deserializationObject.Support.Count(j => j.Priority.StartsWith("Average"));
            int supportIncidentClaim = deserializationObject.Support.Count(j => j.ObjectType.StartsWith("Incident"));
            int supportLowClaim = deserializationObject.Support.Count(j => j.Priority.StartsWith("Low"));

            string message = $"\nПоддержка ИС - {supportAllClaims} обращение(й)" +
                            $"\n   Высокий приоритет - {supportHighClaim} обращение(й)" +
                            $"\n   Средний приоритет - {supportAverageClaim} обращение(й)" +
                            $"\n   Низкий приоритет - {supportLowClaim} обращение(й)" +
                            $"\n   Инцидентов - {supportIncidentClaim} обращение(й)" +
                            "\n===================================================";


            return message;

        }
    }
}
