namespace GaragesStructure.Entities
{
    public class LoginLogger : BaseEntity<Guid>
    {
        public Guid? UserId { get; set; }

        public AppUser? User { get; set; }

        public string? Ip { get; set; }
    }
}
