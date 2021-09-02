using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using razorPagesEgitim.Data;
using razorPagesEgitim.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace razorPagesEgitim
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;//kullan�c�n�n �erez politikas�n� kabul edip etmeyece�ini sor.(context yerine ba�ka isimde verebilirdik)
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            /*
                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)  
            
                //kullan�c� her oturumunu a�t���nda kimlik do�rulamas� yapt�r�yor. mesela e-postas�na do�rulama maili g�ndermek gibi
                //ben sadece kay�t olurken do�rulama olsun istiyorum. her oturumuna giri� yapt���nda de�il. o y�zden yorum sat�r�na ald���m kod �zerinde de�i�iklik yap�yorum
            */
            services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();//yukar�da ki yorum sat�r�na ald���m�z kodu bu �ekilde d�zelttik. sadece kay�t olurken do�rulama istenecek


            services.AddSingleton<IEmailSender, EmailGonderici>();
            services.Configure<EmailOptions>(Configuration);



            services.AddAuthentication().AddFacebook(fb =>
            {
                fb.AppId = "324855876087568";
                fb.AppSecret = "df4fdda15f880443ef3d0630df4244fc";
            });

            
            services.AddRazorPages().AddRazorRuntimeCompilation();//nugetten microsoft.aspnetcore.mvc.razor.runtimecompilation indirdik.
            //addrazorPagesin devam�na addrazorruntimecompilation u ekledik. 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
