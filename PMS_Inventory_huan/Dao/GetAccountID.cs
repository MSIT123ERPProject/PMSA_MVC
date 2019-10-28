using PMS_Inventory_huan.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PMS_Inventory_huan.Controllers
{
    public class Utility
    {
        PMSAEntities db = new PMSAEntities();
        //====================================================================
        public string GetSupplierByAccountID(string accountID)
        {
            SupplierAccount supplierAccount = db.SupplierAccount.Find(accountID);
            if (supplierAccount != null)
            {
                string result = supplierAccount.SupplierCode;
                return result;
            }
            return null;
        }
        public SupplierAccount GetSupplierAccountByAccountID(string accountID)
        {
            //先設定一個帳號
            accountID = "SE00001";
            SupplierAccount supplierAccount;
            supplierAccount = db.SupplierAccount.Find(accountID);
            return supplierAccount;
        }
        //======================================================================
    }
}