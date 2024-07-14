using GaragesStructure.DATA.DTOs;

namespace BackEndStructuer.DATA.DTOs
{

    public class CategoryUpdate: BaseUpdateDto
    {
        public string? Name { get; set; }
        public string? ArtUrl { get; set; }
    }
}
