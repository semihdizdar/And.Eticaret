using And.Eticaret.Core.Model;
using And.Eticaret.Core.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using And.Eticaret.UI.WEB.Controllers.Base;

namespace And.Eticaret.UI.WEB.Controllers
{
    
    public class HomeController : AndControllerBase
    {
        AndDB db = new AndDB();
        public ActionResult Index(int? page)
        {
            ViewBag.IsLogin = this.IsLogin;
            var data = db.Products.OrderByDescending(x => x.CreateDate).ToList().ToPagedList(page ?? 1,8);
            ViewBag.totalProducts = db.Products.Count();
            return View(data);
        }


        public PartialViewResult GetMenu()
        {

            var menus = db.Categories.Where(x => x.ParentID == 0).ToList();
            return PartialView(menus);
        }



        [Route("Uye-Kayit")]
        [HttpGet]
        public ActionResult CreateUser()
        {

            return View();
        }


        [Route("Uye-Kayit")]
        [HttpPost]
        public ActionResult CreateUser(User entity)
        {

            try
            {
                entity.CreateDate = DateTime.Now;
                entity.CreateUserID = 1;
                entity.IsActive = true;
                entity.IsAdmin = false;
                db.Users.Add(entity);
                db.SaveChanges();
                return Redirect("/");
            }
            catch
            {

                return View();
            }
        }


        [Route("Uye-Giris")]
        public ActionResult Login()
        {

            return View();

        }


        [Route("Uye-Giris")]
        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {

            var users = db.Users.Where(x => x.Email == Email && x.Password == Password && x.IsActive == true && x.IsAdmin == false).ToList();

            if (users.Count == 1)
            {

                // Giris Yapıldı...
                Session["LoginUserID"] = users.FirstOrDefault().ID;
                Session["LoginUser"] = users.FirstOrDefault();
                return Redirect("/");
            }
            else
            {
                // Giris Yapılamadı....
                ViewBag.ErorLogin = "Hatalı Kullanıcı Adı veya Şifre !!!!";
                return View();
            }


        }


    }
}