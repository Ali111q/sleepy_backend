using GaragesStructure.DATA;
using Microsoft.EntityFrameworkCore;

public class UserStateMiddleware
{
    private readonly RequestDelegate _next;

    public UserStateMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        
        
        
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            using (var scope = context.RequestServices.CreateScope())
            {
                try
                {
                    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();


                    var userIdFomClaim = context.User.Claims?.FirstOrDefault()?.Value;
                    if (userIdFomClaim != null)
                    {
                        var userId = Guid.Parse(userIdFomClaim);
                        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                        if (user.IsActive == false )
                        {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var responseContent = "{\"error\": \"User is not active\"}";
                            await context.Response.WriteAsync(responseContent);
                            return;
                        }
                    }
                }
                catch (Exception e)
                {
                    await _next(context);

                }
               
            }
        }

        await _next(context);
    }
}