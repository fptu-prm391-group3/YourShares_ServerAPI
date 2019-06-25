using System;
using Newtonsoft.Json;

namespace YourShares.RestApi.ApiResponse
{
    [Serializable]
    public class ResponseModel<T>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ErrorCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMessage { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? Count { get; set; } = 0;

        public bool IsSuccess { get; set; }
    }
}