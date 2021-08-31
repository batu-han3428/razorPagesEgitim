using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorPagesEgitim.Data;
using razorPagesEgitim.Models;

namespace razorPagesEgitim.Pages.Bakimlar
{
    [Authorize]
    public class BakimGesmisiModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public BakimGesmisiModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public List<BakimHizmetiGenel> BakimHizmetiGenel { get; set; }

        public string KullaniciId { get; set; }

        public async Task<IActionResult> OnGet(int makineId)
        {
            BakimHizmetiGenel = await _db.BakimHizmetiGenel.Include(a=>a.Makina)
                .Include(b=>b.Makina.ApplicationUser)
                .Where(b=>b.MakinaId == makineId).ToListAsync();

            KullaniciId = _db.Makina.Where(u=>u.Id == makineId).ToList().FirstOrDefault().KullaniciId;

            return Page();
        }
    }
}
