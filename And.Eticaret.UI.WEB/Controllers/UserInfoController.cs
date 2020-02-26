using And.Eticaret.Core.Model;
using And.Eticaret.UI.WEB.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace And.Eticaret.UI.WEB.Controllers
{
    public class UserInfoController : AndControllerBase
    {
        // GET: UserInfo

        [Route("UyeBilgi")]
        public ActionResult Index()
        {
            AndDB db = new AndDB();
            var data = db.Users.Where(x => x.ID == LoginUserID);
            return View(data);
        }


        [Route("Adres-Degistir")]
        public ActionResult EditAdrress()
        {

            return View();
        }
    }

}