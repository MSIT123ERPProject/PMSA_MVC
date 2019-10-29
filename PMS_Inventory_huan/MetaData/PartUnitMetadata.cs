using System.ComponentModel.DataAnnotations;

namespace PMS_Inventory_huan.Models
{
    public class PartUnitMetadata
    {
        [Display(Name = "料件單位")]
        public string PartUnitName { get; set; }
    }
}