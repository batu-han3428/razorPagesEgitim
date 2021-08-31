using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorPagesEgitim.Data;
using razorPagesEgitim.Models.ViewModel;

namespace razorPagesEgitim.Pages.Makinalar
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public KullaniciMakinaViewModel KullaniciMakinaViewModel { get; set; }

        [TempData]
        public string DurumMesaj { get; set; }

        public async Task<IActionResult> OnGetAsync(string kullaniciID = null)
        {
            if (kullaniciID == null)//kendisi giriþ yaparsa kullaniciýd bilgisi gelmeyeceði için bu kýsmý yapmýþýz
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                kullaniciID = claim.Value;
            }

            KullaniciMakinaViewModel = new KullaniciMakinaViewModel()
            {
                Makinalar = await _db.Makina.Where(a => a.KullaniciId == kullaniciID).ToListAsync(),
                KullaniciObj = await _db.applicationUser.FirstOrDefaultAsync(a=>a.Id == kullaniciID)
            };

            return Page();
        }
    }
}
