using Core;
using Core.Model;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Data;
using System.Security.Claims;

namespace SetPosh.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentService _commentService;
        public CommentController(CommentService CommentService)
        {
            _commentService = CommentService;
        }

        [HttpPost]
        public async Task<ActionResult> AddComment(long PSID, short Rate, string CommentText)
        {
            try
            {
                if (!(User.Identity?.IsAuthenticated ?? false))
                    return Json(new { success = false, message = "-1" });

                string? USID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string? UserName = User.FindFirst(ClaimTypes.Name)?.Value;
                if (!string.IsNullOrEmpty(USID))
                {
                    if (string.IsNullOrEmpty(CommentText))
                        return Json(new { Success = false, Message = "لطفا متن نظر را پر کنید" });
                    if (Rate < 1 || Rate > 5)
                        return Json(new { Success = false, Message = "لطفا امتیاز را وارد کنید" });

                    CommentModel Comment = new CommentModel()
                    {
                        USID = USID.ConvertToLong(),
                        PSID = PSID,
                        CRate = Rate,
                        CDescription = CommentText
                    };
                    await _commentService.AddAsync(Comment);
                    return Json(new { Success = true, UserName = (UserName ?? "Not Found") });
                }
                return Json(new { Success = false, Message = "کاربر یافت نشد" });
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex.Message, "Comment.AddComment");
                return Json(new { Message = ex.Message });
            }
        }

    }
}
