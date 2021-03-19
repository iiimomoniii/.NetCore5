using System.Net;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Hero_Project.NetCore5.Exceptions;
using System.Text.Json;

namespace Hero_Project.NetCore5.Middleware
{
    public class ErrorHadlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHadlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, IWebHostEnvironment env) {
            try {
                await _next(context);
            } catch (Exception ex) {
                await HandleExceptionAsync(context, ex, env);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, IWebHostEnvironment env) {
            HttpStatusCode status;
            string message;
            var stackTrace = String.Empty;

            var exceptionType = exception.GetType();
            if (exceptionType == typeof(BadRequestException)) {
                message = exception.Message;
                status = HttpStatusCode.BadRequest;
            } else if (exceptionType == typeof(NotFoundException)) {
                message = exception.Message;
                status = HttpStatusCode.NotFound;
            } else {
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
                if (env.IsEnvironment("Development")) {
                    stackTrace = exception.StackTrace;
                }
            }
            var result = JsonSerializer.Serialize( new { error = message, stackTrace });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(result);
        }   
    }
}