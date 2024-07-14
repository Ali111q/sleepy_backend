using GaragesStructure.Entities;

namespace BackEndStructuer.Entities
{
    public class Music : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string AudioUrl { get; set; }
        public string ArtUrl { get; set; }
        public string Album { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
