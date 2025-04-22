using Core;
using Core.Model;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Data;
using System.Security.Claims;

namespace SetPosh.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartService _shoppingCartService;
        private readonly ShoppingCartDetailService _shoppingCartDetailService;
        public ShoppingCartController(ShoppingCartService ShoppingCartService, ShoppingCartDetailService shoppingCartDetailService)
        {
            _shoppingCartService = ShoppingCartService;
            _shoppingCartDetailService = shoppingCartDetailService;
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(long PSID)
        {
            try
            {
                if (!(User.Identity?.IsAuthenticated ?? false))
                    return LocalRedirect($"/Auth/{nameof(AuthController.Login)}");

                string? USID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(USID))
                {
                    ShoppingCartDetailModel shoppingCartDetail = new ShoppingCartDetailModel();
                    shoppingCartDetail.ShoppingCart.User.SID = USID.ConvertToLong();
                    shoppingCartDetail.PSID = PSID;
                    await _shoppingCartDetailService.AddAsync(shoppingCartDetail);
                    return Json(new { success = true, SCDCount = 1 });
                }

                return RedirectToAction(nameof(ProductController.ProductDetails), "Product", new { PSID = PSID });
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "ShoppingCart.AddToCart");
                throw;
            }
        }
        [HttpPost]
        public async Task<ActionResult> IncreaseSCDCount(long PSID, int count)
        {
            try
            {
                if (!(User.Identity?.IsAuthenticated ?? false))
                    return LocalRedirect($"/Auth/{nameof(AuthController.Login)}");

                string? USID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(USID))
                {
                    long SCDCount = await _shoppingCartDetailService.ChangeSCDCount(USID.ConvertToLong(), PSID, true);
                    return Json(new { success = true, SCDCount = SCDCount });
                }
                return LocalRedirect($"/Auth/{nameof(AuthController.Login)}");
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "ShoppingCart.IncreaseSCDCount");
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult> DecreaseSCDCount(long PSID, int count)
        {
            try
            {
                if (!(User.Identity?.IsAuthenticated ?? false))
                    return LocalRedirect($"/Auth/{nameof(AuthController.Login)}");

                string? USID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(USID))
                {
                    long SCDCount = await _shoppingCartDetailService.ChangeSCDCount(USID.ConvertToLong(), PSID, false);
                    return Json(new { success = true, SCDCount = SCDCount });
                }
                return LocalRedirect($"/Auth/{nameof(AuthController.Login)}");
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "ShoppingCart.DecreaseSCDCount");
                return Json(new { success = false, message = "خطای سرور: " + ex.Message });
            }
        }

        


    }
}
