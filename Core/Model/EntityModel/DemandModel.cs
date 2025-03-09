using System.Data;

namespace Core.Model.EntityModel
{
    public class DemandModel : BaseEntityModel
    {
        public DimandStatusModel DimandStatus { get; set; } = new DimandStatusModel();
        public UserModel User { get; set; } = new UserModel();
        //public string DeliveryDate { get; set; } = string.Empty;
        public bool Confirmed { get; set; } = default;

        public DemandModel() { }
        public DemandModel(DataRow dr)
        {
            DimandStatus = new DimandStatusModel(dr);
            User = new UserModel(dr);

            //DeliveryDate = dr[nameof(Confirmed)].ConvertToDateTime();
            Confirmed = dr[nameof(Confirmed)].ConvertToBool();
            base.InitBaseEntityModel(dr);
        }
    }
}
