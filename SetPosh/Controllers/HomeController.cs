using Microsoft.AspNetCore.Mvc;
using DataBase;
using System.Data;
using Core.Model;
using Service;

namespace SetPosh.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductCategoryService _productCategoryService;
        private readonly ProductService _productService;
        public HomeController(ProductService ProductService, ProductCategoryService ProductCategoryService)
        {
            _productService = ProductService;
            _productCategoryService = ProductCategoryService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                QueryBuilder PCQB = _productCategoryService.GetSimple();
                string PCQuery = PCQB.CreateQuery();
                DataTable PCDT = await DBConnection.GetDataTableAsync(PCQuery);
                List<ProductCategoryModel> PCList = _productCategoryService.MapDTToModel(PCDT);
                //-------------------
                QueryBuilder ProductQB = _productService.GetWithMainImage();
                string ProductQuery = ProductQB.CreateQuery();
                DataTable ProductDT = await DBConnection.GetDataTableAsync(ProductQuery);
                List<ProductModel> ProductList = _productService.MapDTToModel(ProductDT, true);
                //-------------------
                return View(new Tuple<List<ProductModel>, List<ProductCategoryModel>>(ProductList, PCList));
            }
            catch (Exception ex)
            {
                DBConnection.LogException(ex, "");
                throw;
            }
        }

        public IActionResult Error(string message)
        {
            return View(message);
        }

        //[Authorize("Admin")]
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