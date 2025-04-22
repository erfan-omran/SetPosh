using Core;
using Core.Model;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Data;
using System.Security.Claims;

namespace SetPosh.Controllers
{
    public class ProductController : Controller
    {
        ProductService _productService;
        ShoppingCartDetailService _shoppingCartDetailSCDService;
        public ProductController(ProductService productService, ShoppingCartDetailService ShoppingCartDetailSCDService)
        {
            _productService = productService;
            _shoppingCartDetailSCDService = ShoppingCartDetailSCDService;
        }

        public async Task<IActionResult> ProductDetails(long PSID)
        {
            ProductModel productModel = await _productService.GetWithDetailsAsync(PSID);
            List<ProductModel> RelatedProducts = await _productService.GetRelatedProducts(productModel.SID, productModel.PCSID);

            long UserProductCount = 0;
            if (User.Identity?.IsAuthenticated ?? false)
            {
                string? USID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(USID))
                    UserProductCount = await _shoppingCartDetailSCDService.GetUserProductsCountAsync(USID.ConvertToLong(), PSID);
            }
            return View(new Tuple<ProductModel, List<ProductModel>, long>(productModel, RelatedProducts, UserProductCount));
        }

        public IActionResult ProductList(string searchText, string? PCSID)
        {
            return View(new Tuple<string, string>(searchText, PCSID));
        }

        public async Task<IActionResult> _ProductList(string? searchText, string? PCSID, decimal? minPrice, decimal? maxPrice, int? inStock, int? isActive)
        {
            List<ProductModel> ProductList = await _productService.GetProductsWithFilter(searchText, PCSID, minPrice, maxPrice, inStock, isActive);

            return View(Settings.PartialPathProductList, ProductList);
        }
    }
}
