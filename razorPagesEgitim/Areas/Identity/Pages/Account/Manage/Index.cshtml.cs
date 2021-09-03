using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorPagesEgitim.Data;
using razorPagesEgitim.Models;

namespace razorPagesEgitim.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _db;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel//input model içine düzenlenmesini istediğimiz alanları yazdık
        {
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Telefon")]
            public string PhoneNumber { get; set; }

            [Required]
            public string AdSoyad { get; set; }

            public string Adres { get; set; }

            public string Sehir { get; set; }

            public string PostaKodu { get; set; }

        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var DbKullanici = await _db.applicationUser.FirstOrDefaultAsync(a => a.Email == user.Email);//sayfa yüklenirken kullanıcının bilgilerini çektik

            Username = DbKullanici.UserName;//bilgileri username propuna attık

            Input = new InputModel//input propuna bilgileri attık. bu sayede index.cshtml de bu veriler görüntülencek
            {
                AdSoyad = DbKullanici.adSoyad,
                Email = DbKullanici.Email,
                PhoneNumber = DbKullanici.PhoneNumber,
                Adres = DbKullanici.adres,
                Sehir = DbKullanici.sehir,
                PostaKodu = DbKullanici.postaKodu
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //load bölümünde ki işlemlerin aynısı. belki burada yapmaya gerek olmaya bilir ama yinede kalsın
            var DbKullanici = await _db.applicationUser.FirstOrDefaultAsync(a=>a.Email == user.Email);

            Username = DbKullanici.UserName;

            Input = new InputModel
            {
                AdSoyad = DbKullanici.adSoyad,
                Email = DbKullanici.Email,
                PhoneNumber = DbKullanici.PhoneNumber,
                Adres = DbKullanici.adres,
                Sehir = DbKullanici.sehir,
                PostaKodu = DbKullanici.postaKodu
            };

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //bilgileri güncellenecek olan kullanıcıyı çektik
            ApplicationUser DbKullanici = await _db.applicationUser.FirstOrDefaultAsync(a=>a.Email == user.Email);

            //yeni bilgileri atadık
            DbKullanici.adSoyad = Input.AdSoyad;
            DbKullanici.adres = Input.Adres;
            DbKullanici.sehir = Input.Sehir;
            DbKullanici.postaKodu = Input.PostaKodu;
            DbKullanici.PhoneNumber = Input.PhoneNumber;

            await _db.SaveChangesAsync();//veri tabanına kaydettik. ıdentity de db.add demeye gerek yok

            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //if (Input.PhoneNumber != phoneNumber)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        StatusMessage = "Unexpected error when trying to set phone number.";
            //        return RedirectToPage();
            //    }
            //}

            await _signInManager.RefreshSignInAsync(DbKullanici);//yeni bilgileri refresh ederek identitye kaydettik
            StatusMessage = "Profiliniz güncellenmiştir.";
            return RedirectToPage();
        }
    }
}
