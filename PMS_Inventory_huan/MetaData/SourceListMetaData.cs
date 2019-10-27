using PMS_Inventory_huan.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace PMS_Inventory_huan.Models
{
    internal class SourceListMetaData
    {
        [Display(Name = "SourceListID",ResourceType =typeof(AppResource))]
        public string SourceListID { get; set; }
        [Display(Name = "PartNumber", ResourceType = typeof(AppResource))]
        public string PartNumber { get; set; }
        [Display(Name = "QtyPerUnit", ResourceType = typeof(AppResource))]
        public int QtyPerUnit { get; set; }
        public Nullable<int> MOQ { get; set; }
        [Display(Name = "UnitPrice", ResourceType = typeof(AppResource))]
        public int UnitPrice { get; set; }
        [Display(Name = "SupplierCode", ResourceType = typeof(AppResource))]
        public string SupplierCode { get; set; }
        [Display(Name = "UnitsInStock", ResourceType = typeof(AppResource))]
        public int UnitsInStock { get; set; }
        [Display(Name = "UnitsOnOrder", ResourceType = typeof(AppResource))]
        public int UnitsOnOrder { get; set; }
        public Nullable<int> SafetyQty { get; set; }
        public Nullable<int> EXP { get; set; }
        public int SourceListOID { get; set; }

    }
}