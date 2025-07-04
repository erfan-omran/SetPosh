﻿using Core.Enum;
using Core.Model;
using DataBase;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;

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
                returnURL = Url.Content(Settings.DefaultReturnUrl);
            try
            {
                if (!ModelState.IsValid)
                    return View(new Tuple<UserModel, string>(userModel, returnURL));

                if (!Regex.IsMatch(userModel.UTel, "09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}"))
                {
                    ViewBag.ErrorMessage = "شماره تلفن اشتباه است";
                    return View(new Tuple<UserModel, string>(userModel, returnURL));
                }

                DataRow dr = await _userService.Login(userModel);
                if (dr != null)
                {
                    UserModel User = new UserModel(dr);
                    if (User.Blocked)
                    {
                        ViewBag.ErrorMessage = "حساب کاربری شما مسدود شده است";
                        return View(new Tuple<UserModel, string>(userModel, returnURL));
                    }
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
                ViewBag.ErrorMessage = $"{ex.Message}";
                return View(new Tuple<UserModel, string>(userModel, returnURL));
                //return RedirectToAction("Error", new { message = "خطای پایگاه داده: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Settings.AuthCookieName);
            return LocalRedirect($"/Home/{nameof(HomeController.Index)}");
        }

        public IActionResult Register(string? ReturnURL = "")
        {
            ReturnURL ??= Url.Content("~/");
            return View(new Tuple<UserModel, string>(new UserModel(), ReturnURL));
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel userModel, string? returnURL = "")
        {
            if (string.IsNullOrEmpty(returnURL))
                returnURL = Url.Content(Settings.DefaultReturnUrl);

            try
            {
                if (!ModelState.IsValid)
                    return View(new Tuple<UserModel, string>(userModel, returnURL));

                if (!Regex.IsMatch(userModel.UTel, "09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}"))
                {
                    ViewBag.ErrorMessage = "شماره تلفن اشتباه است";
                    return View(new Tuple<UserModel, string>(userModel, returnURL));
                }

                userModel.UTSID = (int)UserTypeEnum.Normal;
                userModel.UName = _userService.GenerateRandomUsername();

                bool Ans = false;
                try { Ans = await _userService.AddAsync(userModel); }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"{ex.Message}";//پیام دیتابیس
                    return View(new Tuple<UserModel, string>(userModel, returnURL));
                }

                if (Ans) { return RedirectToAction(nameof(AuthController.Login)); }
                else
                {
                    ViewBag.ErrorMessage = "عملیات با مشکل مواجه شد. لطفاً دوباره تلاش کنید";
                    return View(new Tuple<UserModel, string>(userModel, returnURL));
                }
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex, "");
                ViewBag.ErrorMessage = "عملیات با مشکل مواجه شد. لطفاً دوباره تلاش کنید";
                return View(new Tuple<UserModel, string>(userModel, returnURL));
            }
        }
    }
}
