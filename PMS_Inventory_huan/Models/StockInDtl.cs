//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace PMS_Inventory_huan.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StockInDtl
    {
        public int StockInDtlOID { get; set; }
        public string StockInID { get; set; }
        public string InventoryCode { get; set; }
        public string PartNumber { get; set; }
        public int StockInQty { get; set; }
        public Nullable<System.DateTime> EXP { get; set; }
        public string Remark { get; set; }
    
        public virtual InventoryDtl InventoryDtl { get; set; }
        public virtual Part Part { get; set; }
        public virtual StockIn StockIn { get; set; }
    }
}