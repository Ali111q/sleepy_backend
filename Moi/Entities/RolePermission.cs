namespace GaragesStructure.Entities
{
    public class RolePermission : BaseEntity<Guid>
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}