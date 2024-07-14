namespace GaragesStructure.Entities;

public class Country : BaseEntity<Guid>
{
    public string? Name { get; set; }


    public List<AppUser>? Users { get; set; }
}