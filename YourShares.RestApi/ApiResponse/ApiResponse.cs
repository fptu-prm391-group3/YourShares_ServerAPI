namespace YourShares.RestApi.ApiResponse
{
    public static class ApiResponse
    {
        // TODO change Response type to ApiResponse, data field to Object T
        public static Response<T> Ok<T>(T data, long? count)
        {
            return new Response<T>
            {
                Data = data, 
                Count = count ?? 0, 
                IsSuccess = true
                
            };
        }
        
        public static Response<object> Ok()
        {
            return new Response<object> {IsSuccess = true};
        }

        public static Response<object> Error(int errorCode, string errorMessage)
        {
            return new Response<object>
            {
                Count = 0, 
                IsSuccess = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };
        }
    }
}