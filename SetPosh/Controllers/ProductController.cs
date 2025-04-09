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

        public IActionResult ProductList(string serachText)
        {
            return View(serachText);
        }

        public async Task<IActionResult> _ProductList(string? searchText, string? PCSID, decimal? minPrice, decimal? maxPrice, int? inStock, int? isActive)
        {
            QueryBuilder ProductQB = _productService.GetWithMainImage();

            if (!string.IsNullOrEmpty(searchText))
                ProductQB.AddLikeCondition(Dictionary.Product.PName.FullDBName, searchText);
            if (!string.IsNullOrEmpty(PCSID))
                ProductQB.AddEqualCondition(Dictionary.Product.PCSID.FullDBName, PCSID);
            if (minPrice.HasValue)
                ProductQB.AddGreaterThanCondition(Dictionary.Product.PPrice.FullDBName, minPrice);
            if (maxPrice.HasValue)
                ProductQB.AddLessThanCondition(Dictionary.Product.PPrice.FullDBName, maxPrice);
            //inStock ToDo
            if (isActive >= 0)
                ProductQB.AddEqualCondition(Dictionary.Product.Blocked.FullDBName, isActive);

            string ProductQuery = ProductQB.CreateQuery();
            DataTable ProductDT = await DBConnection.GetDataTableAsync(ProductQuery);
            List<ProductModel> ProductList = _productService.MapDTToModel(ProductDT, true);
            return View(Settings.PartialPathProductList, ProductList);
            //return PartialView("_ProductList", products); // نتایج فیلتر شده
        }
    }
}
