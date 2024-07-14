namespace GaragesStructure.DATA.DTOs.User
{
    public class UpdateUserForm : BaseUpdateDto
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }

        public Guid? Role { get; set; }
        
        public bool? IsActive { get; set; }

        public Guid? GarageId { get; set; }
        
        public string? Password { get; set; }

    }
}