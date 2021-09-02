using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using razorPagesEgitim.Data;
using razorPagesEgitim.Models;
using razorPagesEgitim.Utility;

namespace razorPagesEgitim.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;//rol atamak için ekledim
        private readonly ApplicationDbContext _db;//identitynin hazır tablosuna fieldlar ekledim. onlara veri atmak için ekledim

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string adSoyad { get; set; }
            public string adres { get; set; }
            public string sehir { get; set; }
            public string postaKodu { get; set; }
            [Required]
            public string PhoneNumber { get; set; }

            public bool Admin { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {
                    UserName = Input.Email, 
                    Email = Input.Email,
                    adSoyad = Input.adSoyad,
                    adres = Input.adres,
                    sehir = Input.sehir,
                    postaKodu = Input.postaKodu,
                    PhoneNumber = Input.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(StatikRoller.AdminKullanici))//role tablosunda bu rol yoksa if içine gir
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StatikRoller.AdminKullanici));//adminkullanıcı rolünü oluştur
                    }

                    if (!await _roleManager.RoleExistsAsync(StatikRoller.MusteriKullanici))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StatikRoller.MusteriKullanici));
                    }


                    //if (_userManager.Users.Count() == 1)//ilk kaydın admin olarak kaydedilmesi için
                    //{
                    //    await _userManager.AddToRoleAsync(user, StatikRoller.AdminKullanici);
                    //}
                    //else//sonra ki kayıtlar müşteri
                    //{
                    //    await _userManager.AddToRoleAsync(user, StatikRoller.MusteriKullanici);
                    //}


                    if (Input.Admin)//checkbox admin olarak işaretlendiyse
                    {
                        await _userManager.AddToRoleAsync(user, StatikRoller.AdminKullanici);

                        //e posta doğrulama kodu
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        return RedirectToPage("/Kullanicilar/Index");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, StatikRoller.MusteriKullanici);

                        //await _signInManager.SignInAsync(user, isPersistent: false); //admin, müşteri kaydı oluşturduğunda oluşturduğu hesaba oto giriş
                        //yaptığı için bu kısmı yorum satırına aldık. ve altta ki if bloğunu yazdık

                        if (User.IsInRole(StatikRoller.AdminKullanici))
                        {
                            //e posta doğrulama kodu
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                            var callbackUrl = Url.Page(
                                "/Account/ConfirmEmail",
                                pageHandler: null,
                                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                                protocol: Request.Scheme);

                            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


                            return RedirectToPage("/Kullanicilar/Index");
                        }
                        else
                        {
                            //e posta doğrulama kodu
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                            var callbackUrl = Url.Page(
                                "/Account/ConfirmEmail",
                                pageHandler: null,
                                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                                protocol: Request.Scheme);

                            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                            return LocalRedirect(returnUrl);
                        }

                        
                    }

                    //_logger.LogInformation("User created a new account with password.");

                    

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    //}
                    //else
                    //{
                    //    await _signInManager.SignInAsync(user, isPersistent: false);
                    //    return LocalRedirect(returnUrl);
                    //}
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
