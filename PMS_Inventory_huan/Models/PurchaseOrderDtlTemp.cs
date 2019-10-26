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
    
    public partial class PurchaseOrderDtlTemp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PurchaseOrderDtlTemp()
        {
            this.PRPORelationTemp = new HashSet<PRPORelationTemp>();
        }
    
        public int PurchaseOrderDtlOID { get; set; }
        public int PurchaseOrderOID { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public string PartSpec { get; set; }
        public int QtyPerUnit { get; set; }
        public int TotalPartQty { get; set; }
        public int OriginalUnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int PurchaseUnitPrice { get; set; }
        public int Qty { get; set; }
        public int PurchasedQty { get; set; }
        public int GoodsInTransitQty { get; set; }
        public Nullable<int> Total { get; set; }
        public Nullable<System.DateTime> DateRequired { get; set; }
        public Nullable<System.DateTime> CommittedArrivalDate { get; set; }
        public Nullable<System.DateTime> ShipDate { get; set; }
        public Nullable<System.DateTime> ArrivedDate { get; set; }
        public Nullable<int> POChangedOID { get; set; }
        public string SourceListID { get; set; }
    
        public virtual Part Part { get; set; }
        public virtual POChanged POChanged { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRPORelationTemp> PRPORelationTemp { get; set; }
        public virtual PurchaseOrderTemp PurchaseOrderTemp { get; set; }
        public virtual SourceList SourceList { get; set; }
    }
}
