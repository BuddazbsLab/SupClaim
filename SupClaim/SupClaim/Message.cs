using Newtonsoft.Json;
using NLog;
using SupClaim.Model;
using System.Collections.Generic;
using System.Text;
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
        private readonly Logger _logger;


        public Message(string responseApi, TelegramBotClient botInit, UsingBotConfig botSettings, Logger logger)
        {
            this._responseApi = responseApi;
            this._botInit = botInit;
            this._deserializationObject = JsonConvert.DeserializeObject<List<ResponseStructure>>(ResponseApi.ToString());          
            this._botSettings = botSettings;
            this._buffer = string.Empty;
            this._logger = logger;
        }


        public Logger LoggerInfo => this._logger;
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
                LoggerInfo.Info("[*]Получен плохой ответ от API");
                await BotInit.SendTextMessageAsync(
                   BotSettings.ChatID,
                   RES_API);
            }
            else
            {
                int allClaims = DeserializationObject.Count;
                int highClaim = DeserializationObject.Count(j => j.Priority.StartsWith("High"));
                int averageClaim = DeserializationObject.Count(j => j.Priority.StartsWith("Average"));
                int incidentClaim = DeserializationObject.Count(j => j.ObjectType.StartsWith("Incident"));
                int lowClaim = DeserializationObject.Count(j => j.Priority.StartsWith("Low"));

                LoggerInfo.Info("[*]Читаю данные из файла");
                using (FileStream fstream = File.OpenRead("buffer.txt"))
                {

                    byte[] buffer = new byte[fstream.Length];
                    await fstream.ReadAsync(buffer);
                    string textFromFile = Encoding.Default.GetString(buffer);
                    Buffer = textFromFile;
                }

                LoggerInfo.Info("[*]Сравниваю данные полученные по API с теми что в файле");
                int compareString = string.Compare(Buffer, ResponseApi);

                if (compareString <= 0)
                {
                    LoggerInfo.Info("[*]Получены новые данные. Записываю в файл.");
                    using (FileStream fstream = new("buffer.txt", FileMode.OpenOrCreate))
                    using (StreamWriter sr = new(fstream))
                    {
                        await sr.WriteLineAsync(ResponseApi);
                    }
                    LoggerInfo.Info("[*]Напаривл статистику в телеграмм");
                    LoggerInfo.Info("═════════════════════════════════════════════════════════════════════════════════");
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
                else
                {
                    LoggerInfo.Info("[*]Полученные данные совпадают. Жду дальше.");
                    LoggerInfo.Info("═════════════════════════════════════════════════════════════════════════════════");
                }

            }

        }
    }
}
