using DataBase;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using SetPosh.Models;
using Core.Model;
using Service.Service;

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

        public async Task<IActionResult> Index()
        {
            QueryBuilder qb = _UserService.GetWithRelatedEntities();
            string query = qb.CreateQuery();
            DataTable dt = await DBConnection.GetDataTableAsync(query);
            List<UserModel> userList = _UserService.MapDTToModel(dt);
            return View(userList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}