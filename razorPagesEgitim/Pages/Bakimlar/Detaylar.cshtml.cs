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

namespace razorPagesEgitim.Pages.Bakimlar
{
    [Authorize]
    public class DetaylarModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public BakimHizmetiGenel BakimHizmetiGenel { get; set; }

        public List<BakimHizmetiDetay> BakimHizmetiDetay { get; set; }

        public DetaylarModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet(int bakimid)
        {
            BakimHizmetiGenel = _db.BakimHizmetiGenel.Include(a=>a.Makina).Include(a=>a.Makina.ApplicationUser).FirstOrDefault(a=>a.Id == bakimid);

            BakimHizmetiDetay = _db.BakimHizmetiDetay.Where(a=>a.BakimHizmetiGenelId == bakimid).ToList();
            
        }
    }
}
