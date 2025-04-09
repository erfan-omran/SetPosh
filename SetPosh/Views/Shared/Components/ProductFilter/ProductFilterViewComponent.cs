using Microsoft.AspNetCore.Mvc;
using Core.Model;
using Service;
using DataBase;
using System.Data;

namespace SetPosh.Views.Shared.Components.ProductFilter
{
    public class ProductFilterViewComponent : ViewComponent
    {
        private readonly ProductCategoryService _productCategoryService;

        public ProductFilterViewComponent(ProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string Query = _productCategoryService.GetSimple().CreateQuery();
            DataTable CategorieDT = await DBConnection.GetDataTableAsync(Query);
            List<ProductCategoryModel> Categories = _productCategoryService.MapDTToModel(CategorieDT);

            return View(new Tuple<List<ProductCategoryModel>>(Categories));
        }
    }
}
