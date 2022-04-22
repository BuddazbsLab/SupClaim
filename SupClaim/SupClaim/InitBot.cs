using Telegram.Bot;

namespace SupClaim
{
    internal class InitBot
    {
        public async Task<TelegramBotClient> StartBot(Settings settings)
        {
            return new TelegramBotClient(settings.GetSettingsBotConfig().Token);
        }
    }
}
