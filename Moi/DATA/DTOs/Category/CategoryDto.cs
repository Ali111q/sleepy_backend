using GaragesStructure.DATA.DTOs;

namespace BackEndStructuer.DATA.DTOs
{

    public class CategoryDto:BaseDto<Guid>
    {
        public string Name { get; set; }
        public string ArtUrl { get; set; }
        public string Color { get; set; }
    }
}
