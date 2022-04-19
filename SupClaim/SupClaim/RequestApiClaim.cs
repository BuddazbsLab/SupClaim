using RestSharp;
using SupClaim.Model;
using System.Net;

namespace SupClaim
{
    sealed internal class RequestApiClaim
    {
        private readonly UsingBotConfig _botSettings;
        public RequestApiClaim(UsingBotConfig botSettings)
        {
            this._botSettings = botSettings;
        }

        public UsingBotConfig BotSettings => this._botSettings;

        public string MakeRequestToApiClaim()
        {
            var client =
                new RestClient(
                    BotSettings.Basgeteurl);

            if (client.Get(new RestRequest()).StatusCode == HttpStatusCode.OK)
            {
                return client.Get(new RestRequest()).Content;
            }
            else
            {
                return null;
            }

        }
    }
}
