using GaragesStructure.Entities;
using GaragesStructure.Repository;

namespace GaragesStructure.DATA;

public class RoleSeeder
{

    private readonly IRepositoryWrapper _repositoryWrapper;
    

    public async Task AddRole()
    {
        var role = await _repositoryWrapper.Role.Get(x => x.Name == "Super Admin");
        var (per , totalCount) = await _repositoryWrapper.Permission.GetAll();
        
        if (role == null)
        {
            Role newRole = new Role()
            {
                Name = "Super Admin"
            };

            newRole = await _repositoryWrapper.Role.Add(newRole);
            foreach (var permissions in per)
            {
                
               newRole.RolePermissions.Add(new RolePermission()
               {
                   Permission = permissions
               });
            }


            _repositoryWrapper.User.Add(new AppUser()
            {
                RoleId = newRole.Id ,
                Email = "admin@garages.com" ,
                IsActive = true ,
                Password = BCrypt.Net.BCrypt.HashPassword("12345678") ,
                FullName = "Super Admin" 
            });

        }
    }
}