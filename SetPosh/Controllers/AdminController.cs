using Core;
using Core.Enum;
using Core.Model;
using DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Data;
using System.Security.Cryptography;

namespace SetPosh.Controllers
{
    [Authorize(nameof(UserTypeEnum.Admin))]
    public class AdminController : Controller
    {
        private readonly UserService _userService;
        public AdminController(UserService userService)
        {
            _userService = userService;
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
                return RedirectToAction(nameof(UserManager));
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
        public async Task<IActionResult> UserAdd(UserModel User)
        {
            try
            {
                User.UFirstName = User.UFirstName ?? string.Empty;
                User.ULastName = User.ULastName ?? string.Empty;
                User.UEmail = User.UEmail ?? string.Empty;
                bool Ans = await _userService.AddAsync(User);
                if (!Ans)
                {
                    ViewBag.ErrorMessage = "ثبت با مشکل مواجه شد";
                    return RedirectToAction(nameof(UserAddEdit), new { SID = User.SID });
                }

                return RedirectToAction(nameof(UserAddEdit));
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "UserAdd");
                ViewBag.ErrorMessage = "عملیات با مشکل مواجه شد. لطفاً دوباره تلاش کنید : " + ex.Message;
                return RedirectToAction(nameof(UserAddEdit));
            }
        }
        public async Task<IActionResult> UserEdit(UserModel User)
        {
            try
            {
                User.UFirstName = User.UFirstName ?? string.Empty;
                User.ULastName = User.ULastName ?? string.Empty;
                User.UEmail = User.UEmail ?? string.Empty;
                bool Ans = await _userService.EditAsync(User);
                if (!Ans)
                {
                    ViewBag.ErrorMessage = "ویرایش با مشکل مواجه شد";
                    return RedirectToAction(nameof(UserAddEdit), new { SID = User.SID });
                }

                return RedirectToAction(nameof(UserAddEdit));
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "UserEdit");
                ViewBag.ErrorMessage = "عملیات با مشکل مواجه شد. لطفاً دوباره تلاش کنید : " + ex.Message;
                return RedirectToAction(nameof(UserAddEdit), new { SID = User.SID });
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
        public IActionResult ProductManager()
        {
            return View();
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
