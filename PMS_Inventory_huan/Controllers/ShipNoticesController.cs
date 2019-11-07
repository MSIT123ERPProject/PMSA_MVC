﻿using PMS_Inventory_huan.Models;
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

        Utility utility = new Utility();
        private PMSAEntities db = new PMSAEntities();

        //GET: ShipNotices
        public JsonResult GetPurchaseOrderList(string PurchaseOrderStatus)
        {
            string status = PurchaseOrderStatus;

            var query = from po in db.PurchaseOrder.AsEnumerable()
                        where (po.PurchaseOrderStatus == status && po.SupplierCode == "S00001")
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
        [HttpPost]
        public ActionResult Index(string PurchaseOrderStatus)
        {
            string status;
            if (Request["PurchaseOrderStatus"] != null)
            {
                //檢查使用者是否有使用dropdownlist選擇訂單
                status = Request["PurchaseOrderStatus"].ToString();
            }
            else
            {
                //如果沒輸入則找未答交的訂單
                status = "P";
            }
            var query = from po in db.PurchaseOrder
                        where (po.PurchaseOrderStatus == status && po.SupplierCode == "S00001")
                        select po;
            return View(query);
        }
        public ActionResult Index(PurchaseOrder purchaseOrder)
        {
            string status;
            if (Request["PurchaseOrderStatus"] != null)
            {
                //檢查使用者是否有使用dropdownlist選擇訂單
                status = Request["PurchaseOrderStatus"].ToString();
            }
            else
            {
                //如果沒輸入則找未答交的訂單
                status = "P";
            }
            var query = from po in db.PurchaseOrder
                        where (po.PurchaseOrderStatus == status && po.SupplierCode == "S00001")
                        select po;
            return View(query);


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
        //按下檢視後進入此方法
        [ValidateInput(false)]
        public ActionResult Edit(string id)
        {
            PurchaseOrderViewModel purchaseOrderViewModel = new PurchaseOrderViewModel();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //不知道為何找不到該ID的採購單，在INDEX的方法中明明有找出來並顯示在VIEW裡面，而同一筆卻在這找不到
            PurchaseOrder purchaseOrder = db.PurchaseOrder.Where(x => x.PurchaseOrderID == id).SingleOrDefault();
            if (purchaseOrder == null)
            {
                return HttpNotFound("purchaseOrder Not Found");
            }
            var query = from n in db.PurchaseOrderDtl where n.PurchaseOrderID == id select n;
            int amount = 0;
            foreach (var x in query)
            {
                amount = amount + (int)x.Total;
            }
            purchaseOrderViewModel.failMessage = Convert.ToString(TempData["failMessage"]);
            purchaseOrderViewModel.PurchaseOrderID = purchaseOrder.PurchaseOrderID;
            purchaseOrderViewModel.ReceiverName = purchaseOrder.ReceiverName;
            purchaseOrderViewModel.ReceiverTel = purchaseOrder.ReceiverTel;
            purchaseOrderViewModel.ReceiverMobile = purchaseOrder.ReceiverMobile;
            purchaseOrderViewModel.ReceiptAddress = purchaseOrder.ReceiptAddress;

            //ViewBag.failMessage = Convert.ToString(TempData["failMessage"]);
            //ViewBag.PurchaseOrderID = purchaseOrder.PurchaseOrderID;
            //ViewBag.ReceiverName = purchaseOrder.ReceiverName;
            //ViewBag.ReceiverTel = purchaseOrder.ReceiverTel;
            //ViewBag.ReceiverMobile = purchaseOrder.ReceiverMobile;
            //ViewBag.ReceiptAddress = purchaseOrder.ReceiptAddress;
            ViewBag.amount = amount;
            return View(purchaseOrderViewModel);
        }
        //判斷如果是未答交的訂單的方法
        public ActionResult purchaseOrderSended(string id)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrder.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound("purchaseOrder Not Found or id is null");
            }
            PurchaseOrderViewModel purchaseOrderViewModel = new PurchaseOrderViewModel();
            purchaseOrderViewModel.failMessage = Convert.ToString(TempData["failMessage"]);
            purchaseOrderViewModel.PurchaseOrderID = purchaseOrder.PurchaseOrderID;
            purchaseOrderViewModel.ReceiverName = purchaseOrder.ReceiverName;
            purchaseOrderViewModel.ReceiverTel = purchaseOrder.ReceiverTel;
            purchaseOrderViewModel.ReceiverMobile = purchaseOrder.ReceiverMobile;
            purchaseOrderViewModel.ReceiptAddress = purchaseOrder.ReceiptAddress;
            return View(purchaseOrderViewModel);
        }
        public ActionResult purchaseOrderAccepted(string id)
        {

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
        [HttpPost, ActionName("shipCheck")]
        public ActionResult shipCheck(string purchaseOrderID)
        {
            if (purchaseOrderID != null)
            {
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
                        TempData.Add("failmessage", $"<script>Swal.fire('{PMS_Inventory_huan.Resources.AppResource.NoStock}')</script>");
                        var query = from nn in db.PurchaseOrderDtl where nn.PurchaseOrderID == purchaseOrderID select nn;
                        int amount = 0;
                        foreach (var y in query)
                        {
                            amount = amount + (int)y.Total;
                        }
                        ViewBag.amount = amount;
                        //return View(po);
                        return RedirectToAction("Edit", "ShipNotices", new { id = purchaseOrderID });
                    }
                }
                //===================================
                //舊寫法
                //var n = from pod in db.PurchaseOrderDtl where pod.PurchaseOrderID == purchaseOrderID select pod;
                //var s = from sl in db.SourceList select sl;
                //List<string> sourceListTemp = new List<string>();
                //List<string> PurchaseOrderDtlCodeTemp = new List<string>();
                //foreach (var i in n)
                //{
                //    foreach (var j in s)
                //    {
                //        if (i.SourceListID == j.SourceListID)
                //        {
                //            sourceListTemp.Add(j.SourceListID);
                //            PurchaseOrderDtlCodeTemp.Add(i.PurchaseOrderDtlCode);
                //        }
                //    }
                //}
                //for (int i = 0; i < sourceListTemp.Count(); i++)
                //{
                //    SourceList sourceList = db.SourceList.Find(sourceListTemp[i]);
                //    PurchaseOrderDtl purchaseOrderDtl = db.PurchaseOrderDtl.Find(PurchaseOrderDtlCodeTemp[i]);
                //    int temp = sourceList.UnitsInStock - purchaseOrderDtl.TotalPartQty;
                //    if (temp >= 0)
                //    {
                //        sourceList.UnitsInStock = temp;


                //    }
                //    else
                //    {
                //        TempData.Add("failMessage", $"<script>Swal.fire('{PMS_Inventory_huan.Resources.AppResource.NoStock}')</script>");
                //        PurchaseOrder po = db.PurchaseOrder.Find(purchaseOrderID);
                //        var query = from nn in db.PurchaseOrderDtl where nn.PurchaseOrderID == purchaseOrderID select nn;
                //        int amount = 0;
                //        foreach (var x in query)
                //        {
                //            amount = amount + (int)x.Total;
                //        }
                //        ViewBag.amount = amount;
                //        return RedirectToAction("Edit", "ShipNotices", new { id = purchaseOrderID });
                //    }
                //    db.Entry(sourceList).State = EntityState.Modified;
                //    //db.Entry(purchaseOrderDtl).State = EntityState.Modified;
                //    // db.SaveChanges();
                //}
                //修改採購單狀態
                PurchaseOrder purchaseOrder = db.PurchaseOrder.Find(purchaseOrderID);
                purchaseOrder.PurchaseOrderStatus = "S";
                db.Entry(purchaseOrder).State = EntityState.Modified;
                //存進資料庫
                db.SaveChanges();
                //return View(purchaseOrder);
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
