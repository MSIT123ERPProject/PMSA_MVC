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
    public class SupplierStocksController : BaseController
    {
        private PMSAEntities db;
        private string SupplierCode;
        private string SupplierAccount;
        public SupplierStocksController()
        {
            db = new PMSAEntities();
            SupplierCode = "S00001";
            SupplierAccount = "SE00001";
        }
        public ActionResult Index()
        {
            SupplierInfo supplierInfo = db.SupplierInfo.Find(SupplierCode);
            ViewBag.supplierName = supplierInfo.SupplierName;
            ViewBag.supplierCode = SupplierCode;
            return View();
        }
        // GET: SupplierStocks
        [HttpPost]
        public ActionResult Index([Bind(Include = "PartNumber")] SourceList SourceList)
        {
            var qeury = from sl in db.SourceList.AsEnumerable()
                        where sl.PartNumber == SourceList.PartNumber
                        select sl;
            ViewBag.supplierCode = SupplierCode;
            return View(qeury);
        }
        [HttpGet]
        public JsonResult GetSourcelistBySupplierCode(string supplierCode)
        {
            //注意  :   dataTable只接受Enumerable類別 ，所以要加上AsEnumerable()方法
            var query = from sl in db.SourceList.AsEnumerable()
                        where sl.SupplierCode == supplierCode
                        select new SourceList
                        {
                            SourceListID = sl.SourceListID,
                            PartNumber = sl.PartNumber,
                            QtyPerUnit = sl.QtyPerUnit,
                            UnitPrice = sl.UnitPrice,
                            UnitsOnOrder = sl.UnitsOnOrder,
                            UnitsInStock = sl.UnitsInStock
                        };
            return Json(new { data = query }, JsonRequestBehavior.AllowGet);
        }

        //供應商庫存管理首頁，將使用者輸入的件號從資料庫找出來，等資料庫出來再來實驗
        //[HttpPost]
        //public ActionResult SupplierStocksView(int partNumber)
        //{
        // ===========  實驗dataTableg 失敗
        //    var query = db.SourceList.ToList<SourceList>();
        //    return Json(new { data = query, },JsonRequestBehavior.AllowGet);
        //=====================================
        //    //var query = from n in db.SourceList
        //    //            join c in db.Part
        //    //            on n.PartNumber equals c.PartNumber
        //    //            where n.PartNumber == partNumber
        //    //            orderby n.SourceListOID ascending
        //    //            select new { c.Partname, n.Partnumber, n.UnitsInStock };

        //    //return View(query);
        //}

        // GET: SupplierStocks/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SourceList supplierStock = db.SourceList.Find(id);
            if (supplierStock == null)
            {
                return HttpNotFound();
            }
            return View(supplierStock);
        }

        // GET: SupplierStocks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupplierStocks/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupplierStockOID,PartNumber,PartName,PartSpec,InventoryCode,UnitsInStock,UnitsOnStockOutOrder,UnitsOnStockInOrder,Shelf_life,CreateDate,CreateEmployeeID,LastModifiedDate,LastModifiedEmployeeID")] SourceList SourceList)
        {
            if (ModelState.IsValid)
            {
                db.SourceList.Add(SourceList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(SourceList);
        }

        // GET: SupplierStocks/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SourceList SourceList = db.SourceList.Find(id);
            if (SourceList == null)
            {
                return HttpNotFound();
            }
            return View(SourceList);
        }

        // POST: SupplierStocks/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UnitsInStock,PartNumber,SourceListOID,SourceListID")] SourceList SourceList)
        {
            int? UnitsInStock = SourceList.UnitsInStock;
            if (UnitsInStock != null && UnitsInStock > 0)
            {

                SourceList a = db.SourceList.Find(SourceList.SourceListID);
                a.UnitsInStock = (int)UnitsInStock;
                db.Entry(a).State = EntityState.Modified;
                db.SaveChanges();
            }
            //if (ModelState.IsValid)
            //{
            //    db.Entry(SourceList).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            return RedirectToAction("Index", "SupplierStocks");
        }

        // GET: SupplierStocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SourceList supplierStock = db.SourceList.Find(id);
            if (supplierStock == null)
            {
                return HttpNotFound();
            }
            return View(supplierStock);
        }

        // POST: SupplierStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SourceList supplierStock = db.SourceList.Find(id);
            db.SourceList.Remove(supplierStock);
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
