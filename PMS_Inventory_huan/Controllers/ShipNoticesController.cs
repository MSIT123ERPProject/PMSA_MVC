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
        private PMSAEntities db = new PMSAEntities();

        // GET: ShipNotices
        [HttpPost]
        public ActionResult Index(PurchaseOrder purchaseOrder)
        {

            if (purchaseOrder != null && purchaseOrder.PurchaseOrderID == "P")
            {
                IQueryable<PurchaseOrder> po = db.PurchaseOrder.Where(n => n.PurchaseOrderID == "P");
                return View(po);
            }
            return View();
        }
        public ActionResult Index(string purchaseOrderID)
        {
            if (purchaseOrderID != null)
            {
                var Order = db.PurchaseOrder.Where(n => n.PurchaseOrderID == purchaseOrderID);
                return View(Order);
            }
            var query = from n in db.PurchaseOrder
                        where n.PurchaseOrderStatus == "P"
                        select new { n.PurchaseOrderID, n.ReceiverName, n.ReceiverTel, n.ReceiverMobile, n.ReceiptAddress };

            var q = db.PurchaseOrder.Where(n => n.PurchaseOrderStatus == "P");
            //ViewBag.PurchaseOrderStatus = new SelectList(,);

            return View(q);
        }

        //[HttpPost]
        //public ActionResult purchaseOrderStatus()
        //{
        //    return View();
        //}

        // GET: ShipNotices/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipNotice shipNotice = db.ShipNotice.Find(id);
            if (shipNotice == null)
            {
                return HttpNotFound();
            }
            return View(shipNotice);
        }

        // GET: ShipNotices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShipNotices/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShipNoticeOID,ShipNoticeID,PurchaseOrderID,ShipDate,EmployeeID,CompanyCode,SupplierAccountID")] ShipNotice shipNotice)
        {
            if (ModelState.IsValid)
            {
                db.ShipNotice.Add(shipNotice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shipNotice);
        }

        // GET: ShipNotices/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrder.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            var query = from n in db.PurchaseOrderDtl where n.PurchaseOrderID == id select n;
            int amount = 0;
            foreach (var x in query)
            {
                amount = amount + (int)x.Total;
            }

            ViewBag.failMessage = Convert.ToString(TempData["failMessage"]);
            ViewBag.PurchaseOrderID = purchaseOrder.PurchaseOrderID;
            ViewBag.ReceiverName = purchaseOrder.ReceiverName;
            ViewBag.ReceiverTel = purchaseOrder.ReceiverTel;
            ViewBag.ReceiverMobile = purchaseOrder.ReceiverMobile;
            ViewBag.ReceiptAddress = purchaseOrder.ReceiptAddress;
            ViewBag.amount = amount;
            return View(purchaseOrder);
        }

        // POST: ShipNotices/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShipNoticeOID,ShipNoticeID,PurchaseOrderID,ShipDate,EmployeeID,CompanyCode,SupplierAccountID")] ShipNotice shipNotice)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrder.Find(shipNotice.PurchaseOrderID);
            TempData.Add("purchaseOrderID", shipNotice.PurchaseOrderID);
            ViewBag.PurchaseOrderID = purchaseOrder.PurchaseOrderID;
            ViewBag.ReceiverName = purchaseOrder.ReceiverName;
            ViewBag.ReceiverTel = purchaseOrder.ReceiverTel;
            ViewBag.ReceiverMobile = purchaseOrder.ReceiverMobile;
            ViewBag.ReceiptAddress = purchaseOrder.ReceiptAddress;
            //if (ModelState.IsValid)
            //{
            //    db.Entry(shipNotice).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            return View(shipNotice.PurchaseOrderID);
        }

        //出貨確認Controller，要修改採購單狀態、以及貨源清單庫存數量
        [HttpPost]
        public ActionResult shipCheck(string purchaseOrderID)
        {
            if (purchaseOrderID != null)
            {
                //修改貨源清單庫存數量
                //var q = from po in db.PurchaseOrder
                //        join pod in db.PurchaseOrderDtl on po.PurchaseOrderID equals pod.PurchaseOrderID
                //        join sl in db.SourceList on pod.SourceListID equals sl.SourceListID
                //        where po.PurchaseOrderID == purchaseOrderID
                //        select new { sl.UnitsInStock };
                //var qty = from po in db.PurchaseOrder
                //        join pod in db.PurchaseOrderDtl on po.PurchaseOrderID equals pod.PurchaseOrderID
                //        where po.PurchaseOrderID == purchaseOrderID
                //        select new {pod.PurchaseOrderDtlCode, pod.Qty };
                var n = from pod in db.PurchaseOrderDtl where pod.PurchaseOrderID == purchaseOrderID select pod;
                var s = from sl in db.SourceList select sl;
                List<string> sourceListTemp = new List<string>();
                List<string> PurchaseOrderDtlCodeTemp = new List<string>();
                foreach (var i in n)
                {
                    foreach (var j in s)
                    {
                        if (i.SourceListID == j.SourceListID)
                        {
                            sourceListTemp.Add(j.SourceListID);
                            PurchaseOrderDtlCodeTemp.Add(i.PurchaseOrderDtlCode);
                        }
                    }
                }
                for (int i = 0; i < sourceListTemp.Count(); i++)
                {
                    SourceList sourceList = db.SourceList.Find(sourceListTemp[i]);
                    PurchaseOrderDtl purchaseOrderDtl = db.PurchaseOrderDtl.Find(PurchaseOrderDtlCodeTemp[i]);
                    int temp = sourceList.UnitsInStock - purchaseOrderDtl.TotalPartQty;
                    if (temp >= 0)
                    {
                        sourceList.UnitsInStock = temp;


                    }
                    else
                    {
                        TempData.Add("failMessage", $"<script>Swal.fire('{PMS_Inventory_huan.Resources.AppResource.NoStock}')</script>");
                        PurchaseOrder po = db.PurchaseOrder.Find(purchaseOrderID);
                        var query = from nn in db.PurchaseOrderDtl where nn.PurchaseOrderID == purchaseOrderID select nn;
                        int amount = 0;
                        foreach (var x in query)
                        {
                            amount = amount + (int)x.Total;
                        }
                        ViewBag.amount = amount;
                        return RedirectToAction("Edit", "ShipNotices", new { id = purchaseOrderID });
                    }
                    db.Entry(sourceList).State = EntityState.Modified;
                    //db.Entry(purchaseOrderDtl).State = EntityState.Modified;
                    // db.SaveChanges();
                }
                //修改採購單狀態
                PurchaseOrder purchaseOrder = db.PurchaseOrder.Find(purchaseOrderID);
                purchaseOrder.PurchaseOrderStatus = "S";
                db.Entry(purchaseOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "ShipNotices", new { controller = "Index", action = "ShipNotices", id = purchaseOrder.PurchaseOrderID });
            }


            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: ShipNotices/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipNotice shipNotice = db.ShipNotice.Find(id);
            if (shipNotice == null)
            {
                return HttpNotFound();
            }
            return View(shipNotice);
        }

        // POST: ShipNotices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ShipNotice shipNotice = db.ShipNotice.Find(id);
            db.ShipNotice.Remove(shipNotice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
