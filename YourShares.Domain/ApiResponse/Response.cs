namespace YourShares.Domain.ApiResponse
{
    public class Response
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; }
        public long? Count { get; set; } = 0;
        public bool IsSuccess { get; set; }
    }
}