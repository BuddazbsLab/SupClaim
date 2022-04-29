using Newtonsoft.Json;

namespace SupClaim.Model
{
    internal class ResponeStruct
    {
        [JsonProperty(PropertyName = "chekYT")]
        public List<ChekYT> ChekYT { get; init; }

        [JsonProperty(PropertyName = "support")]
        public List<Support> Support { get; init; }

        [JsonProperty(PropertyName = "supportClient")]
        public List<SupportClient> SupportClient { get; init; }

        [JsonProperty(PropertyName = "groupYT")]
        public List<GroupYT> GroupYT { get; init; }
    }
}
