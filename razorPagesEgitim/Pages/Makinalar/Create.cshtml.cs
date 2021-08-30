using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorPagesEgitim.Data;
using razorPagesEgitim.Models;

namespace razorPagesEgitim.Pages.Makinalar
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string DurumMesaj { get; set; }

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Makina Makina { get; set; }

        public IActionResult OnGet(string userId = null)
        {
            Makina = new Makina();

            if (userId == null)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                userId = claim.Value;
            }

            Makina.KullaniciId = userId;
                
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
                 _db.Makina.Add(Makina);
                await _db.SaveChangesAsync();
                DurumMesaj = "Makine bilgileri baþarýlý bir þekilde kaydedilmiþtir!";

                return RedirectToPage("Index", new { kullaniciId = Makina.KullaniciId });
            }

        }
    }
}
