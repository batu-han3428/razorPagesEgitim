using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorPagesEgitim.Data;
using razorPagesEgitim.Models;
using razorPagesEgitim.Utility;

namespace razorPagesEgitim.Pages.bakim_tipleri
{
    [Authorize(Roles = StatikRoller.AdminKullanici)]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public bakimTipi BakimTipi { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync(/*bakimTipi BakimTipi*/)//yukarýda BakýmTipi propertysine
        //[BindProperty] yazarak post methoduna onu baðlamýþ oldum. bu sayede parametre olarak tekrar yazmama gerek kalmadý
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _db.bakimTipi.Add(BakimTipi);

            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
