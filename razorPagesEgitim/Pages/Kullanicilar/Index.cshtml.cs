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
            //defaultta 1. sayfadan ba�las�n productPage. arama k�s�mlar� bo� g�nderile bilece�i i�in default de�erlerine null verdik
        {
            UsersListViewModel = new UsersListViewModel()
            {
                ApplicationUserList = await _db.applicationUser.ToListAsync()
            };

            StringBuilder param = new StringBuilder();
            param.Append("/Kullanicilar?productPage=:");//url yap�s�n� olu�turduk
           
           
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


            var sayi = UsersListViewModel.ApplicationUserList.Count;//toplam sayfa bilgisi. her bir sayfa bir kullan�c� mant���yla gitti galiba

            UsersListViewModel.PagingInfo = new PagingInfo()
            {
                CurrentPage = productPage,//g�ncel sayfa
                ItemsPerPage = StatikRoller.KullaniciSayfalamaSayfaBoyutu,//her sayfada KullaniciSayfalamaSayfaBoyutun da ki say� kadar kullan�c� g�sterilsin
                TotalItems = sayi,//kullan�c�lar�n toplam say�s�
                UrlParam = param.ToString()//url adresi
            };

            UsersListViewModel.ApplicationUserList = UsersListViewModel.ApplicationUserList
                .OrderBy(a => a.Email)//emaile g�re s�rala
                .Skip((productPage - 1)* StatikRoller.KullaniciSayfalamaSayfaBoyutu)// 1 -1 = 0 * 5 = 0 yani ilk 0 kayd� g�sterme dedik (productPage defaultta 1 vermi�tik. sonra kendisi 2,3,4 diye artacak. fakat nas�l artacak onu anlamad�m)
                .Take(StatikRoller.KullaniciSayfalamaSayfaBoyutu)//skiple ge�ilen kay�ttan ba�layarak ilk 5 kayd� g�ster dedik.
                .ToList();//listesini ver

            return Page();
        }
    }
}
