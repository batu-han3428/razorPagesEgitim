using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using razorPagesEgitim.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace razorPagesEgitim.Pages
{
    public class IndexModel : PageModel
    {

        public IActionResult OnGet()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }else if (User.IsInRole(StatikRoller.AdminKullanici))
            {
                return RedirectToPage("/Kullanicilar/Index");
            }

            return RedirectToPage("/Makinalar/Index");
        }
    }
}
