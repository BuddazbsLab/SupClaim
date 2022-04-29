using Nancy.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SupClaim.Model;
using System.Linq;
using System.Net;

namespace SupClaim
{
    sealed internal class RequestApiClaim
    {
        private readonly RestClient _ytChekClaim;
        private readonly RestClient _supportClaim;
        private readonly RestClient _supportClientClaim;
        private readonly RestClient _youtrack;
        public RequestApiClaim(UsingBotConfig botSettings)
        {
            this._ytChekClaim = new RestClient(botSettings.YTChekClaim);
            this._supportClaim = new RestClient(botSettings.SupportClaim);
            this._supportClientClaim = new RestClient(botSettings.SupportClientClaim);
            this._youtrack = new RestClient(botSettings.Youtrack);
        }
        public RestClient YTChekRestClient
        {
            get { return this._ytChekClaim; }           
        }
        public RestClient SupportClaimRestClient
        {
            get { return this._supportClaim; }
        }
        public RestClient SupportClientClaimRestClient
        {
            get { return this._supportClientClaim; }
        }
        public RestClient YoutrackRestClient
        {
            get { return this._youtrack; }
        }


        public (string?, string?, string?, string?) MakeRequestToApiClaim()
        {
            if(YTChekRestClient.Get(new RestRequest()).StatusCode == HttpStatusCode.OK)
            {
                //Делаем запросы к Claim
                var newYT = YTChekRestClient.Get(new RestRequest()).Content;
                var newSupport = SupportClaimRestClient.Get(new RestRequest()).Content;
                var newSupportClient = SupportClientClaimRestClient.Get(new RestRequest()).Content;
                var newYTGroup = YoutrackRestClient.Get(new RestRequest()).Content;             

                return new (newYT, newSupport, newSupportClient, newYTGroup);

            }
            else
            {
                return new (null, null, null, null);
            }

        }
    }
}
