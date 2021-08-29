using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorPagesEgitim.Data;
using razorPagesEgitim.Models;

namespace razorPagesEgitim.Pages.Kullanicilar
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id.Trim().Length == 0)
            {
                return NotFound();
            }

            ApplicationUser = await _db.applicationUser.FirstOrDefaultAsync(a => a.Id == id);

            if (ApplicationUser == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                var kullaniciDb = await _db.applicationUser.SingleOrDefaultAsync(x => x.Id == ApplicationUser.Id);

                if (kullaniciDb == null)
                    return NotFound();
                else
                {
                    kullaniciDb.adSoyad = ApplicationUser.adSoyad;
                    kullaniciDb.PhoneNumber = ApplicationUser.PhoneNumber; 
                    kullaniciDb.adres = ApplicationUser.adres; 
                    kullaniciDb.sehir = ApplicationUser.sehir;
                    kullaniciDb.postaKodu = ApplicationUser.postaKodu;

                    await _db.SaveChangesAsync();

                    return RedirectToPage("Index");
                }
            }

        }
    }
}
