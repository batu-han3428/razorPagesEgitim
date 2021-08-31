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
                                                                     //SelectBox a ataca��m�z verileri olu�turduk

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
    
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                //Bak�m hizmetinin olu�turulma tarihini ald�k
                MakinaBakimHizmetiViewModel.BakimHizmetiGenel.EklendigiTarih = DateTime.Now;

                //bakimhizmetkart � sepet olarak d���nelim. sepetin i�inde ki bak�m tiplerini ald�k
                MakinaBakimHizmetiViewModel.BakimHizmetKart = _db.BakimHizmetKart.Include(a => a.BakimTipi)
                    .Where(a => a.MakinaId == MakinaBakimHizmetiViewModel.Makina.Id).ToList();
                

                //sepet i�inde ki bak�m tiplerinin fiyatlar�n� ald�k
                foreach (var item in MakinaBakimHizmetiViewModel.BakimHizmetKart)
                {
                    MakinaBakimHizmetiViewModel.BakimHizmetiGenel.ToplamFiyat += item.BakimTipi.BakimFiyati;

                }
                    
                 //bu bilgilerin hangi makina i�in yap�ld���n� aktard�k
                 MakinaBakimHizmetiViewModel.BakimHizmetiGenel.MakinaId = MakinaBakimHizmetiViewModel.Makina.Id;
               
                //yukar�da ki bilgileri database e att�k
                _db.BakimHizmetiGenel.Add(MakinaBakimHizmetiViewModel.BakimHizmetiGenel);
                await _db.SaveChangesAsync();

                //sepette ki bak�m tipinin detaylar�n� bakimhizmetdetay tablosuna ekledik
                foreach (var detay in MakinaBakimHizmetiViewModel.BakimHizmetKart)
                {
                    BakimHizmetiDetay bakimHizmetiDetay = new BakimHizmetiDetay
                    {
                        BakimHizmetiGenelId = MakinaBakimHizmetiViewModel.BakimHizmetiGenel.Id,
                        BakimAdi = detay.BakimTipi.BakimAdi,
                        BakimFiyati = detay.BakimTipi.BakimFiyati,
                        BakimTipiId = detay.BakimTipiId
                    }; 
                    
                    _db.BakimHizmetiDetay.Add(bakimHizmetiDetay);
                }

                //sepetten bilgileri sildik ve as�l tablolara ald���m�z verileri database e kaydettik
                _db.BakimHizmetKart.RemoveRange(MakinaBakimHizmetiViewModel.BakimHizmetKart);
                await _db.SaveChangesAsync();

                return RedirectToPage("../Makineler/Index", new { kullaniciId = MakinaBakimHizmetiViewModel.Makina.KullaniciId });
            }
            else
            {
                return Page();
            }
        }
    
        public async Task<IActionResult> OnPostKartaEklemeAsync()
        {
            //hangi makinaya hangi bak�m tipini ekleyece�imizi belirledik. sepete ekledik
            BakimHizmetKart objBakimKarti = new BakimHizmetKart
            {
                MakinaId = MakinaBakimHizmetiViewModel.Makina.Id,
                BakimTipiId = MakinaBakimHizmetiViewModel.BakimHizmetiDetay.BakimTipiId
            };

            _db.BakimHizmetKart.Add(objBakimKarti);
            await _db.SaveChangesAsync();

            return RedirectToPage("Create",new { makineId = MakinaBakimHizmetiViewModel.Makina.Id });
        }

        public async Task<IActionResult> OnPostKarttanSilmeAsync(int bakimTipiId)
        {
            //sepetten kay�tlar� sildik
            BakimHizmetKart objBakimKarti = _db.BakimHizmetKart.FirstOrDefault(a => a.MakinaId == MakinaBakimHizmetiViewModel.Makina.Id && a.BakimTipiId == bakimTipiId);

            _db.BakimHizmetKart.Remove(objBakimKarti);
            await _db.SaveChangesAsync();

            return RedirectToPage("Create", new { makineId = MakinaBakimHizmetiViewModel.Makina.Id });
        }

    }
}