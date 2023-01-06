using System.ComponentModel.DataAnnotations.Schema;

namespace PrupleBuzz.Models
{
    public class Service
    {
        public Service()
        {
            Images = new List<ServiceImage>();
        }
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; } 
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public bool IsDeleted { get; set; }
        public List<ServiceImage> Images { get; set; }

    }
}
