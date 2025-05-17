using Core;
using Core.Model;
using DataBase;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ProductService _productService;
        public ShoppingCartController(ShoppingCartService ShoppingCartService, ShoppingCartDetailService shoppingCartDetailService, ProductService productService)
        {
            _shoppingCartService = ShoppingCartService;
            _shoppingCartDetailService = shoppingCartDetailService;
            _productService = productService;
        }

        public async Task<ActionResult> AddToCart(long PSID)
        {
            try
            {
                if (!(User.Identity?.IsAuthenticated ?? false))
                    return Json(new { success = false, message = "-1" });

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
        public async Task<ActionResult> IncreaseSCDCount(long PSID, int count)
        {
            try
            {
                if (!(User.Identity?.IsAuthenticated ?? false))
                    return Json(new { success = false, message = "-1" });

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
        public async Task<ActionResult> DecreaseSCDCount(long PSID, int count)
        {
            try
            {
                if (!(User.Identity?.IsAuthenticated ?? false))
                    return Json(new { success = false, message = "-1" });

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


        public async Task<ActionResult> ShoppingCartDetail()
        {
            try
            {
                string? USID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(USID))
                    return RedirectToAction(nameof(AuthController.Login), "Auth");

                QueryBuilder ShoppingCartQB = new QueryBuilder();
                ShoppingCartQB.AddColumn(Dictionary.ShoppingCart.SID.FullDBName);
                ShoppingCartQB.SetTable(Dictionary.ShoppingCart.TableName);
                ShoppingCartQB.AddEqualCondition(Dictionary.ShoppingCart.USID.FullDBName, USID);
                ShoppingCartQB.AddEqualCondition(Dictionary.ShoppingCart.IsActive.FullDBName, 1);
                ShoppingCartQB.AddEqualCondition(Dictionary.ShoppingCart.Blocked.FullDBName, 0);
                ShoppingCartQB.AddEqualCondition(Dictionary.ShoppingCart.Deleted.FullDBName, 0);

                QueryBuilder MainQB = _productService.GetWithMainImage();
                MainQB.AddColumn(Dictionary.ShoppingCartDetail.PSID.FullDBName);
                MainQB.AddColumn(Dictionary.ShoppingCartDetail.SCDCount.FullDBName);
                MainQB.AddWith(ShoppingCartQB, nameof(ShoppingCartQB));

                MainQB.AddInnerJoin(Dictionary.ShoppingCartDetail.TableName, qb =>
                {
                    qb.AddEqualCondition(Dictionary.Product.SID.FullDBName, Dictionary.ShoppingCartDetail.PSID.FullDBName);
                });
                MainQB.AddInnerJoin(nameof(ShoppingCartQB), qb =>
                {
                    qb.AddEqualCondition(Dictionary.ShoppingCartDetail.SCSID.FullDBName, $"{nameof(ShoppingCartQB)}.SID");
                });

                string Query = MainQB.CreateQuery();
                DataTable DT = await DBConnection.GetDataTableAsync(Query);
                List<ShoppingCartDetailModel> SCDList = _shoppingCartDetailService.MapDTToModel(DT);
                List<ProductModel> ProductList = _productService.MapDTToModel(DT);

                decimal FinalPrice = 0;
                if (SCDList != null)
                {
                    for (int i = 0; i < SCDList.Count; i++)
                    {
                        ProductModel? CurrentSCDProduct = ProductList.FirstOrDefault(Product => Product.SID == SCDList[i].PSID);
                        FinalPrice += SCDList[i].SCDCount * CurrentSCDProduct?.PPrice ?? -1;
                    }
                }

                return View(new Tuple<List<ShoppingCartDetailModel>, List<ProductModel>, decimal>(SCDList, ProductList, FinalPrice));
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "ShoppingCartDetail");
                throw;
            }
        }
    }
}
