using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PMS_Inventory_huan.Models;

namespace PMS_Inventory_huan.Controllers
{
    public class InventoryDtlsController : BaseController
    {
        private PMSAEntities db = new PMSAEntities();

        // GET: InventoryDtls
        public ActionResult Index()
        {
            var inventoryDtl = db.InventoryDtl.Include(i => i.Employee).Include(i => i.Employee1).Include(i => i.InventoryCategory).Include(i => i.Part).Include(i => i.SourceList).Include(i => i.WarehouseInfo);
            return View(inventoryDtl);
        }

        // GET: InventoryDtls/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryDtl inventoryDtl = db.InventoryDtl.Find(id);
            if (inventoryDtl == null)
            {
                return HttpNotFound();
            }
            return View(inventoryDtl);
        }

        // GET: InventoryDtls/Create
        public ActionResult Create()
        {
            var q = from p in db.SourceList
                    select p.SourceListID;
            var q1 = from p in db.InventoryDtl
                     select p.SourceListID;
            var q2 = q.Except(q1);

            ViewBag.CreateEmployeeID = new SelectList(db.Employee, "EmployeeID", "Name");
            ViewBag.LastModifiedEmployeeID = new SelectList(db.Employee, "EmployeeID", "Name");
            ViewBag.InventoryCategoryCode = new SelectList(db.InventoryCategory, "InventoryCategoryCode", "InventoryCategoryName");
            ViewBag.PartNumber = new SelectList(db.Part, "PartNumber", "PartName");
            ViewBag.SourceListID = new SelectList(q2);
            ViewBag.WarehouseCode = new SelectList(db.WarehouseInfo, "WarehouseCode", "WarehouseName");
            return View();
        }

        //嘗試 在新增頁面按下 新增 時，先執行一方法(在此處SQL語法將要自動產生的值在後端帶入)
        //然後再執行原本的CREATE方法

        //嘗試將貨源清單編號的下拉選單跟料件名稱的下拉選單做成連動效果

        // POST: InventoryDtls/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InventoryDtlOID,InventoryCode,WarehouseCode,InventoryCategoryCode,SourceListID,PartNumber,UnitsInStock,UnitsOnStockOutOrder,UnitsOnStockInOrder,SafetyQty,CreateDate,CreateEmployeeID,LastModifiedDate,LastModifiedEmployeeID")] InventoryDtl inventoryDtl)
        {
            int z = 1;
            string y = inventoryDtl.WarehouseCode + "-" + inventoryDtl.InventoryCategoryCode + "-" + inventoryDtl.SourceListID + "-S0000" + z.ToString();

            for (int i = 0; i < db.InventoryDtl.Count(); i++)
            {
                InventoryDtl test = db.InventoryDtl.Find(y);
                if (z < 9)
                {
                    if (test != null)
                    {
                        z += 1;
                        y = inventoryDtl.WarehouseCode + "-" + inventoryDtl.InventoryCategoryCode + "-" + inventoryDtl.SourceListID + "-S0000" + z.ToString();
                        test = db.InventoryDtl.Find(y);
                    }
                }
                else if (z < 99)
                {
                    if (test != null)
                    {
                        z += 1;
                        y = inventoryDtl.WarehouseCode + "-" + inventoryDtl.InventoryCategoryCode + "-" + inventoryDtl.SourceListID + "-S000" + z.ToString();
                        test = db.InventoryDtl.Find(y);
                    }
                }
                else if (z < 999)
                {
                    if (test != null)
                    {
                        z += 1;
                        y = inventoryDtl.WarehouseCode + "-" + inventoryDtl.InventoryCategoryCode + "-" + inventoryDtl.SourceListID + "-S00" + z.ToString();
                        test = db.InventoryDtl.Find(y);
                    }
                }
                else if (z < 9999)
                {
                    if (test != null)
                    {
                        z += 1;
                        y = inventoryDtl.WarehouseCode + "-" + inventoryDtl.InventoryCategoryCode + "-" + inventoryDtl.SourceListID + "-S0" + z.ToString();
                        test = db.InventoryDtl.Find(y);
                    }
                }
                else
                {
                    if (test != null)
                    {
                        z += 1;
                        y = inventoryDtl.WarehouseCode + "-" + inventoryDtl.InventoryCategoryCode + "-" + inventoryDtl.SourceListID + "-S" + z.ToString();
                        test = db.InventoryDtl.Find(y);
                    }
                }

            }

            inventoryDtl.InventoryCode = y;

            var q = from p in db.SourceList
                    where p.SourceListID == inventoryDtl.SourceListID
                    select p.PartNumber;

            var p1 = q.ToList();

            var qe = from p in db.SourceList
                     select p.SourceListID;
            var q1 = from p in db.InventoryDtl
                     select p.SourceListID;
            var q2 = qe.Except(q1);

            if (ModelState.IsValid)
            {
                inventoryDtl.PartNumber = p1[0].ToString();
                inventoryDtl.CreateEmployeeID = "CE00001";
                inventoryDtl.LastModifiedEmployeeID = "CE00001";
                inventoryDtl.CreateDate = DateTime.Now;
                inventoryDtl.LastModifiedDate = DateTime.Now;
                db.InventoryDtl.Add(inventoryDtl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreateEmployeeID = new SelectList(db.Employee, "EmployeeID", "Name", inventoryDtl.CreateEmployeeID);
            ViewBag.LastModifiedEmployeeID = new SelectList(db.Employee, "EmployeeID", "Name", inventoryDtl.LastModifiedEmployeeID);
            ViewBag.InventoryCategoryCode = new SelectList(db.InventoryCategory, "InventoryCategoryCode", "InventoryCategoryName", inventoryDtl.InventoryCategoryCode);
            ViewBag.PartNumber = new SelectList(db.Part, "PartNumber", "PartName", inventoryDtl.PartNumber);
            ViewBag.SourceListID = new SelectList(q2);
            ViewBag.WarehouseCode = new SelectList(db.WarehouseInfo, "WarehouseCode", "WarehouseName", inventoryDtl.WarehouseCode);
            return View(inventoryDtl);
        }

        // GET: InventoryDtls/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryDtl inventoryDtl = db.InventoryDtl.Find(id);
            if (inventoryDtl == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreateEmployeeID = new SelectList(db.Employee, "EmployeeID", "Name", inventoryDtl.CreateEmployeeID);
            ViewBag.LastModifiedEmployeeID = new SelectList(db.Employee, "EmployeeID", "Name", inventoryDtl.LastModifiedEmployeeID);
            ViewBag.InventoryCategoryCode = new SelectList(db.InventoryCategory, "InventoryCategoryCode", "InventoryCategoryName", inventoryDtl.InventoryCategoryCode);
            ViewBag.PartNumber = new SelectList(db.Part, "PartNumber", "PartName", inventoryDtl.PartNumber);
            ViewBag.SourceListID = new SelectList(db.SourceList, "SourceListID", "SourceListID", inventoryDtl.SourceListID);
            ViewBag.WarehouseCode = new SelectList(db.WarehouseInfo, "WarehouseCode", "WarehouseName", inventoryDtl.WarehouseCode);
            return View(inventoryDtl);
        }

        // POST: InventoryDtls/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InventoryDtlOID,InventoryCode,WarehouseCode,InventoryCategoryCode,SourceListID,PartNumber,UnitsInStock,UnitsOnStockOutOrder,UnitsOnStockInOrder,SafetyQty,CreateDate,CreateEmployeeID,LastModifiedDate,LastModifiedEmployeeID")] InventoryDtl inventoryDtl)
        {
            if (ModelState.IsValid)
            {
                inventoryDtl.CreateEmployeeID = "CE00001";
                inventoryDtl.LastModifiedEmployeeID = "CE00001";
                inventoryDtl.LastModifiedDate = DateTime.Now;
                db.Entry(inventoryDtl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateEmployeeID = new SelectList(db.Employee, "EmployeeID", "Name", inventoryDtl.CreateEmployeeID);
            ViewBag.LastModifiedEmployeeID = new SelectList(db.Employee, "EmployeeID", "Name", inventoryDtl.LastModifiedEmployeeID);
            ViewBag.InventoryCategoryCode = new SelectList(db.InventoryCategory, "InventoryCategoryCode", "InventoryCategoryName", inventoryDtl.InventoryCategoryCode);
            ViewBag.PartNumber = new SelectList(db.Part, "PartNumber", "PartName", inventoryDtl.PartNumber);
            ViewBag.SourceListID = new SelectList(db.SourceList, "SourceListID", "SourceListID", inventoryDtl.SourceListID);
            ViewBag.WarehouseCode = new SelectList(db.WarehouseInfo, "WarehouseCode", "WarehouseName", inventoryDtl.WarehouseCode);
            return View(inventoryDtl);
        }

        // GET: InventoryDtls/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    InventoryDtl inventoryDtl = db.InventoryDtl.Find(id);
        //    if (inventoryDtl == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(inventoryDtl);
        //}

        //// POST: InventoryDtls/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    InventoryDtl inventoryDtl = db.InventoryDtl.Find(id);
        //    db.InventoryDtl.Remove(inventoryDtl);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
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
