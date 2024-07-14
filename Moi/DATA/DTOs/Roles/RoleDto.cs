using GaragesStructure.Entities;

namespace GaragesStructure.DATA.DTOs.roles
{
    public class RoleDto : BaseDto<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
       
    }
}