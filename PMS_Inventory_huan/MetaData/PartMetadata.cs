using System.ComponentModel.DataAnnotations;

namespace PMS_Inventory_huan.Models
{
    internal class PartMetadata
    {
        [Display(Name ="料件名稱")]
        public string PartName { get; set; }
    }
}