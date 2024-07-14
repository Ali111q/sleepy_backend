namespace GaragesStructure.DATA.DTOs.User;

public class UserFilter : BaseFilter
{
    public string? Email { get; set; }
        
    public string? FullName { get; set; }

    public Guid? RoleId { get; set; }
    
    public bool? IsActive { get; set; }
    

    
}