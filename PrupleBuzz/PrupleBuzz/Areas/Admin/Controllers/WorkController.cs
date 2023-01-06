using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrupleBuzz.DAL;
using PrupleBuzz.Extentions;
using PrupleBuzz.Models;

namespace PrupleBuzz.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WorkController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public WorkController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
            => View(await _context.Works.Include(x => x.WorkImages).ToListAsync());

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Work work)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            foreach (var item in work.FormFiles)
            {
                if (!item.IsImage())
                {
                    ModelState.AddModelError("FormFiles", "The field form file must be image file");
                    return View();
                }

                if (!item.IsSizeOk(2))
                {
                    ModelState.AddModelError("FormFiles", "The field form can not be than more 2 mb");
                    return View();
                }
            }

            foreach (var item in work.FormFiles)
            {
                WorkImage workImage = new WorkImage
                {
                    Work = work,

                    Image = item.CreateFile(_env.WebRootPath, "assets/img"),

                    IsMain = work.WorkImages.Count > 0 ? false : true
                };
                work.WorkImages.Add(workImage);
            }
            await _context.Works.AddAsync(work);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            return View(await _context.Works.Include(x => x.WorkImages).FirstOrDefaultAsync(x => x.Id == id));
        }

        [HttpPost]

        public async Task<IActionResult> Edit(Work work)
        {
            string mainimage=null;

            Work? Exsistwork = await _context.Works.Include(x => x.WorkImages).FirstOrDefaultAsync(x => x.Id == work.Id);

            if (Exsistwork == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(Exsistwork);
            }

            if (work.FormFiles != null)
            {
                foreach (var item in work.FormFiles)
                {
                    if (!item.IsImage())
                    {
                        ModelState.AddModelError("FormFiles", "The field form file must be image file");
                        return View();
                    }

                    if (!item.IsSizeOk(2))
                    {
                        ModelState.AddModelError("FormFiles", "The field form can not be than more 2 mb");
                        return View();
                    }

                }
            }
            List<WorkImage> RemovableImages = _context.WorkImages.Where(x => !work.Ids.Contains(x.Id)).ToList();

            foreach (var item in RemovableImages)
            {
                Exsistwork.WorkImages.Remove(item);


                if (!item.IsMain)
                {
                 
                    Helpers.Helper.DeleteFile(_env.WebRootPath, "assets/img", item.Image);
                }
                else
                {
                    mainimage = item.Image;
                }

            }


            if (!Exsistwork.WorkImages.Any(x => x.IsMain))
            {
                if (work.FormFiles.Count == 0)
                {
                    ModelState.AddModelError("", "Please Choose Main Image");
                    return View(Exsistwork);
                }
            }



            if (work.FormFiles != null)
            {
                foreach (var item in work.FormFiles)
                {
                    WorkImage workImage = new WorkImage
                    {
                        Work = work,
                        WorkId = work.Id,

                        Image = item.CreateFile(_env.WebRootPath, "assets/img"),

                        IsMain = Exsistwork.WorkImages.Count > 0 && Exsistwork.WorkImages.Any(x => x.IsMain) ? false : true
                    };
                    Exsistwork.WorkImages.Add(workImage);
                }
            }
            else
            {
                if(mainimage!= null)
                {
                    Helpers.Helper.DeleteFile(_env.WebRootPath, "assets/img",mainimage);
                }
            }

            Exsistwork.Title = work.Title;
            Exsistwork.Description = work.Description;

            _context.Works.Update(Exsistwork);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
