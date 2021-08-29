using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorPagesEgitim.Data;
using razorPagesEgitim.Models;
using razorPagesEgitim.Models.ViewModel;
using razorPagesEgitim.Utility;

namespace razorPagesEgitim.Pages.Kullanicilar
{
    [Authorize(Roles = StatikRoller.AdminKullanici)]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public UsersListViewModel UsersListViewModel { get; set; }
        public async Task<IActionResult> OnGetAsync(int productPage = 1, string aramaAdSoyad = null, string aramaEmail = null, string aramaTelefon = null)
            //defaultta 1. sayfadan baþlasýn productPage. arama kýsýmlarý boþ gönderile bileceði için default deðerlerine null verdik
        {
            UsersListViewModel = new UsersListViewModel()
            {
                ApplicationUserList = await _db.applicationUser.ToListAsync()
            };

            StringBuilder param = new StringBuilder();
            param.Append("/Kullanicilar?productPage=:");//url yapýsýný oluþturduk
           
           
            param.Append("&aramaAdSoyad=");//aranan isim = den sonra gelecek
            if(aramaAdSoyad != null)
            {
                param.Append(aramaAdSoyad);
            }

            param.Append("&aramaEmail=");//aranan email = den sonra gelecek
            if (aramaEmail != null)
            {
                param.Append(aramaEmail);
            }

            param.Append("&aramaTelefon=");//aranan telefon = den sonra gelecek
            if (aramaTelefon != null)
            {
                param.Append(aramaTelefon);
            }


            if (aramaAdSoyad != null && aramaEmail != null && aramaTelefon != null)
            {
                UsersListViewModel.ApplicationUserList = await _db.applicationUser
                    .Where(a => a.adSoyad.ToLower().Contains(aramaAdSoyad.ToLower()) && a.Email.ToLower().Contains(aramaEmail.ToLower()) && a.PhoneNumber.ToLower().Contains(aramaTelefon.ToLower()))
                    .ToListAsync();
            } else if (aramaAdSoyad != null && aramaEmail != null)
            {
                UsersListViewModel.ApplicationUserList = await _db.applicationUser
                    .Where(a => a.adSoyad.ToLower().Contains(aramaAdSoyad.ToLower()) && a.Email.ToLower().Contains(aramaEmail.ToLower()))
                    .ToListAsync();
            }else if (aramaAdSoyad != null && aramaTelefon != null)
            {
                UsersListViewModel.ApplicationUserList = await _db.applicationUser
                    .Where(a => a.adSoyad.ToLower().Contains(aramaAdSoyad.ToLower()) && a.PhoneNumber.ToLower().Contains(aramaTelefon.ToLower()))
                    .ToListAsync();
            }else if (aramaEmail != null && aramaTelefon != null)
            {
                UsersListViewModel.ApplicationUserList = await _db.applicationUser
                    .Where(a => a.Email.ToLower().Contains(aramaEmail.ToLower()) && a.PhoneNumber.ToLower().Contains(aramaTelefon.ToLower()))
                    .ToListAsync();
            }else if (aramaAdSoyad != null)
            {
                UsersListViewModel.ApplicationUserList = await _db.applicationUser
                   .Where(a => a.adSoyad.ToLower().Contains(aramaAdSoyad.ToLower())).ToListAsync();
            }else if (aramaEmail != null)
            {
                UsersListViewModel.ApplicationUserList = await _db.applicationUser
                   .Where(a => a.Email.ToLower().Contains(aramaEmail.ToLower())).ToListAsync();
            }else if (aramaTelefon != null)
            {
                UsersListViewModel.ApplicationUserList = await _db.applicationUser
                  .Where(a => a.PhoneNumber.ToLower().Contains(aramaTelefon.ToLower())).ToListAsync();
            }


            var sayi = UsersListViewModel.ApplicationUserList.Count;//toplam sayfa bilgisi. her bir sayfa bir kullanýcý mantýðýyla gitti galiba

            UsersListViewModel.PagingInfo = new PagingInfo()
            {
                CurrentPage = productPage,//güncel sayfa
                ItemsPerPage = StatikRoller.KullaniciSayfalamaSayfaBoyutu,//her sayfada KullaniciSayfalamaSayfaBoyutun da ki sayý kadar kullanýcý gösterilsin
                TotalItems = sayi,//kullanýcýlarýn toplam sayýsý
                UrlParam = param.ToString()//url adresi
            };

            UsersListViewModel.ApplicationUserList = UsersListViewModel.ApplicationUserList
                .OrderBy(a => a.Email)//emaile göre sýrala
                .Skip((productPage - 1)* StatikRoller.KullaniciSayfalamaSayfaBoyutu)// 1 -1 = 0 * 5 = 0 yani ilk 0 kaydý gösterme dedik (productPage defaultta 1 vermiþtik. sonra kendisi 2,3,4 diye artacak. fakat nasýl artacak onu anlamadým)
                .Take(StatikRoller.KullaniciSayfalamaSayfaBoyutu)//skiple geçilen kayýttan baþlayarak ilk 5 kaydý göster dedik.
                .ToList();//listesini ver

            return Page();
        }
    }
}
