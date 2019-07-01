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
            HttpStatusCode httpStatus;
            var errorMessage = ex.Message;
            switch (ex)
            {
                case EntityNotFoundException _:
                    httpStatus = HttpStatusCode.NotFound;
                    break;
                case FormatException _ :
                    httpStatus = HttpStatusCode.BadRequest;
                    errorMessage = ex.Message;
                    break;
                case DbUpdateException _:
                    httpStatus = HttpStatusCode.InternalServerError;
                    errorMessage = "Fail to update data. Database error occurs";
                    break;
                case UnauthorizedUser _:
                    httpStatus = HttpStatusCode.Unauthorized;
                    errorMessage = "User unauthorized. Please try to login again";
                    break;
                default:
                    httpStatus = HttpStatusCode.InternalServerError;
                    errorMessage = ex.Message;
                    break;
            }

            var response = new ResponseBuilder<dynamic>().Fail(httpStatus, errorMessage).build();
            context.Response.StatusCode = (int) httpStatus;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}