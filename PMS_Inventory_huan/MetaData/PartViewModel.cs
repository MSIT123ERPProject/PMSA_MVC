using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS_Inventory_huan.MetaData
{
    public partial class PartViewModel
    {
        public int PartOID { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public string PartSpec { get; set; }
        public Nullable<int> PartUnitOID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string PictureAdress { get; set; }
        public string PictureDescription { get; set; }
        public string PartCategory { get; set; }
    }
}