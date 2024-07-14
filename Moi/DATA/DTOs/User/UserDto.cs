using GaragesStructure.DATA.DTOs.Country;
using GaragesStructure.DATA.DTOs.roles;

namespace GaragesStructure.DATA.DTOs.User
{
    public class UserDto : BaseDto<Guid>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        
        public string Email { get; set; }
        public RoleDto? Role { get; set; }
        public string Token { get; set; }

        public CountryDto? Country { get; set; }
        
        public bool? IsActive { get; set; }
    }
}