using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace razorPagesEgitim.Areas.Identity.Pages.Account
{
    public class EmailDogrulaModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }


        public IActionResult OnGet(string id)
        {
            Email = id;

            return Page();
        }
    }
}
