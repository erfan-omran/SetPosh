using Core.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Service.Service;
using System.Data;
using System.Security.Claims;

namespace SetPosh.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login(string returnURL = "")
        {
            returnURL ??= Url.Content("~/");
            return View(new Tuple<UserModel, string>(new UserModel(), returnURL));
            //return RedirectToAction("Error", "Home", new { message = "خطای پایگاه داده: " + ex.Message });
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel userModel, string? returnURL = "")
        {
            if (string.IsNullOrEmpty(returnURL))
                returnURL = Url.Content("/Home/Privacy");
            try
            {
                if (!ModelState.IsValid)
                    return View(new Tuple<UserModel, string>(userModel, returnURL));

                DataRow dr = await _userService.Login(userModel);
                if (dr != null)
                {
                    UserModel User = new UserModel(dr);
                    ClaimsPrincipal claimsPrincipal = _userService.CreateCookie(User, Settings.AuthCookieName);
                    AuthenticationProperties AuthProps = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = Settings.CookieExpierTime
                    };
                    await HttpContext.SignInAsync(Settings.AuthCookieName, claimsPrincipal, AuthProps);
                }
                else
                {
                    ViewBag.ErrorMessage = "شماره تلفن یا رمزعبور اشتباه است";
                    return View(new Tuple<UserModel, string>(userModel, returnURL));
                }
                return LocalRedirect(returnURL);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error : {ex}";
                return View(new Tuple<UserModel, string>(userModel, returnURL));
                //return RedirectToAction("Error", new { message = "خطای پایگاه داده: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Settings.AuthCookieName);
            return LocalRedirect("/Home/Index");
        }
        
        public IActionResult Register(string? ReturnURL = "")
        {
            ReturnURL ??= Url.Content("~/");
            return View(new Tuple<UserModel, string>(new UserModel(), ReturnURL)); // ارسال تاپل به ویو
        }

        [HttpPost]
        public IActionResult Register(UserModel model, string? ReturnURL = "")
        {
            if (string.IsNullOrEmpty(ReturnURL))
                ReturnURL = Url.Content("/Home/Privacy");
            if (ModelState.IsValid)
            {

                // بررسی صحت اطلاعات و ثبت‌نام در دیتابیس
                // به عنوان مثال، ذخیره‌سازی داده‌ها در دیتابیس

                // فرض کنید نام کاربر و رمز عبور را ذخیره کرده‌ایم
                // شما باید این بخش را با کد خودتان برای ذخیره‌سازی دیتابیس کامل کنید

                // نمایش پیامی مبنی بر موفقیت
                TempData["SuccessMessage"] = "ثبت نام با موفقیت انجام شد!";
                return RedirectToAction("Login"); // هدایت به صفحه ورود بعد از ثبت‌نام موفق
            }
            else
            {
                // در صورتی که اطلاعات وارد شده معتبر نباشد
                return View(new Tuple<UserModel, string>(model, ReturnURL));
            }
        }
    }
}
