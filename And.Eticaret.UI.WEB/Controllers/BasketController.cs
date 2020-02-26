using And.Eticaret.Core.Model;
using And.Eticaret.Core.Model.Entity;
using And.Eticaret.UI.WEB.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace And.Eticaret.UI.WEB.Controllers
{
    public class BasketController : AndControllerBase
    {
        // Post: AddProduct
        [HttpPost]
        public JsonResult AddProduct(int productID, int quantity)
        {
            var db = new AndDB();
            db.Baskets.Add(new Core.Model.Entity.Basket
            {
                CreateDate = DateTime.Now,
                CreateUserID = LoginUserID,
                ProductID = productID,
                Quantity = quantity,
                UserID = LoginUserID,

            }) ;           
            var rt = db.SaveChanges();
            return Json(rt,JsonRequestBehavior.AllowGet);
        }

        [Route("Sepetim")]
        public ActionResult Index()
        {
            var db = new AndDB();
            var data = db.Baskets.Include("Product").Where(x => x.UserID == LoginUserID).ToList();

            //Sepetim Ekranındaki Ara Toplam değerini getirir...
            int tax = ((int)data.Sum(x => x.Product.Price) * (int)data.Sum(x => x.Product.Tax)) / 100;
            ViewBag.subtax = tax.ToString("##,##  ₺");


            int subtotal = ((int)data.Sum(x => x.Product.Price) * (100 - (int)data.Sum(x => x.Product.Tax))) / 100;
            ViewBag.subtotal = subtotal.ToString("##,##  ₺");

            ViewBag.discount = data.Sum(x=>x.Product.Discount).ToString("##,##  ₺");

            ViewBag.total = data.Sum(x => x.Product.Price).ToString("##,##  ₺");
            return View(data);
        }


        public ActionResult Delete(int id)
        {
            var db = new AndDB();

            var deleteitem = db.Baskets.Where(x => x.ID == id).FirstOrDefault();
            db.Baskets.Remove(deleteitem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

     
 


    }
}