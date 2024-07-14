namespace BackEndStructuer.DATA.DTOs
{

    public class MusicForm 
    {
        public string Name { get; set; }
        public string AudioUrl { get; set; }
        public string ArtUrl { get; set; }
        public string Album { get; set; }
        public Guid CategoryId { get; set; }
    }
}
