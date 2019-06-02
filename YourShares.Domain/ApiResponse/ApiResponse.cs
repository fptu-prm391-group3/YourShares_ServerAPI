using Newtonsoft.Json;

namespace YourShares.Domain.ApiResponse
{
    public static class ApiResponse
    {
        // TODO change Response type to ApiResponse, data field to Object T
        public static string Ok(object Object, long? count)
        {
            var response = new Response 
            {
                Data = Object, 
                Count = count ?? 0, 
                IsSuccess = true
                
            };
            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }
        
        public static string Ok()
        {
            var response = new Response {IsSuccess = true};
            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        public static string Error(int errorCode, string errorMessage)
        {
            var response = new Response
            {
                Count = 0, 
                IsSuccess = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };
            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }
    }
}