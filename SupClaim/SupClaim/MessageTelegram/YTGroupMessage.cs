

using Newtonsoft.Json;
using NLog;
using SupClaim.Model;
using Telegram.Bot;

namespace SupClaim
{
    internal class YTGroupMessage : Message
    {
        public YTGroupMessage(string resultHendler, TelegramBotClient botInit, UsingBotConfig botSettings, Logger logger)
            : base(resultHendler, botInit, botSettings, logger)
        {
        }

        public string SendYTGroupMessage()
        {

            ResponeStruct deserializationObject = JsonConvert.DeserializeObject<ResponeStruct>(ResponseApi.ToString());
            // Создано в YT Обращения
            int GroupYTAllClaims = deserializationObject.GroupYT.Count;
            int GroupYTHighClaim = deserializationObject.GroupYT.Count(j => j.Priority.StartsWith("High"));
            int GroupYTAverageClaim = deserializationObject.GroupYT.Count(j => j.Priority.StartsWith("Average"));
            int GroupYTIncidentClaim = deserializationObject.GroupYT.Count(j => j.ObjectType.StartsWith("Incident"));
            int GroupYTLowClaim = deserializationObject.GroupYT.Count(j => j.Priority.StartsWith("Low"));

            string message = $"\nСоздано YT  - {GroupYTAllClaims} обращение(й)" +
                            $"\n   Высокий приоритет - {GroupYTHighClaim} обращение(й)" +
                            $"\n   Средний приоритет - {GroupYTAverageClaim} обращение(й)" +
                            $"\n   Низкий приоритет - {GroupYTLowClaim} обращение(й)" +
                            $"\n   Инцидентов - {GroupYTIncidentClaim} обращение(й)" +
                            "\n===================================================";

            return message;
        }
    }
}
