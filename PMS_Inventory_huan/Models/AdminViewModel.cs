using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PMS_Inventory_huan.Models
{
    public class RoleViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "角色名稱")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "信箱")]
        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}