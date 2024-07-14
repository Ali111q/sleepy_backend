using System.ComponentModel.DataAnnotations;

namespace GaragesStructure.DATA.DTOs.roles
{
    public class RoleForm
    {
        [Required]
        public string Name { get; set; }
    }
}