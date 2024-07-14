using System.ComponentModel.DataAnnotations;
using GaragesStructure.Utils;

namespace GaragesStructure.Entities
{
    

    public class Notifications : BaseEntity<Guid>
    {
        public Guid NotifyId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public string? Picture { get; set; }
        public NotifyFor? NotifyFor { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public bool IsRead { get; set; } = false;

        public NotificationDestination? NotificationDestination { get; set; }
    }
}