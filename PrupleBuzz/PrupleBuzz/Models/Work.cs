using System.ComponentModel.DataAnnotations.Schema;

namespace PrupleBuzz.Models
{
    public class Work
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<WorkImage> WorkImages { get; set;}
        [NotMapped]
        public List<IFormFile> FormFiles { get; set;}=new List<IFormFile>();
        [NotMapped]
        public List<int> Ids { get; set;}=new List<int>();

        public Work()
        {
            WorkImages= new List<WorkImage>();
        }
    }
}
