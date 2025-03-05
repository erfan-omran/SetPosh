using DataBase;
using Microsoft.AspNetCore.Mvc;
using SetPosh.Models;
using System.Data;
using System.Diagnostics;

namespace SetPosh.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            QueryBuilder QB = new QueryBuilder();
            QB.AddColumn(SqlFunction.Count("*"));
            QB.SetTable("User");
            string q = QB.CreateQuery();

            DBConnection a = new DBConnection();
            DataTable b = a.GetDataTable(q);

            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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