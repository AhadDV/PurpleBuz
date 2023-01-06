namespace PrupleBuzz.Models
{
    public class ServiceImage
    {
        public int Id { get; set; }
        public int Path { get; set; }
        public int ServiceId { get; set; }
        public bool isMain { get; set; }
        public Service Service { get; set; }
    }
}
