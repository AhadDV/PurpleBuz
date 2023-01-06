using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrupleBuzz.DAL;
using PrupleBuzz.Models;

namespace PrupleBuzz.Controllers
{
    public class WorkController : Controller
    {

        private readonly AppDbContext _context;

        public WorkController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> slider = await _context.Slider.ToListAsync();
            return View(slider);
        }

        public async Task<IActionResult> Detail()
        {
            return View();
        }
    }
}
