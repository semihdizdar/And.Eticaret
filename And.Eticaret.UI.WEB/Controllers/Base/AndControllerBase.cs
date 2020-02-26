using And.Eticaret.Core.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace And.Eticaret.UI.WEB.Controllers.Base
{
    public class AndControllerBase : Controller
    {
        //<summary>
        //  Kullanıcı Login İşlemleri
        //</summary>
        public bool IsLogin { get; private set; }

        //<summary>
        //  Giriş yapmış kişinin loginID'si
        //</summary>
        public int LoginUserID { get; set; }

        //<summary>
        //  Login User Entity
        //</summary>
        public User LoginUserEntity { get; private set; }

        protected override void Initialize(RequestContext requestContext)
        {
            // 
            if (requestContext.HttpContext.Session["LoginUserID"] != null)
            {
                //Kullanıcı Giriş Yapmış
                IsLogin = true;
                LoginUserID = (int)requestContext.HttpContext.Session["LoginUserID"] ;
                LoginUserEntity = (Core.Model.Entity.User)requestContext.HttpContext.Session["LoginUser"];

            }
            base.Initialize(requestContext);
        }
    }
}