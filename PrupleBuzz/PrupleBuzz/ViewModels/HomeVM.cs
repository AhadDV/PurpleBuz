using PrupleBuzz.Models;

namespace PrupleBuzz.ViewModels
{
    public class HomeVM
    {
        public List<Slider> slider { get; set; }
        public List<Category> categories { get; set; }
        public List<Service> services { get; set; }
    }
}
