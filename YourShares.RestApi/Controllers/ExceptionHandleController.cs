using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YourShares.Application.Exceptions;
using YourShares.RestApi.ApiResponse;

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
            var errMsg = ex.Message;
            switch (ex)
            {
                case EntityNotFoundException _:
                    code = (int) HttpStatusCode.NotFound;
                    break;
                case DbUpdateException _:
                    code = (int) HttpStatusCode.InternalServerError;
                    errMsg = "Fail to update data. Database error occurs";
                    break;
            }

            var response = new ResponseBuilder<dynamic>().NotFound(errMsg).build();
            context.Response.StatusCode = code;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}