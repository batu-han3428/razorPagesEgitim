using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using razorPagesEgitim.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace razorPagesEgitim.ViewComponents
{
    //loginpartialin pagemodelı olmadığı için component mantığı ile login partial e veri çekip gönderdik
    public class KullaniciAdiViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public KullaniciAdiViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var dbKullanici = await _db.applicationUser.FirstOrDefaultAsync(a => a.Id == claims.Value);

            return View(dbKullanici);
        }

    }
}
