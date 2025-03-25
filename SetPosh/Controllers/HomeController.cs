using Microsoft.AspNetCore.Mvc;
using Core.Model;
using Service.Service;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using DataBase;
using Core;
using System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SetPosh.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _UserService;

        public HomeController(ILogger<HomeController> logger, UserService UserService)
        {
            _UserService = UserService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string ReturnURL = "")
        {
            ReturnURL ??= Url.Content("~/");
            return View(new Tuple<UserModel, string>(new UserModel(), ReturnURL));
            //return RedirectToAction("Error", "Home", new { message = "خطای پایگاه داده: " + ex.Message });
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserModel UserModel, string? ReturnURL = "")
        {
            if (string.IsNullOrEmpty(ReturnURL))
                ReturnURL = Url.Content("/Home/Privacy");
            try
            {
                if (!ModelState.IsValid)
                    return View(new Tuple<UserModel, string>(UserModel, ReturnURL));

                QueryBuilder qb = _UserService.GetSimple();
                qb.AddEqualCondition(Dictionary.User.UTel.FullDBName, UserModel.UTel.SetSingleQuotes());
                qb.AddEqualCondition(Dictionary.User.UPass.FullDBName, UserModel.UPass.SetSingleQuotes());
                DataRow? dr = await DBConnection.GetDataRowAsync(qb);

                if (dr != null)
                {
                    UserModel user = new UserModel(dr);
                    ClaimsPrincipal claimsPrincipal = _UserService.CreateCookie(UserModel);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                    return LocalRedirect(ReturnURL);
                }
                else
                {
                    ViewBag.ErrorMessage = "شماره تلفن یا رمزعبور اشتباه است";
                    return View(new Tuple<UserModel, string>(UserModel, ReturnURL));
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error : {ex}";
                return View(new Tuple<UserModel, string>(UserModel, ReturnURL));
                //return RedirectToAction("Error", new { message = "خطای پایگاه داده: " + ex.Message });
            }
        }

        public IActionResult Error(string message)
        {
            return View(message);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}