using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorPagesEgitim.Data;
using razorPagesEgitim.Models;

namespace razorPagesEgitim.Pages.bakim_tipleri
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IList<bakimTipi> BakimTipi { get; set; }
        public async Task<IActionResult> OnGet()
        {
            BakimTipi = await _db.bakimTipi.ToListAsync();
            return Page();
        }
    }
}
