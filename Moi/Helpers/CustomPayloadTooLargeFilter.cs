using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GaragesStructure.Helpers
{
    public class CustomPayloadTooLargeMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomPayloadTooLargeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.ContentLength.HasValue && context.Request.ContentLength > MaximumPayloadSize)
            {
                context.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
                context.Response.ContentType = "application/json";
                var responseContent = "{\"error\": \"Request payload too large\"}";
                await context.Response.WriteAsync(responseContent);
            }
            else
            {
                await _next(context);
            }
        }

        // Adjust this value to set the maximum payload size in bytes
        private const long MaximumPayloadSize = 100 * 1024 * 1024 ; // 10 MB
    }
}