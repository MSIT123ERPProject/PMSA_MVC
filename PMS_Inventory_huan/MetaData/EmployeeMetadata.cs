using System.ComponentModel.DataAnnotations;

namespace PMS_Inventory_huan.Models
{
    internal class EmployeeMetadata
    {
        [Display(Name = "員工編號")]
        public string EmployeeID { get; set; }
        [Display(Name = "採購員姓名")]
        public string Name { get; set; }
    }
}