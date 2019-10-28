using System.ComponentModel.DataAnnotations;

namespace PMS_Inventory_huan.Models
{
    internal class SupplierInfoMetadata
    {
        [Display(Name ="供應商名稱")]
        public string SupplierName { get; set; }
    }
}