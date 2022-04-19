using Microsoft.Extensions.Configuration;
using SupClaim.Model;

namespace SupClaim
{
    internal class Settings
    {
        private readonly IConfiguration _configuration;
        public Settings(IConfiguration configureProvider)
        {
            this._configuration = configureProvider;
        }
        public IConfiguration ConfigureProvider => this._configuration;
        public UsingBotConfig GetSettingsBotConfig()
        {
            return ConfigureProvider.GetSection("AppSettings").Get<UsingBotConfig>();
        }
    }
}
