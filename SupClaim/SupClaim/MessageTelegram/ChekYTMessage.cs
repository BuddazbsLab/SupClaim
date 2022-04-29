using Newtonsoft.Json;
using NLog;
using SupClaim.Model;
using Telegram.Bot;

namespace SupClaim
{
    internal class ChekYTMessage : Message
    {
        public ChekYTMessage(string resultHendler, TelegramBotClient botInit, UsingBotConfig botSettings, Logger logger)
            : base(resultHendler, botInit, botSettings, logger)
        {
        }

        public async Task<string> SendChekYTMessage()
        {
            ResponeStruct deserializationObject = JsonConvert.DeserializeObject<ResponeStruct>(ResponseApi.ToString());
            // Проверить YT Обращения
            int ChekYTAllClaims =  deserializationObject.ChekYT.Count;
            int ChekYTHighClaim = deserializationObject.ChekYT.Count(j => j.Priority.StartsWith("High"));
            int ChekYTAverageClaim = deserializationObject.ChekYT.Count(j => j.Priority.StartsWith("Average"));
            int ChekYTIncidentClaim = deserializationObject.ChekYT.Count(j => j.ObjectType.StartsWith("Incident"));
            int ChekYTLowClaim = deserializationObject.ChekYT.Count(j => j.Priority.StartsWith("Low"));

            string message = $"\nПроверить YT - {ChekYTAllClaims} обращение(й)" +
                            $"\n   Высокий приоритет - {ChekYTHighClaim} обращение(й)" +
                            $"\n   Средний приоритет - {ChekYTAverageClaim} обращение(й)" +
                            $"\n   Низкий приоритет - {ChekYTLowClaim} обращение(й)" +
                            $"\n   Инцидентов - {ChekYTIncidentClaim} обращение(й)";

            return  message;
        }
    }
}
