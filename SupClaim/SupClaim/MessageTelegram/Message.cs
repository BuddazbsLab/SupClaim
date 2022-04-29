using NLog;
using SupClaim.Model;
using Telegram.Bot;

namespace SupClaim
{
    internal class Message
    {
        private readonly UsingBotConfig _botSettings;
        private readonly string _responseApi;
        private readonly TelegramBotClient _botInit;
        const string RES_API = "Нет ответа от базы!";
        private readonly Logger _logger;
        private string _sendMessage;


        public Message(string resultHendler, TelegramBotClient botInit, UsingBotConfig botSettings, Logger logger)
        {
            this._responseApi = resultHendler;
            this._botInit = botInit;
            this._botSettings = botSettings;
            this._logger = logger;
        }


        public Logger LoggerInfo => this._logger;
        public string ResponseApi => this._responseApi;
        public TelegramBotClient BotInit => this._botInit;
        public UsingBotConfig BotSettings => this._botSettings;

        public string SendSomeMessage
        {
            get => this._sendMessage;
            set => this._sendMessage = value;
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
                await BotInit.SendTextMessageAsync(
                        BotSettings.ChatID, SendSomeMessage);
            }

        }
    }
}