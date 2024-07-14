using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using GaragesStructure.DATA;
using GaragesStructure.DATA.DTOs.roles;
using GaragesStructure.Entities;
using GaragesStructure.Repository;

namespace GaragesStructure.Services
{
    public interface IRoleService
    {
        Task<(List<RoleDto>? roles, int? totalCount, string? error)> GetAll();

        Task<(RoleDto? role, string? error)> Add(RoleForm role);

        Task<(RoleDto? roleDto, string? error)> Edit(Guid id, RoleForm role);

        Task<(RoleDto? roleDto, string? error)> Delete(Guid id);

        Task<(AllPermissionsDto? permissionsDto, string? error)> MyPermissions(Guid userId);

        Task<(string? role, string? error)> AddPermissionToRole(Guid roleId, AssignPermissionsDto permissions);

        Task<(AllPermissionsDto? permissionDtos, string? error)> GetAllPermissions(PermissionsFilter filter);
    }

    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly DataContext _dataContext;

        public RoleService(IMapper mapper, IRepositoryWrapper repositoryWrapper, DataContext dataContext)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _dataContext = dataContext;
        }

        public async Task<(List<RoleDto>? roles, int? totalCount, string? error)> GetAll()
        {
            var (data, totalCount) = await _repositoryWrapper.Role.GetAll<RoleDto>(0);
            return (_mapper.Map<List<RoleDto>>(data), totalCount, null);
        }

        public async Task<(RoleDto? role, string? error)> Add(RoleForm roleForm)
        {
            var check = await _repositoryWrapper.Role.Get<RoleDto>(x => x.Name == roleForm.Name);
            if (check != null)
            {
                return (null, "Role already exists");
            }

            var role = new Role()
            {
                Name = roleForm.Name
            };
            var response = await _repositoryWrapper.Role.Add(role);
            if (response == null)
            {
                return (null, "Error");
            }

            return (_mapper.Map<RoleDto>(role), null);
        }

        public async Task<(RoleDto? roleDto, string? error)> Edit(Guid id, RoleForm role)
        {
            var roleEntity = await _repositoryWrapper.Role.Get(x => x.Id == id);
            if (roleEntity == null)
            {
                return (null, "Role not found");
            }

            roleEntity.Name = role.Name;
            var response = await _repositoryWrapper.Role.Update(roleEntity);
            if (response == null)
            {
                return (null, "Error");
            }

            return (_mapper.Map<RoleDto>(response), null);
        }

        public async Task<(RoleDto? roleDto, string? error)> Delete(Guid id)
        {
            var role = await _repositoryWrapper.Role.Get(x => x.Id == id);
            if (role == null)
            {
                return (null, "Role not found");
            }

            var response = await _repositoryWrapper.Role.Delete(id);
            if (response == null)
            {
                return (null, "Error");
            }

            return (_mapper.Map<RoleDto>(response), null);
        }

        public async Task<(AllPermissionsDto? permissionsDto, string? error)> MyPermissions(Guid userId)
        {
            var user = await _repositoryWrapper.User.Get(x => x.Id == userId,
                i => i.Include(r => r.Role).ThenInclude(rp => rp.RolePermissions).ThenInclude(p => p.Permission));
            if (user == null)
            {
                return (null, "User not found");
            }

            var permissions = user.Role.RolePermissions.Select(p => p.Permission).ToList();

            // group permissions by subject

            var groupedPermissions = permissions
                .GroupBy(p => p.Subject)
                .Select(group => new PermissionDto
                {
                    Subject = group.Key,
                    Actions = group.Select((p, index) => new ActionDetailDto
                    {
                        Id = p.Id, // Or any other logic to assign Id
                        Action = p.Action,
                    }).ToList()
                }).ToList();

            AllPermissionsDto allPermissionsDto = new AllPermissionsDto
            {
                Data = groupedPermissions
            };

            return (allPermissionsDto, null);
        }

        public async Task<(string? role, string? error)> AddPermissionToRole(Guid roleId,
            AssignPermissionsDto assignPermissions)
        {
            var role = await _repositoryWrapper.Role.Get(x => x.Id == roleId, k => k.Include(p => p.RolePermissions));
            if (role == null)
            {
                return (null, "Role not found");
            }

            var allPermissions = await _repositoryWrapper.Permission.GetAll();
            var permissionMap = allPermissions.data.ToDictionary(p => p.Id, p => p);

            using var transaction = await _dataContext.Database.BeginTransactionAsync();

            try
            {
                role.RolePermissions.RemoveAll(rp => !assignPermissions.Permissions.Contains(rp.Permission.Id));
                foreach (var permissionName in assignPermissions.Permissions)
                {
                    if (!role.RolePermissions.Any(rp => rp.Permission.Id == permissionName))
                    {
                        var permissionEntity = permissionMap[permissionName];
                        role.RolePermissions.Add(new RolePermission { Role = role, Permission = permissionEntity });
                    }
                }

                await _dataContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return ("done", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("++++++++++++++++++++++" + ex.Message);
                await transaction.RollbackAsync();
                return (null, "An error occurred while updating role permissions.");
            }
        }

        public async Task<(AllPermissionsDto? permissionDtos, string? error)> GetAllPermissions(
            PermissionsFilter filter)
        {
            var (data, totalCount) = await _repositoryWrapper.Permission.GetAll(
                x => (
                    (filter.RoleId == null || x.RolePermissions.Any(rp => rp.Role.Id == filter.RoleId))
                )
                , 0);

            
            // group permissions by subject

            var groupedPermissions = data
                .GroupBy(p => p.Subject)
                .Select(group => new PermissionDto
                {
                    Subject = group.Key,
                    Actions = group.Select((p, index) => new ActionDetailDto
                    {
                        Id = p.Id, // Or any other logic to assign Id
                        Action = p.Action,
                    }).ToList()
                }).ToList();

            AllPermissionsDto allPermissionsDto = new AllPermissionsDto
            {
                Data = groupedPermissions
            };

            return (allPermissionsDto, null);
        }
    }
}