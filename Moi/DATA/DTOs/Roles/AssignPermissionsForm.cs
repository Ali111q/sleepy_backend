namespace GaragesStructure.DATA.DTOs.roles;

public class AssignPermissionsForm
{
    public Guid RoleId { get; set; }
    public AssignPermissionsDto Permissions { get; set; }
}