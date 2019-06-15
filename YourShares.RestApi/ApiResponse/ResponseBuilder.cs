using System.Net;

namespace YourShares.RestApi.ApiResponse
{
    public class ResponseBuilder<T>
    {
        private readonly ResponseModel<T> _responseModel;

        public ResponseBuilder()
        {
            _responseModel = new ResponseModel<T>();
        }

//        public static ResponseModel<T> Ok<T>(T data, long? count)
//        {
//            return new ResponseModel<T>
//            {
//                Data = data, 
//                Count = count ?? 0, 
//                IsSuccess = true
//                
//            };
//        }
//        
//        public static ResponseModel<object> Ok()
//        {
//            return new ResponseModel<object> {IsSuccess = true};
//        }
//
//        public static ResponseModel<object> Error(int errorCode, string errorMessage)
//        {
//            return new ResponseModel<object>
//            {
//                Count = 0, 
//                IsSuccess = false,
//                ErrorCode = errorCode,
//                ErrorMessage = errorMessage
//            };
//        }
        public ResponseBuilder<T> Success()
        {
            _responseModel.IsSuccess = true;
            return this;
        }

        public ResponseBuilder<T> NotFound(string errorMsg)
        {
            _responseModel.ErrorCode = (int) HttpStatusCode.NotFound;
            _responseModel.ErrorMessage = errorMsg;
            return this;
        }

        public ResponseBuilder<T> BadRequest(string errorMsg)
        {
            _responseModel.ErrorCode = (int) HttpStatusCode.BadRequest;
            _responseModel.ErrorMessage = errorMsg;
            return this;
        }

        public ResponseBuilder<T> Data(T data)
        {
            _responseModel.Data = data;
            return this;
        }

        public ResponseBuilder<T> Count(long count)
        {
            _responseModel.Count = count;
            return this;
        }

        public ResponseModel<T> build()
        {
            return _responseModel;
        }
    }
}