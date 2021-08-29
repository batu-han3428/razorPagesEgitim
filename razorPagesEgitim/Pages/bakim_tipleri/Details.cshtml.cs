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
using razorPagesEgitim.Utility;

namespace razorPagesEgitim.Pages.bakim_tipleri
{
    [Authorize(Roles = StatikRoller.AdminKullanici)]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DetailsModel(ApplicationDbContext db)
        {
            _db = db;
        }
        
        [BindProperty]
        public bakimTipi BakimTipi { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            BakimTipi = await _db.bakimTipi.FirstOrDefaultAsync(m => m.Id == id);

            if (BakimTipi == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
