using GaragesStructure.DATA.DTOs;

namespace BackEndStructuer.DATA.DTOs
{

    public class MusicFilter : BaseFilter 
    {
        public string? Name { get; set; }
        public Guid CategoryId { get; set; }
    }
}
