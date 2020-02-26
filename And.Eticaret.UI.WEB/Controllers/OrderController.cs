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
    public class OrderController : AndControllerBase
    {
        // GET: Order
        [Route("SatınAl")]
        public ActionResult AddressList()
        {
            var db = new AndDB();
            var data = db.Adresses.Where(x => x.UserID == LoginUserID).ToList();
            
            return View(data);
        }

        public ActionResult CreateUserAddress()
        {

            return View();
        }

        [HttpPost]
        public ActionResult CreateUserAddress(UserAdress entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.CreateUserID = LoginUserID;
            entity.IsActive = true;
            entity.UserID = LoginUserID;

            var db = new AndDB();
            db.Adresses.Add(entity);
            db.SaveChanges();
            return RedirectToAction("AddressList");
        }

        public ActionResult CreateOrder(int id)
        {
            var db = new AndDB();
            var sepet = db.Baskets.Include("Product").Where(x => x.UserID == LoginUserID).ToList();
            Order order = new Order();
         
            order.CreateDate = DateTime.Now;
            order.CreateUserID = LoginUserID;
            order.StatusID = 1;
            order.TotalProductPrice = sepet.Sum(x => x.Product.Price);
            order.TotalTaxPrice = sepet.Sum(x => x.Product.Tax);
            order.TotalDiscount = sepet.Sum(x => x.Product.Discount);
            order.TotalPrice = order.TotalProductPrice + order.TotalTaxPrice;
            order.UserAdressID = id;
            order.UserID = LoginUserID;

            order.OrderProducts = new List<OrderProduct>();

            foreach (var item in sepet)
            {
                order.OrderProducts.Add(new OrderProduct
                {
                    CreateDate = DateTime.Now,
                    CreateUserID = LoginUserID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity

                });
                //Sepeti Boşalt
                db.Baskets.Remove(item);
            }

            db.Orders.Add(order);
            db.SaveChanges();
            var orderid = db.Orders.Where(x => x.UserID == LoginUserID).ToList().LastOrDefault();
            return RedirectToAction("Detail", new { id = orderid.ID });
        }

        [Route("Odeme-Yontemi")]
        public ActionResult PaymentStep(int id)
        {

            var db = new AndDB();
            var data = db.Adresses.Where(x => x.ID == id).ToList();
            return View(data);
        }

    


        public ActionResult Detail(int id)
        {
            var db = new AndDB();
            var data = db.Orders.Include("OrderProducts").Include("OrderProducts.Product").Include("OrderPayments").Include("Status").Include("UserAdress").Include("User").Where(x => x.ID == id).FirstOrDefault();

            return View(data);
        }

        [Route("Siparislerim")]
        public ActionResult Index()
        {
            var db = new AndDB();
            var data = db.Orders.Include("Status").Where(x => x.UserID == LoginUserID).ToList();
            return View(data);
        }

        public ActionResult Pay(int id)
        {
            var db = new AndDB();
            var order = db.Orders.Where(x => x.ID == id).FirstOrDefault();
            order.StatusID = 7;
            db.SaveChanges();
            return RedirectToAction("Detail",new { id = order.ID });
        } 
    }


}
