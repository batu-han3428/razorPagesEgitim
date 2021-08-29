using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using razorPagesEgitim.Data;
using razorPagesEgitim.Models;
using razorPagesEgitim.Utility;

namespace razorPagesEgitim.Pages.bakim_tipleri
{
    [Authorize(Roles = StatikRoller.AdminKullanici)]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public bakimTipi bakimTipi { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            bakimTipi = await _db.bakimTipi.FirstOrDefaultAsync(m => m.Id == id);

            if (bakimTipi == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_db.Attach(bakimTipi).State = EntityState.Modified; 

            //yukarıda ki kod ilgili id nin tüm field alanlarını günceller. 90 field alanının
            //sadece 5 tanesini güncellemek istersek yinede 90 field alanını da günceller.
            //Gereksiz yere veritabanını yormuş oluruz. o yüzden bu kod yerine aşağıda ki kodu yazıyorum

            var bakimFromDb = await _db.bakimTipi.FirstOrDefaultAsync(a => a.Id == bakimTipi.Id);

            bakimFromDb.BakimAdi = bakimTipi.BakimAdi;
            bakimFromDb.BakimFiyati = bakimTipi.BakimFiyati;

            //yukari da tüm fieldları güncelledik fakat isteseydik adı atamayıp sadece fiyatı atardık.
            //ve böylelikle sadece fiyat güncellenmiş olurdu. Diğer field alanlarını veritabanı aramamış olurdu
            //tasarım kısmında da güncellemek istediğin alanları checkbox ile seçme özelliği verilebilir

            await _db.SaveChangesAsync();

        //    try
        //    {
        //        await _db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!bakimTipiExists(bakimTipi.Id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

           return RedirectToPage("./Index");
        }

        //private bool bakimTipiExists(int id)
        //{
        //    return _db.bakimTipi.Any(e => e.Id == id);
        //}
    }
}
