using Newtonsoft.Json;

namespace YourShares.RestApi.ApiResponse
{
    public class Response<T>
    {
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? ErrorCode { get; set; }
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
        public long? Count { get; set; } = 0;
        public bool IsSuccess { get; set; }
    }
}