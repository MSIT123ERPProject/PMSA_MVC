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
    public class SupplierShipmentsController : BaseController
    {
        private PMSEntities1 db = new PMSEntities1();

        // GET: SupplierShipments
        [HttpPost]
        public ActionResult Index(string partNumber)
        {
            //db.SupplierShipment.Where(s=>s.)
            
            return View(partNumber);
        }

        // GET: SupplierShipments/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierShipment supplierShipment = db.SupplierShipment.Find(id);
            if (supplierShipment == null)
            {
                return HttpNotFound();
            }
            return View(supplierShipment);
        }

        // GET: SupplierShipments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupplierShipments/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupplierShipmentOID,ShipCode,InventoryCode,OrderID,CreateDate,CreateEmployeeID,LastModifiedDate,LastModifiedEmployeeID,Remark")] SupplierShipment supplierShipment)
        {
            if (ModelState.IsValid)
            {
                db.SupplierShipment.Add(supplierShipment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplierShipment);
        }

        // GET: SupplierShipments/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierShipment supplierShipment = db.SupplierShipment.Find(id);
            if (supplierShipment == null)
            {
                return HttpNotFound();
            }
            return View(supplierShipment);
        }

        // POST: SupplierShipments/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupplierShipmentOID,ShipCode,InventoryCode,OrderID,CreateDate,CreateEmployeeID,LastModifiedDate,LastModifiedEmployeeID,Remark")] SupplierShipment supplierShipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplierShipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplierShipment);
        }

        // GET: SupplierShipments/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierShipment supplierShipment = db.SupplierShipment.Find(id);
            if (supplierShipment == null)
            {
                return HttpNotFound();
            }
            return View(supplierShipment);
        }

        // POST: SupplierShipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SupplierShipment supplierShipment = db.SupplierShipment.Find(id);
            db.SupplierShipment.Remove(supplierShipment);
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
