using Newtonsoft.Json;
using SupClaim.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SupClaim
{
    sealed internal class Message
    {
        private readonly List<ResponseStructure> _deserializationObject;
        private readonly UsingBotConfig _botSettings;
        private readonly string _responseApi;
        private readonly TelegramBotClient _botInit;
        const string RES_API = "Нет ответа от базы!";
        private string _buffer;

        public Message(string responseApi, TelegramBotClient botInit, UsingBotConfig botSettings)
        {
            this._responseApi = responseApi;
            this._botInit = botInit;
            this._deserializationObject = JsonConvert.DeserializeObject<List<ResponseStructure>>(ResponseApi.ToString());
            this._botSettings = botSettings;
            this._buffer = string.Empty;
        }

        public string ResponseApi => this._responseApi;
        public TelegramBotClient BotInit => this._botInit;
        public List<ResponseStructure> DeserializationObject => this._deserializationObject;
        public UsingBotConfig BotSettings => this._botSettings;

        public string Buffer
        {
            get => this._buffer;
            set => this._buffer = value;
        }

        public async Task SendMessage()
        {

            if (ResponseApi == null)
            {
                await BotInit.SendTextMessageAsync(
                   BotSettings.ChatID,
                   RES_API);
            }
            else
            {
                var allClaims = DeserializationObject.Count;
                var highClaim = DeserializationObject.Count(j => j.Priority.StartsWith("High"));
                var averageClaim = DeserializationObject.Count(j => j.Priority.StartsWith("Average"));
                var incidentClaim = DeserializationObject.Count(j => j.ObjectType.StartsWith("Incident"));
                var lowClaim = DeserializationObject.Count(j => j.Priority.StartsWith("Low"));


                using (FileStream fstream = File.OpenRead("buffer.txt"))
                {

                    byte[] buffer = new byte[fstream.Length];
                    await fstream.ReadAsync(buffer);
                    string textFromFile = Encoding.Default.GetString(buffer);
                    Buffer = textFromFile;
                }


                var compareString = string.Compare(Buffer, ResponseApi);

                if (compareString <= 0)
                {
                    using (FileStream fstream = new("buffer.txt", FileMode.OpenOrCreate))
                    using (StreamWriter sr = new(fstream))
                    {
                        await sr.WriteLineAsync(ResponseApi);
                    }

                    if (allClaims >= 1)
                    {
                        await BotInit.SendTextMessageAsync(
                            BotSettings.ChatID,
                            $"Всего обращений - {allClaims}" +
                            $"\nВысокий приоритет - {highClaim} обращение(й)" +
                            $"\nСредний приоритет - {averageClaim} обращение(й)" +
                            $"\nИнцидентов!!! {incidentClaim} обращение(й)" +
                            $"\nНизкий приорите (может и инцидент) - {lowClaim} обращение(й)");
                    }
                    else
                    {
                        await BotInit.SendTextMessageAsync(
                            BotSettings.ChatID,
                            "Расслабтесь.");
                    }

                }

            }

        }
    }
}
