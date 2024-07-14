using GaragesStructure.DATA.DTOs;

namespace BackEndStructuer.DATA.DTOs
{

    public class MusicUpdate: BaseUpdateDto
    {
        public string? Name { get; set; }
        public string? ArtUrl { get; set; }
        public string? AudioUrl { get; set; }
    }
}
