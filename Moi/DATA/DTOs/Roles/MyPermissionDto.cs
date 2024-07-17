using GaragesStructure.Entities;

namespace GaragesStructure.DATA.DTOs.roles;

public class MyPermissionDto: BaseDto<Guid>
{
    public string Action { get; set; }
    public string Subject { get; set; }
    public List<RolePermission> RolePermissions { get; set; }
}