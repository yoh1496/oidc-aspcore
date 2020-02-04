using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using oidc_aspcore.Models;
using oidc_aspcore.Services;

namespace oidc_aspcore.Controllers {
    public class LoginController : Controller {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserService _userService;

        public LoginController(ILogger<LoginController> logger, IUserService userService) {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult Index([FromForm] LoginViewModel model, [FromQuery] string ReturnUrl) {
            if (!ModelState.IsValid) return View(model);

            try {
                var loginedUser = _userService.getUser(model.loginId, model.password);
                if (loginedUser != null) {
                    // login success
                    this.login(loginedUser).Wait();
                } else {
                    ModelState.AddModelError("LoginError", "ユーザーIDかパスワードが違います。");
                    return View(model);
                }
            } catch {
                ModelState.AddModelError("LoginError", "ログイン中にエラーが発生しました。");
                return View(model);
            }

            if (ReturnUrl != null) {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private Task login(User _user) {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, _user.loginId),
            };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme
            );

            return HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties {
                    IsPersistent = true,
                }
            );
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}