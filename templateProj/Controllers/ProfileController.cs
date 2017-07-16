using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using templateProj.Models;
using templateProj.Property;

namespace templateProj.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        DataContext db = new DataContext();
        Paths path = new Paths();

        #region View Profile Function
        public ActionResult Profile()
        {
            UserModel um = db.Umodel.Find(HttpContext.Session["Uname"]);
            return View(um);
        }

        #endregion

        #region Update User profile(database)
        [HttpPost]
        public void SaveUserInfo(string[] infoArr)
        {
            try
            {
                UserModel um = db.Umodel.Find(HttpContext.Session["Uname"]);
                um.FirstName = infoArr[0];
                um.Email = infoArr[1];
                um.Address = infoArr[2];
                um.ContactNo = infoArr[3];
                um.DoB = infoArr[4];
                um.Notes = infoArr[5];

                if (ModelState.IsValid)
                {
                    db.Entry(um).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Other / not yet used
        //Direct user to a profile of another user
        public ActionResult Profilee(string MemId)
        {
            UserModel um = db.Umodel.Find(MemId);
            // Debug.WriteLine("dddddddd"+um.Username);
            return View(path.dict["ProfileURL"], um);
        }
        #endregion
    }
}