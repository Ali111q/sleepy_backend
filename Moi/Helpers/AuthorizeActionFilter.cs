using System.Security.Claims;
using GaragesStructure.Repository;
using GaragesStructure.DATA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using TextExtensions;

namespace GaragesStructure.Helpers
{
    public class AuthorizeActionFilter : IAsyncActionFilter
    {
        
        private readonly DataContext _dbContext;

        public AuthorizeActionFilter(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                string controllerName = controllerActionDescriptor.ControllerName;
                string actionName = GetCrudType(controllerActionDescriptor);

                string requiredPermission = $"{controllerName.ToKebabCase()}.{actionName.ToKebabCase()}";

                bool hasPermission = await UserHasPermission(context.HttpContext.User, requiredPermission);

                if (!hasPermission)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }

            await next();
        }
        
        private async Task<bool> UserHasPermission(ClaimsPrincipal user, string permission)
        {
               var userIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);
               var role  = user.FindFirstValue(ClaimTypes.Role);
               
               if (!Guid.TryParse(userIdStr, out var userId))
               {
                   return false;
               }

               var hasPermission = await _dbContext.Roles.Where(r => r.Name == role)
               .AsNoTracking()
               .Join(_dbContext.RolePermissions, r => r.Id, rp => rp.RoleId, (r, rp) => rp)
               .Join(_dbContext.Permissions, rp => rp.PermissionId, p => p.Id, (rp, p) => p)
               .AnyAsync(p => p.FullName == permission);

               return hasPermission;
        }
        
        private string GetCrudType(ControllerActionDescriptor descriptor)
        {
            return descriptor.ActionName.ToKebabCase();
         
        }
        
    }
}