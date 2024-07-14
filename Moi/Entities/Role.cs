namespace GaragesStructure.Entities
{
    public class Role : BaseEntity<Guid>
    {
            public string Name { get; set; }
            public List<RolePermission> RolePermissions { get; set; }

    }
}