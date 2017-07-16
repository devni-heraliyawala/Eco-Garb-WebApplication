using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using templateProj.Models;

namespace templateProj.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        public ActionResult RestrictionError()
        {
            return View();
        }

        public ActionResult Timeout()
        {
            return View();
        }
    }
}