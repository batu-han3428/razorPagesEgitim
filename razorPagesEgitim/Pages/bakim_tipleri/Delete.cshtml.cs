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
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DeleteModel(ApplicationDbContext db)
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

            BakimTipi = await _db.bakimTipi.FirstOrDefaultAsync(a => a.Id == id);

            if(BakimTipi == null)
            {
                return NotFound();
            }

            return Page();
        }
    
        public async Task<IActionResult> OnPostAsync()
        {
            if(BakimTipi == null)
            {
                return NotFound();
            }

            _db.bakimTipi.Remove(BakimTipi);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
