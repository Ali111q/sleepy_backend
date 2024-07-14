using GaragesStructure.Entities;
using GaragesStructure.Helpers;
using GaragesStructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TextExtensions;

namespace GaragesStructure.DATA
{
    public class PermissionSeeder
    {
        private readonly DataContext _dbContext;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public PermissionSeeder(DataContext dbContext,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _dbContext = dbContext;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        public async Task SeedPermissions()
        {
            var permistions = AllPermissions();

            var permissions = new List<Permission>();

            foreach (var permission in permistions)
            {
                foreach (var action in permission.Actions)
                {
                    var permissionInDb = await _dbContext.Permissions.FirstOrDefaultAsync(p =>
                        p.FullName == $"{permission.Subject.ToKebabCase()}.{action.ToKebabCase()}");
                    if (permissionInDb == null)
                    {
                        _dbContext.Permissions.Add(new Permission
                        {
                            Subject = permission.Subject.ToKebabCase(),
                            Action = action.ToKebabCase(),
                            FullName = $"{permission.Subject.ToKebabCase()}.{action.ToKebabCase()}",
                        });
                    }
                }
            }

           

       
            Console.Error.Write("============================ start seed =============================");
            var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Name == "Super Admin");
            var per = await _dbContext.Permissions.ToListAsync();

            if (role == null)
            {
                
                Console.Error.Write("============================ start Error =============================");
                role = new Role()
                {
                    Name = "Super Admin" ,
                };
                
                _dbContext.Roles.Add(role);
                // foreach (var permiss in per)
                // {
                //     role.RolePermissions.Add(new RolePermission()
                //     {
                //         Permission = permiss
                //     });
                // }

                _dbContext.Users.Add(new AppUser()
                {
                    RoleId = role.Id,
                    Email = "admin@garages.com",
                    IsActive = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                    FullName = "Super Admin"
                });
                
                Console.Error.Write("============================ end user  =============================");

                
            }

            _dbContext.SaveChanges();
        }


        public class ShapedPermissions
        {
            public string Subject { get; set; }
            public IEnumerable<string> Actions { get; set; }
        }

        public List<ShapedPermissions> AllPermissions()
        {
            var groupedPermissions = _actionDescriptorCollectionProvider.ActionDescriptors.Items
                .OfType<ControllerActionDescriptor>()
                .Where(descriptor => HasAuthorizeActionFilter(descriptor))
                .GroupBy(descriptor => descriptor.ControllerName)
                .Select(group => new ShapedPermissions
                {
                    Subject = group.Key.ToKebabCase(),
                    Actions = group.Select(descriptor => $"{descriptor.ActionName}").Distinct(),
                })
                .OrderBy(controller => controller.Subject)
                .ToList();


            return groupedPermissions;
        }

        private bool HasAuthorizeActionFilter(ControllerActionDescriptor descriptor)
        {
            return descriptor.ControllerTypeInfo.GetCustomAttributes(typeof(ServiceFilterAttribute), true)
                .Concat(descriptor.MethodInfo.GetCustomAttributes(typeof(ServiceFilterAttribute), true))
                .Any(attr => attr is ServiceFilterAttribute serviceFilterAttr &&
                             serviceFilterAttr.ServiceType == typeof(AuthorizeActionFilter));
        }

        private string GetCrudType(ControllerActionDescriptor descriptor)
        {
            return descriptor.ActionName.ToKebabCase();
        }
    }
}