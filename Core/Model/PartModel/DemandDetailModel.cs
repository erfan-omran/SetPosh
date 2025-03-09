using Core.Model.EntityModel;
using System.Data;

namespace Core.Model.PartModel
{
    public class DemandDetailModel : BasePartModel
    {
        public DemandModel Demand { get; set; } = new DemandModel();
        public ProductModel Product { get; set; } = new ProductModel();
        public long DDCount { get; set; } = default;
        public DemandDetailModel() { }
        public DemandDetailModel(DataRow dr)
        {
            Demand = new DemandModel(dr);
            Product = new ProductModel(dr);

            DDCount = dr[nameof(DDCount)].ConvertToLong();
            base.InitBasePartModel(dr);
        }
    }
}
