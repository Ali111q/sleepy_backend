namespace GaragesStructure.Entities
{
    public class Permission : BaseEntity<Guid>
    {
        public string? Subject { get; set; } 
        public string? Action { get; set; }
        public string? FullName { get; set; }
        
        
        public List<RolePermission> RolePermissions { get; set; }
        
    }
}