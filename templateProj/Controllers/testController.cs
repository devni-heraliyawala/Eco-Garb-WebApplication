using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace templateProj.Controllers
{
    public class testController : Controller
    {
        // GET: test
        public ActionResult test()
        {
            return View();
        }

        public ActionResult testSetSourceVertex()
        {

            return Json(new { addressList = true }, JsonRequestBehavior.AllowGet);
        }

    }
}