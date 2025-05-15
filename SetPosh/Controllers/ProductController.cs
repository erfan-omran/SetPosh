using Core;
using Core.Model;
using Service;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

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
            return View(new Tuple<string, string?>(searchText, PCSID));
        }

        public async Task<IActionResult> _ProductList(string? searchText, string? PCSID, decimal? minPrice, decimal? maxPrice, int? inStock, int? isBlocked, int? isDeleted)
        {
            List<ProductModel> ProductList = await _productService.GetProductsWithFilter(searchText, PCSID, minPrice, maxPrice, inStock, isBlocked, isDeleted);

            return View(Settings.PartialPathProductList, ProductList);
        }
    }
}
