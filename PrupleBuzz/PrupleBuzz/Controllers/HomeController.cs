using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrupleBuzz.DAL;
using PrupleBuzz.Models;
using PrupleBuzz.ViewModels;
using System.Diagnostics;

namespace PrupleBuzz.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                slider = await _context.Slider.ToListAsync(),
                categories = await _context.Categories.ToListAsync(),
                services = await _context.Services
                .Include(s => s.Category)
                .Include(s => s.Images)
                .OrderByDescending(s => s.Id)
                .Take(10)
                .ToListAsync(),
            };
        
            return View(homeVM);
        }
    }
}