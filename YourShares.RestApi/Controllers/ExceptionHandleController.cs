using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using YourShares.Application.Exceptions;

namespace YourShares.RestApi.Controllers
{
    
    public class ExceptionHandleController
    {
        private readonly RequestDelegate _next;

        public ExceptionHandleController(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = (int) HttpStatusCode.InternalServerError;
            if (ex is EntityNotFoundException)
            {
                code = (int) HttpStatusCode.NotFound;
            }

            var response = ApiResponse.ApiResponse.Error(code, ex.Message);
            context.Response.StatusCode = code;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}