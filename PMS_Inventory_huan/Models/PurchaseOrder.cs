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
    
    public partial class PurchaseOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PurchaseOrder()
        {
            this.PRPORelation = new HashSet<PRPORelation>();
            this.PurchaseOrderDtl = new HashSet<PurchaseOrderDtl>();
            this.PurchaseOrderReceive = new HashSet<PurchaseOrderReceive>();
            this.ShipNotice = new HashSet<ShipNotice>();
        }
    
        public int PurchaseOrderOID { get; set; }
        public string PurchaseOrderID { get; set; }
        public string SupplierCode { get; set; }
        public string EmployeeID { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverTel { get; set; }
        public string ReceiverMobile { get; set; }
        public string ReceiptAddress { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string PurchaseOrderStatus { get; set; }
        public string SignStatus { get; set; }
        public Nullable<int> SignFlowOID { get; set; }
    
        public virtual Employee Employee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRPORelation> PRPORelation { get; set; }
        public virtual SignFlow SignFlow { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrderDtl> PurchaseOrderDtl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrderReceive> PurchaseOrderReceive { get; set; }
        public virtual SupplierInfo SupplierInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShipNotice> ShipNotice { get; set; }
    }
}
