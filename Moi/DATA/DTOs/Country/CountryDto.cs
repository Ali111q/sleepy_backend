using GaragesStructure.DATA.DTOs.User;
using GaragesStructure.Entities;

namespace GaragesStructure.DATA.DTOs.Country;

public class CountryDto : BaseDto<Guid>
{
    public string? Name { get; set; }


    public List<UserDto>? Users { get; set; }
}