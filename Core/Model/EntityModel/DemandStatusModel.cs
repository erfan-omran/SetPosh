using System.Data;

namespace Core.Model.EntityModel
{
    public class DimandStatusModel : BaseEntityModel
    {
        public string DSName { get; set; } = string.Empty;
        public string DSDescription { get; set; } = string.Empty;
        public DimandStatusModel() { }
        public DimandStatusModel(DataRow dr)
        {
            DSName = dr[nameof(DSName)].ConvertToString();
            DSDescription = dr[nameof(DSDescription)].ConvertToString();
            base.InitBaseEntityModel(dr);
        }
    }
}
