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
using razorPagesEgitim.Models.ViewModel;
using razorPagesEgitim.Utility;

namespace razorPagesEgitim.Pages.Bakimlar
{
    [Authorize(Roles = StatikRoller.AdminKullanici)]
    public class CreateModel : PageModel
    {

        private readonly ApplicationDbContext _db;

        [BindProperty]
        public MakinaBakimHizmetiViewModel MakinaBakimHizmetiViewModel { get; set; }

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnGetAsync(int makineId)
        {
            MakinaBakimHizmetiViewModel = new MakinaBakimHizmetiViewModel
            {
                Makina = await _db.Makina.Include(a => a.ApplicationUser).FirstOrDefaultAsync(a => a.Id == makineId),
                BakimHizmetiGenel = new BakimHizmetiGenel()
            };

            List<string> BakimHizmetiKartIcinBakimTipleriListesi = _db.BakimHizmetKart
                                                                    .Include(a => a.BakimTipi)
                                                                    .Where(a=>a.MakinaId == makineId)
                                                                    .Select(a=>a.BakimTipi.BakimAdi).ToList();
                                                                     //SelectBox a atacaðýmýz verileri oluþturduk

            IQueryable<bakimTipi> BakimListesi = from x in _db.bakimTipi where
                                                 !(BakimHizmetiKartIcinBakimTipleriListesi.Contains(x.BakimAdi))
                                                 select x;

            MakinaBakimHizmetiViewModel.BakimTipleriListesi = BakimListesi.ToList();

            MakinaBakimHizmetiViewModel.BakimHizmetKart = _db.BakimHizmetKart.Include(a => a.BakimTipi).Where(a=>a.MakinaId == makineId).ToList();

            MakinaBakimHizmetiViewModel.BakimHizmetiGenel.ToplamFiyat = 0;

            foreach (var item in MakinaBakimHizmetiViewModel.BakimHizmetKart)
            {
                MakinaBakimHizmetiViewModel.BakimHizmetiGenel.ToplamFiyat += item.BakimTipi.BakimFiyati;
            }

            return Page();
        }
    }
}
