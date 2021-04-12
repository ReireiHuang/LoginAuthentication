using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestProject.Biz.Account;
using TestProject.Models.Account;
using TestProject.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;


namespace TestProject.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize(Policy = "Users")]
    public class HomeController : Controller
    {
        private IConfiguration appconfig;
        public HomeController(IConfiguration appconfig)
        {
            this.appconfig = appconfig;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel user,string rtnurl = null)
        {
            try
            {
                using (AccountManager AM = new AccountManager())
                {
                    User users = new User();
                    users = AM.AccountVerify(user);
                    if (users != null)
                    {
                        //以帳號為依據設定於Client Cookie
                        Claim[] claims = new[] { new Claim(ClaimTypes.Role, users.Role) };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);    //權限
                        //執行登入，相當於以前的FormsAuthentication.SetAuthCookie()
                        //從組態讀取登入逾時設定
                        double loginExpireMinute = this.appconfig.GetValue<double>("LoginExpireMinute");
                        await HttpContext.SignInAsync(principal, new AuthenticationProperties()
                        {
                            IsPersistent = false,
                            ExpiresUtc = DateTime.Now.AddMinutes(loginExpireMinute)
                        });

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.errMsg = "帳號密碼錯誤";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                return View();
            }
            
        }

        [Authorize(Policy ="Company")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminUse()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login","Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
