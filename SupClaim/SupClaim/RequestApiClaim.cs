using RestSharp;
using SupClaim.Model;
using System.Net;

namespace SupClaim
{
    sealed internal class RequestApiClaim
    {
        private readonly RestClient _restClient;
        public RequestApiClaim(UsingBotConfig botSettings)
        {
            this._restClient = new RestClient(botSettings.Basgeteurl);
        }
        public RestClient NewrestClient
        {
            get { return this._restClient; }           
        }

        public string? MakeRequestToApiClaim()
        {
            if(NewrestClient.Get(new RestRequest()).StatusCode == HttpStatusCode.OK)
            {
                return NewrestClient.Get(new RestRequest()).Content;
            }
            else
            {
                return null;
            }



        }
    }
}
