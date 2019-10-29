using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMS_Inventory_huan.Models
{
    public class PartViewModel
    {
        [Display(Name = "料件識別碼")]
        public int PartOID { get; set; }
        [Display(Name = "料件編號")]
        public string PartNumber { get; set; }
        [Display(Name = "料件名稱")]
        public string PartName { get; set; }
        [Display(Name = "料件規格")]
        public string PartSpec { get; set; }
        [Display(Name = "料件單位識別碼")]
        public Nullable<int> PartUnitOID { get; set; }
        [Display(Name = "料件新增時間")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime CreatedDate { get; set; }
        [Display(Name = "料件圖片位置")]
        public string PictureAdress { get; set; }
        [Display(Name = "料件圖片說明")]
        public string PictureDescription { get; set; }

        public byte[] File1 { get; set; }
    }
}