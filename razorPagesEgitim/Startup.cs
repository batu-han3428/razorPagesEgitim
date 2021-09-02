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
                options.CheckConsentNeeded = context => true;//kullanýcýnýn çerez politikasýný kabul edip etmeyeceðini sor.(context yerine baþka isimde verebilirdik)
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            /*
                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)  
            
                //kullanýcý her oturumunu açtýðýnda kimlik doðrulamasý yaptýrýyor. mesela e-postasýna doðrulama maili göndermek gibi
                //ben sadece kayýt olurken doðrulama olsun istiyorum. her oturumuna giriþ yaptýðýnda deðil. o yüzden yorum satýrýna aldýðým kod üzerinde deðiþiklik yapýyorum
            */
            services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();//yukarýda ki yorum satýrýna aldýðýmýz kodu bu þekilde düzelttik. sadece kayýt olurken doðrulama istenecek


            services.AddSingleton<IEmailSender, EmailGonderici>();
            services.Configure<EmailOptions>(Configuration);



            services.AddAuthentication().AddFacebook(fb =>
            {
                fb.AppId = "324855876087568";
                fb.AppSecret = "df4fdda15f880443ef3d0630df4244fc";
            });

            
            services.AddRazorPages().AddRazorRuntimeCompilation();//nugetten microsoft.aspnetcore.mvc.razor.runtimecompilation indirdik.
            //addrazorPagesin devamýna addrazorruntimecompilation u ekledik. 
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
