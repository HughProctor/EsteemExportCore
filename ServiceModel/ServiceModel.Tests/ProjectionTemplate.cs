using Newtonsoft.Json;
using System;

namespace ServiceModel.Test
{
    public class ProjectionTemplate
    {
        public int TemplateId { get; set; }
        public Guid CreatedById { get; set; }
    }

    public class Projection
    {
        public int ObjectId { get; set; }
        public string DisplayName { get; set; }
        public string AssetName { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string HWAssetStatus { get; set; }
        [JsonIgnore]
        public HWAssetStatus HWAssetStatus_Enum { get; set; }
        public string ObjectStatus { get; set; }

    }

    public enum HWAssetStatus
    {
        [StringValue("Purchase Order")]
        PurchaseOrder = 1,
        [StringValue("New Item")]
        NewItem = 2,
        [StringValue("Commissioned")]
        Commissioned = 3,
        [StringValue("Stocked")]
        Stocked = 4,
        [StringValue("Deployed")]
        Deployed = 5,
        [StringValue("Returned")]
        Returned = 6,
        [StringValue("Retired")]
        Retired = 7,
        [StringValue("Ammended")]
        Ammended = 8
    }
}
