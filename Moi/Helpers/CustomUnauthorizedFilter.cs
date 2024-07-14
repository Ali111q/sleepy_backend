using System.Net;
using System.Text.Json;
using GaragesStructure.DATA.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace GaragesStructure.Helpers
{
    public class CustomUnauthorizedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;

        public CustomUnauthorizedMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment)
        {
            _next = next;
            _hostEnvironment = hostEnvironment;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            try{
                await _next(context);

            }catch(Exception ex){
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var responseContent = "{\"error\": \"Internal Server Error\"}";
                Log.Logger.Error(ex, "Error in {Endpoint} {Method} {ErrorMessage} {ErrorStackTrace}", context.Request.Path, context.Request.Method, ex.Message, ex.StackTrace);

                // check if is dev or prod
                var ApiException = _hostEnvironment.IsDevelopment() ? new ApiException(context.Response.StatusCode,ex.Message,ex.StackTrace.ToString()) : new ApiException(context.Response.StatusCode,"Internal Server Error",null);
                
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var json = JsonSerializer.Serialize(ApiException,options);
                await context.Response.WriteAsync(json);
                Log.CloseAndFlush();
            }
                

            if (context.Response.StatusCode == 401)
            {
                // Modify the response
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                var apiEXception = new ApiException(context.Response.StatusCode,"You are not authorized",null);
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var json = JsonSerializer.Serialize(apiEXception,options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}