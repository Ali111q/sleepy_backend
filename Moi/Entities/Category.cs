using GaragesStructure.Entities;

namespace BackEndStructuer.Entities
{
    public class Category : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string ArtUrl { get; set; }
        public List<Music> Musics { get; set; }
    }
}
