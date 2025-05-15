using Core;
using Core.Enum;
using Core.Model;
using DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Data;

namespace SetPosh.Controllers
{
    [Authorize(nameof(UserTypeEnum.Admin))]
    public class AdminController : Controller
    {
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly ProductCategoryService _productCategoryService;
        public AdminController(UserService userService, ProductService productService, ProductCategoryService productCategoryService)
        {
            _userService = userService;
            _productService = productService;
            _productCategoryService = productCategoryService;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        #region UserManager
        public async Task<IActionResult> UserManager()
        {
            try
            {
                QueryBuilder qb = _userService.GetWithRelatedEntities();
                qb.AddEqualCondition(Dictionary.User.Deleted.FullDBName, 0);
                //qb.AddValidationCondition(Dictionary.User.TableName);
                string Query = qb.CreateQuery();
                DataTable DT = await DBConnection.GetDataTableAsync(Query);

                List<UserModel> UserList = _userService.MapDTToModel(DT);
                return View(UserList);
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "UserManager");
                ViewBag.ErrorMessage = "عملیات با مشکل مواجه شد. لطفاً دوباره تلاش کنید : " + ex.Message;
                return RedirectToAction(nameof(Dashboard));
            }
        }
        public async Task<IActionResult> UserAddEdit(long SID)
        {
            try
            {
                UserModel UserIcon = new UserModel();
                bool IsAdd = true;

                if (SID > 0)
                {
                    IsAdd = false;
                    UserIcon = await _userService.GetModelSimpleAsync(SID);
                    if (UserIcon.SID <= 0)
                    {
                        ViewBag.ErrorMessage = "کاربر پیدا نشد";
                        return RedirectToAction(nameof(UserManager));
                    }
                }

                return View(new Tuple<UserModel, bool>(UserIcon, IsAdd));
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "UserAddEdit");
                ViewBag.ErrorMessage = "عملیات با مشکل مواجه شد. لطفاً دوباره تلاش کنید : " + ex.Message;
                return RedirectToAction(nameof(UserManager));
            }
        }
        public async Task<JsonResult> UserAdd(UserModel User)
        {
            try
            {
                User.UFirstName = User.UFirstName ?? string.Empty;
                User.ULastName = User.ULastName ?? string.Empty;
                User.UEmail = User.UEmail ?? string.Empty;
                bool Ans = await _userService.AddAsync(User);
                if (!Ans)
                    return Json(new { success = false, message = "ثبت با مشکل مواجه شد" });
                return Json(new { success = true, message = "کاربر با موفقیت ثبت شد" });
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "UserAdd");
                return Json(new { success = false, message = "عملیات با مشکل مواجه شد. لطفاً دوباره تلاش کنید : " + ex.Message });
            }
        }
        public async Task<JsonResult> UserEdit(UserModel User)
        {
            try
            {
                User.UFirstName = User.UFirstName ?? string.Empty;
                User.ULastName = User.ULastName ?? string.Empty;
                User.UEmail = User.UEmail ?? string.Empty;
                bool Ans = await _userService.EditAsync(User);
                if (!Ans)
                    return Json(new { success = false, message = "ویرایش با مشکل مواجه شد" });
                return Json(new { success = true, message = "کاربر با موفقیت ویرایش شد" });
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "UserEdit");
                return Json(new { success = false, message = "عملیات با مشکل مواجه شد. لطفاً دوباره تلاش کنید : " + ex.Message });
            }
        }

        public async Task<JsonResult> UserBlock(long SID)
        {
            try
            {
                bool ans = await _userService.BlockAsync(SID);
                if (ans)
                    return Json(new { success = true, message = "کاربر با موفقیت مسدود شد" });
                else
                    return Json(new { success = false, message = "مسدود کردن کاربر با مشکل مواجه شد" });
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "UserBlock");
                return Json(new { success = false, message = "عملیات با مشکل مواجه شد: " + ex.Message });
            }
        }
        public async Task<JsonResult> UserUnBlock(long SID)
        {
            try
            {
                bool ans = await _userService.UnBlockAsync(SID);
                if (ans)
                    return Json(new { success = true, message = "کاربر با موفقیت رفع مسدود شد" });
                else
                    return Json(new { success = false, message = "رفع مسدود کردن کاربر با مشکل مواجه شد" });
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "UserUnBlock");
                return Json(new { success = false, message = "عملیات با مشکل مواجه شد: " + ex.Message });
            }
        }

        public async Task<JsonResult> UserDelete(long SID)
        {
            try
            {
                bool ans = await _userService.DeleteAsync(SID);
                if (ans)
                    return Json(new { success = true, message = "کاربر با موفقیت حذف شد" });
                else
                    return Json(new { success = false, message = "حذف کردن کاربر با مشکل مواجه شد" });
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "UserDelete");
                return Json(new { success = false, message = "عملیات با مشکل مواجه شد: " + ex.Message });
            }
        }

        #endregion

        #region ProductManager
        public async Task<IActionResult> ProductManager()
        {
            try
            {
                QueryBuilder qb = _productService.GetWithRelatedEntities();
                qb.AddEqualCondition(Dictionary.Product.Deleted.FullDBName, 0);
                //qb.AddValidationCondition(Dictionary.Product.TableName);
                string Query = qb.CreateQuery();
                DataTable DT = await DBConnection.GetDataTableAsync(Query);

                List<ProductModel> ProductList = _productService.MapDTToModel(DT);
                return View(ProductList);
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "ProductManager");
                ViewBag.ErrorMessage = "عملیات با مشکل مواجه شد. لطفاً دوباره تلاش کنید : " + ex.Message;
                return RedirectToAction(nameof(Dashboard));
            }
        }

        public async Task<IActionResult> ProductAddEdit(long SID)
        {
            try
            {
                QueryBuilder PCQB = _productCategoryService.GetSimple();
                string PCQuery = PCQB.CreateQuery();
                DataTable PCDT = await DBConnection.GetDataTableAsync(PCQuery);
                List<ProductCategoryModel> PCList = _productCategoryService.MapDTToModel(PCDT);

                ProductModel ProductIcon = new ProductModel();
                bool IsAdd = true;

                if (SID > 0)
                {
                    IsAdd = false;
                    ProductIcon = await _productService.GetModelWithRelatedEntitiesAsync(SID);
                    if (ProductIcon.SID <= 0)
                    {
                        ViewBag.ErrorMessage = "کالا پیدا نشد";
                        return RedirectToAction(nameof(ProductManager));
                    }
                }
                return View(new Tuple<ProductModel, List<ProductCategoryModel>, bool>(ProductIcon, PCList,  IsAdd));
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "ProductAddEdit");
                ViewBag.ErrorMessage = "عملیات با مشکل مواجه شد. لطفاً دوباره تلاش کنید : " + ex.Message;
                return RedirectToAction(nameof(ProductManager));
            }
        }

        public async Task<JsonResult> ProductAdd(ProductModel Product, IFormFile ProductImage)
        {
            try
            {
                Product.PDescription ??= string.Empty;
                ProductImageModel productImageModel = new ProductImageModel();
                // ذخیره عکس (در صورت وجود)
                if (ProductImage != null && ProductImage.Length > 0)
                {
                    productImageModel.IsMain = true;
                    productImageModel.ImgName = await FileManager.SaveProductImageAsync(ProductImage);
                }

                Product.ProductImages.Add(productImageModel);
                bool result = await _productService.AddWithImageAsync(Product);
                if (!result)
                {
                    // اگر ویرایش شکست خورد، عکس جدید را پاک کن
                    if (ProductImage != null && !string.IsNullOrEmpty(productImageModel.ImgName))
                        FileManager.DeleteProductImage(productImageModel.ImgName);

                    return Json(new { success = false, message = "ثبت کالا با مشکل مواجه شد!" });
                }

                return Json(new { success = true, message = "کالا با موفقیت ثبت شد." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "خطا: " + ex.Message });
            }
        }

        public async Task<JsonResult> ProductEdit(ProductModel Product, IFormFile ProductImage, string CurrentImagePath)
        {
            try
            {
                Product.PDescription ??= string.Empty;

                ProductImageModel productImageModel = new ProductImageModel();
                productImageModel.PSID = Product.SID;
                productImageModel.IsMain = true;

                bool result = false;
                if (ProductImage != null || CurrentImagePath != null)
                {
                    // مدیریت عکس جدید (حذف عکس قدیم و ذخیره عکس جدید)
                    if (ProductImage != null && ProductImage.Length > 0)
                    {
                        productImageModel.ImgName = await FileManager.SaveProductImageAsync(ProductImage);
                        Product.ProductImages.Add(productImageModel);

                        // حذف عکس قبلی (در صورت وجود)
                        if (!string.IsNullOrEmpty(CurrentImagePath))
                            FileManager.DeleteProductImage(CurrentImagePath);
                    }

                    result = await _productService.EditWithImageAsync(Product);
                }
                else
                {
                    result = await _productService.EditAsync(Product);
                }
                if (!result)
                {
                    // اگر ویرایش شکست خورد، عکس جدید را پاک کن
                    if (ProductImage != null && !string.IsNullOrEmpty(productImageModel.ImgName))
                        FileManager.DeleteProductImage(productImageModel.ImgName);

                    return Json(new { success = false, message = "ویرایش کالا با مشکل مواجه شد!" });
                }

                return Json(new { success = true, message = "کالا با موفقیت ویرایش شد." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "خطا: " + ex.Message });
            }
        }

        public async Task<JsonResult> ProductDelete(long SID)
        {
            try
            {
                bool ans = await _productService.DeleteAsync(SID);
                if (ans)
                    return Json(new { success = true, message = "کالا با موفقیت حذف شد" });
                else
                    return Json(new { success = false, message = "حذف کردن کالا با مشکل مواجه شد" });
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "ProductDelete");
                return Json(new { success = false, message = "عملیات با مشکل مواجه شد: " + ex.Message });
            }
        }
        #endregion

        #region CommentManager
        public IActionResult CommentManager()
        {
            return View();
        }
        #endregion
    }
}
