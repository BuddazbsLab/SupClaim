using Newtonsoft.Json;
using NLog;
using SupClaim.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SupClaim
{
    internal class SupportClientMessage : Message
    {
        public SupportClientMessage(string resultHendler, TelegramBotClient botInit, UsingBotConfig botSettings, Logger logger)
            : base(resultHendler, botInit, botSettings, logger)
        {
        }

        public string SendSupportClientMessage()
        {
            ResponeStruct deserializationObject = JsonConvert.DeserializeObject<ResponeStruct>(ResponseApi.ToString());
            // SupportClient Обращения
            int SupportClientAllClaims = deserializationObject.SupportClient.Count;
            int SupportClientHighClaim = deserializationObject.SupportClient.Count(j => j.Priority.StartsWith("High"));
            int SupportClientAverageClaim = deserializationObject.SupportClient.Count(j => j.Priority.StartsWith("Average"));
            int SupportClientIncidentClaim = deserializationObject.SupportClient.Count(j => j.ObjectType.StartsWith("Incident"));
            int SupportClientLowClaim = deserializationObject.SupportClient.Count(j => j.Priority.StartsWith("Low"));

            string message =$"\nПочта клиентов - {SupportClientAllClaims} обращение(й)" +
                            $"\n   Высокий приоритет - {SupportClientHighClaim} обращение(й)" +
                            $"\n   Средний приоритет - {SupportClientAverageClaim} обращение(й)" +
                            $"\n   Низкий приоритет - {SupportClientLowClaim} обращение(й)" +
                            $"\n   Инцидентов - {SupportClientIncidentClaim} обращение(й)" +
                            "\n===================================================";

            return message;
        }
    }
}
