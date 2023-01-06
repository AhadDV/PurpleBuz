using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrupleBuzz.DAL;
using PrupleBuzz.Extentions;
using PrupleBuzz.Models;

namespace PrupleBuzz.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories.Where(x=>!x.IsDeleted).ToListAsync();
            return View(categories);
        }
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (category.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "The field image is required");
                return View();
            }



            bool isExist = await _context.Categories.Where(x=>!x.IsDeleted).AnyAsync(c => c.Name.ToLower().Trim() == category.Name.ToLower().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Name", "It is alreay exist!");
                return View();
            }

            if (!category.FormFile.IsImage())
            {
                ModelState.AddModelError("FormFile", "Qaqa sekil gonder");
                return View();
            }

            if (!category.FormFile.IsSizeOk(2))
            {
                ModelState.AddModelError("FormFile", "Brat seklin olcusu 2 mb artiq ola bilmez");
                return View();
            }
            category.Image = category.FormFile.CreateFile(_env.WebRootPath, "assets/img");

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            // return Json(category);
        }
        public IActionResult Update(int? id)
        {
            if (id == null || id <= 0)
                return NotFound();

            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }
        [HttpPost]
        public IActionResult Update(Category category)
        {

            Category? ExsistCategory = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if(!ModelState.IsValid)
                return View(ExsistCategory);

            if (ExsistCategory == null)
                return NotFound();

            if (category.FormFile != null)
            {
                if (!category.FormFile.IsImage())
                {
                    ModelState.AddModelError("FormFile", "Qaqa sekil gonder");
                    return View();
                }

                if (!category.FormFile.IsSizeOk(2))
                {
                    ModelState.AddModelError("FormFile", "Brat seklin olcusu 2 mb artiq ola bilmez");
                    return View();
                }

                Helpers.Helper.DeleteFile(_env.WebRootPath,"assets/img",ExsistCategory.Image);

                ExsistCategory.Image = category.FormFile.CreateFile(_env.WebRootPath, "assets/img");
            }

            ExsistCategory.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Category? category=_context.Categories.Find(id);

            if (category == null)
                return Json( new {status=404 });


            Helpers.Helper.DeleteFile(_env.WebRootPath, "assets/img", category.Image);
             category.IsDeleted=true;
            _context.SaveChanges();

            return Json(new { status=200});
            
        }

    }
}
