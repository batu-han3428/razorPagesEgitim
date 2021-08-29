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

namespace razorPagesEgitim.Pages.Kullanicilar
{
    [Authorize(Roles = StatikRoller.AdminKullanici)]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ApplicationUser applicationUser { get; set; }

        public async Task<IActionResult> OnGet(string id)
        {
            if (id.Trim().Length == 0)
            {
                return NotFound();
            }

            applicationUser = await _db.applicationUser.FirstOrDefaultAsync(a => a.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

                var kullaniciDb = await _db.applicationUser.SingleOrDefaultAsync(x => x.Id == applicationUser.Id);

                if (kullaniciDb == null)
                    return NotFound();
                else
                {

                    _db.Users.Remove(kullaniciDb);
                    await _db.SaveChangesAsync();

                    return RedirectToPage("Index");
                }
        }
    }
}
