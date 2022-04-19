using Newtonsoft.Json;

namespace SupClaim.Model
{
    public class ResponseStructure
    {
        [JsonProperty(PropertyName = "objectType")]
        public string ObjectType { get; init; }

        [JsonProperty(PropertyName = "priority")]
        public string Priority { get; init; }

        [JsonProperty(PropertyName = "assignedToOperatorName")]
        public string AssignedToOperatorName { get; init; }

    }
}
