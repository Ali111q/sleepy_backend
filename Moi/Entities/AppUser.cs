using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GaragesStructure.Entities
{
    public class AppUser : BaseEntity<Guid>
    {
        public string? Email { get; set; }
        
        public string? FullName { get; set; }
        
        public string? Password { get; set; }
        
        public Guid? RoleId { get; set; }
        public Role? Role { get; set; }

        public bool? IsActive { get; set; } = true;


        public Guid? CountryId { get; set; }
        
        public Country? Country { get; set; }

        public TimeOnly SleepTime { get; set; }
        public int SleepHours { get; set; }
        public int Gender { get; set; }

    }
    
}