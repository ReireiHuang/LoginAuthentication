using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Authentication.Cookies;

namespace TestProject
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
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.addcon
            //從組態讀取登入逾時設定
            double loginExpireMinute = this.Configuration.GetValue<double>("LoginExpireMinute");
            //註冊 CookieAuthentication，Scheme必填
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(option =>
            {
                //或許要從組態檔讀取，自己斟酌決定
                option.LoginPath = new PathString("/Home/Login"); //登入Action
                option.LogoutPath = new PathString("/Home/Logout"); //登出Action
                //用戶頁面停留太久，登入逾期，或Controller中用戶登入時機點也可以設定
                option.ExpireTimeSpan = TimeSpan.FromMinutes(loginExpireMinute); //沒給預設14天
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Users", policy =>
                    policy.RequireRole("User", "Account", "Admin"));
                options.AddPolicy("Company", policy =>
                    policy.RequireRole("Account", "Admin"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            //留意先執行驗證...
            app.UseAuthentication();
            //再執行Route，如此順序程式邏輯才正確
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Login}/{id?}");
            });
        }
    }
}
