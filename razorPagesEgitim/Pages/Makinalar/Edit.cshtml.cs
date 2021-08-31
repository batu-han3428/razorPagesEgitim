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

namespace razorPagesEgitim.Pages.Makinalar
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Makina Makina { get; set; }

        [TempData]
        public string DurumMesaj { get; set; }

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnGetAsync(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            Makina = await _db.Makina.Include(a => a.ApplicationUser).FirstOrDefaultAsync(b => b.Id == Id);

            if (Makina == null)
            {
                return NotFound();
            }
            else
            {
                return Page();
            }

        }
   
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _db.Attach(Makina).State = EntityState.Modified;//T�m fieldlar g�ncellensin diye attach yapt�k
            await _db.SaveChangesAsync();
            DurumMesaj = "Makine bilgisi ba�ar�l� bir �ekilde g�ncellenmi�tir!";
            return RedirectToPage("./Index", new { kullaniciId = Makina.KullaniciId });
        }
    }
}
