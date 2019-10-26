using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMS_Inventory_huan.Models
{
    [MetadataType(typeof(ShipNoticeMetaData))]
    public partial class ShipNotice
    {
    }
    [MetadataType(typeof(PurchaseOrderMetaData))]
    public partial class PurchaseOrder
    {
    }
    [MetadataType(typeof(SourceListMetaData))]
    public partial class SourceList
    {
    }
}