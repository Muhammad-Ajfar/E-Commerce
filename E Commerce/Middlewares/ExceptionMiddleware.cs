namespace E_Commerce.Middlewares
{
    using E_Commerce.Helpers;
    using Microsoft.AspNetCore.Http;
    using System.Net;
    using System.Text.Json;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = exception switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;

            var response = new ApiResponses<string>(
                statusCode,
                exception.Message,
                null,
                exception.StackTrace // Optional: Remove in production.
            );

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

}
