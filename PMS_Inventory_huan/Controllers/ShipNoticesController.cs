using PMS_Inventory_huan.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PMS_Inventory_huan.Controllers
{
    public class ShipNoticesController : BaseController
    {
        private PMSAEntities db;
        string supplierAccount;
        string supplierCode;
        public ShipNoticesController()
        {
            db = new PMSAEntities();
            supplierCode = "S00001";
            supplierAccount = "SE00001";
        }

        //GET: ShipNotices
        //此方法為幫助DATATABLE查訂單資料
        public JsonResult GetPurchaseOrderList(string PurchaseOrderStatus)
        {
            string status = PurchaseOrderStatus;

            var query = from po in db.PurchaseOrder.AsEnumerable()
                        where (po.PurchaseOrderStatus == status && po.SupplierCode == supplierCode)
                        select new PurchaseOrder
                        {
                            PurchaseOrderStatus = po.PurchaseOrderStatus,
                            PurchaseOrderID = po.PurchaseOrderID,
                            ReceiverName = po.ReceiverName,
                            ReceiverTel = po.ReceiverTel,
                            ReceiverMobile = po.ReceiverMobile,
                            ReceiptAddress = po.ReceiptAddress
                        };
            return Json(new { data = query }, JsonRequestBehavior.AllowGet);
        }

        //目前沒功能
        public ActionResult IndexUnshipped(PurchaseOrder purchaseOrder)
        {

            if (purchaseOrder != null && purchaseOrder.PurchaseOrderID == "P")
            {
                IQueryable<PurchaseOrder> po = db.PurchaseOrder.Where(n => n.PurchaseOrderID == "P");

                return View(po);
            }
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }


        // GET: ShipNotices/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder shipNotice = db.PurchaseOrder.Find(id);
            if (shipNotice == null)
            {
                return HttpNotFound();
            }
            var query = from n in db.PurchaseOrderDtl where n.PurchaseOrderID == id select n;
            int amount = 0;
            foreach (var x in query)
            {
                amount = amount + (int)x.Total;
            }
            ViewBag.amount = amount;
            return View(shipNotice);
        }


        //按下檢視後進入此方法

        public ActionResult Edit([Bind(Include = "id")] string id)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrder.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound("purchaseOrder is null");
            }
            if (purchaseOrder.PurchaseOrderStatus == "E")
            {
                return RedirectToAction("shipCheck", "ShipNotices", new { id });
            }
            else if (purchaseOrder.PurchaseOrderStatus == "S")
            {
                return RedirectToAction("Details", "ShipNotices", new { id });
            }
            return HttpNotFound("Not Found");
            //===================================================================================
            //PurchaseOrderViewModel purchaseOrderViewModel = new PurchaseOrderViewModel();

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            ////不知道為何找不到該ID的採購單，在INDEX的方法中明明有找出來並顯示在VIEW裡面，而同一筆卻在這找不到
            //PurchaseOrder purchaseOrder = db.PurchaseOrder.Where(x => x.PurchaseOrderID == id).SingleOrDefault();
            //if (purchaseOrder == null)
            //{
            //    return HttpNotFound("purchaseOrder Not Found");
            //}
            //var query = from n in db.PurchaseOrderDtl where n.PurchaseOrderID == id select n;
            //int amount = 0;
            //foreach (var x in query)
            //{
            //    amount = amount + (int)x.Total;
            //}
            //purchaseOrderViewModel.failMessage = Convert.ToString(TempData["failMessage"]);
            //purchaseOrderViewModel.PurchaseOrderID = purchaseOrder.PurchaseOrderID;
            //purchaseOrderViewModel.ReceiverName = purchaseOrder.ReceiverName;
            //purchaseOrderViewModel.ReceiverTel = purchaseOrder.ReceiverTel;
            //purchaseOrderViewModel.ReceiverMobile = purchaseOrder.ReceiverMobile;
            //purchaseOrderViewModel.ReceiptAddress = purchaseOrder.ReceiptAddress;
            //ViewBag.amount = amount;
            //return View(purchaseOrderViewModel);
        }
        /// <summary>
        /// 此功能為亞辰負責
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //判斷如果是未答交的訂單的方法
        public ActionResult purchaseOrderSended(string id)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrder.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound("purchaseOrder Not Found or id is null");
            }
            PurchaseOrder purchaseOrderViewModel = new PurchaseOrder();
            //purchaseOrderViewModel.failMessage = Convert.ToString(TempData["failMessage"]);
            purchaseOrderViewModel.PurchaseOrderID = purchaseOrder.PurchaseOrderID;
            purchaseOrderViewModel.ReceiverName = purchaseOrder.ReceiverName;
            purchaseOrderViewModel.ReceiverTel = purchaseOrder.ReceiverTel;
            purchaseOrderViewModel.ReceiverMobile = purchaseOrder.ReceiverMobile;
            purchaseOrderViewModel.ReceiptAddress = purchaseOrder.ReceiptAddress;
            return View(purchaseOrderViewModel);
        }
        //======================================================================================

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipNotice"></param>
        /// <returns></returns>



        //出貨確認Controller，要修改採購單狀態、以及貨源清單庫存數量
        public ActionResult shipCheck(string id)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrder.Find(id);
            return View(purchaseOrder);
        }

        [HttpGet]
        public ActionResult shipChecked(string id)
        {
            string purchaseOrderID = id;
            if (purchaseOrderID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //修改貨源清單庫存數量              
            var podquery = from pod in db.PurchaseOrderDtl
                           join sl in db.SourceList on pod.SourceListID equals sl.SourceListID
                           where pod.PurchaseOrderID == purchaseOrderID
                           select new { pod.TotalPartQty, sl.UnitsInStock, pod.SourceListID, pod.PurchaseOrderID, pod.PurchaseOrderDtlCode };
            foreach (var x in podquery)
            {
                if (x.UnitsInStock >= x.TotalPartQty)
                {
                    SourceList sourceList = db.SourceList.Find(x.SourceListID);
                    PurchaseOrderDtl purchaseOrderDtl = db.PurchaseOrderDtl.Find(x.PurchaseOrderDtlCode);
                    sourceList.UnitsInStock = sourceList.UnitsInStock - purchaseOrderDtl.TotalPartQty;
                    db.Entry(sourceList).State = EntityState.Modified;
                }
                else
                {
                    PurchaseOrder po = db.PurchaseOrder.Find(purchaseOrderID);
                    var query = from nn in db.PurchaseOrderDtl where nn.PurchaseOrderID == purchaseOrderID select nn;
                    int amount = 0;
                    foreach (var y in query)
                    {
                        amount = amount + (int)y.Total;
                    }
                    ViewBag.amount = amount;
                    return Json("<script>Swal.fire('庫存不足')</script>", JsonRequestBehavior.AllowGet);
                    //庫存不足時，顯示視窗警告並且導回原頁面
                    return RedirectToAction("shipCheck", "ShipNotices", new { id = purchaseOrderID });
                }
            }

            //修改採購單狀態
            PurchaseOrder purchaseOrder = db.PurchaseOrder.Find(purchaseOrderID);
            purchaseOrder.PurchaseOrderStatus = "S";
            db.Entry(purchaseOrder).State = EntityState.Modified;
            //新增出貨通知

            DateTime now = DateTime.Now;
            ShipNotice shipNotice = new ShipNotice();
            string snId = $"SN-{now:yyyyMMdd}-";
            int count = db.ShipNotice.Where(x => x.ShipNoticeID.StartsWith(snId)).Count();
            count++;
            snId = $"{snId}{count:000}";
            shipNotice.ShipNoticeID = snId;
            shipNotice.PurchaseOrderID = purchaseOrderID;
            shipNotice.ShipDate = now;
            shipNotice.EmployeeID = purchaseOrder.EmployeeID;
            var compCode = db.Employee.Where(x => x.EmployeeID == purchaseOrder.EmployeeID).First();
            shipNotice.CompanyCode = compCode.CompanyCode;
            shipNotice.SupplierAccountID = supplierAccount;
            //檢查出貨通知表裡面是否有該訂單ID，如果有，顯示該筆訂單已出貨
            //這裡找步道方法可以檢查成功
            ShipNotice ship = db.ShipNotice.Where(x => x.PurchaseOrderID == purchaseOrderID).FirstOrDefault();
            if (ship != null)
            {
                return Json("<script>Swal.fire('此筆訂單已出貨')</script>", JsonRequestBehavior.AllowGet);
            }
            //存進資料庫 
            purchaseOrder.ShipNotice.Add(shipNotice);
            db.SaveChanges();
            return Json("<script>Swal.fire('出貨成功')</script>", JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index", "ShipNotices", new { id = purchaseOrder.PurchaseOrderID });
        }

        //public ActionResult shipChecked([Bind(Include = "purchaseOrderID")] PurchaseOrder purchaseOrder) {
        //    if (purchaseOrder.PurchaseOrderID == null) {
        //        return HttpNotFound("purchaseOrder.PurchaseOrderID Not Found");
        //    }

        //  return  RedirectToAction("shipChecked",new {id= purchaseOrder.PurchaseOrderID });
        //}
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
