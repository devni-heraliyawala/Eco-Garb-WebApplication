using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml.Linq;
using templateProj.Models;
using templateProj.Property;

namespace templateProj.Controllers
{
    //[AllowAnonymous]
    public class LoginController : Controller
    {

        DataContext db = new DataContext();
        Paths path = new Paths();
        Encrypt hash = new Encrypt();

        #region Register Function
        //register new company user
        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public ActionResult Register(string companyName, string fullName,string recAddress, string fullUsername, string email, string confirmPassword)
        {
            try
            {
                DataContext dc = new DataContext();
                UserModel newUser = new UserModel();
                string salt = hash.CreateSalt();

                string hashPw = hash.HashPassword(confirmPassword, salt);

                int idNo = dc.Umodel.OrderByDescending(c => c.id).Select(c => c.id).FirstOrDefault();

                newUser.Username = fullUsername;
                newUser.password = hashPw;
                newUser.salt = salt;
                newUser.Email = email;
                newUser.FirstName = fullName;
                newUser.CompanyName = companyName;
                newUser.ProfilePic = "../../Styles/dist/img/user3-128x128.jpg";
                newUser.id = idNo + 1;

                DataContext dc2 = new DataContext();
                var count = dc2.Umodel.Where(t => t.CompanyName == companyName).Count();

                if(count <= 0)
                {
                    //company has not registered yet. so register the both user and the company
                    CreateDefaultTrashItems(companyName);

                    CreateDefaultMeasurementTypes(companyName);

                    CreateDefaultUnitConversions(companyName);

                    CreateCompanyGeoCoordinates(companyName,recAddress);
                }
                // else register the user only
                DataContext dc3 = new DataContext();
                using (dc3)
                {
                    dc3.Umodel.Add(newUser);
                    dc3.SaveChanges();
                }
               
              
                return Json(new { res = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", "Login");
            }



        }

        public void CreateDefaultTrashItems(string companyName)
        {
            //find the company name in the trash item table id not in the table then create a new row and fill
            // it with defaults else ignore 
            try
            {
                DataContext dc = new DataContext();
                TrashItems defaultItems = new TrashItems();

                defaultItems.CompanyName = companyName;
                defaultItems.ItemName1 = "Plastic";
                defaultItems.ItemName2 = "Glass";
                defaultItems.ItemName3 = "Polythene";
                defaultItems.ItemName4 = "E-waste";
                defaultItems.ItemName5 = "Cardboard";

                using (dc)
                {
                    dc.trashItemModel.Add(defaultItems);
                    dc.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void CreateDefaultMeasurementTypes(string companyName)
        {
            try
            {
                DataContext dc4 = new DataContext();

                TrashItemMeasurements defaultMeasurements = new TrashItemMeasurements();

                defaultMeasurements.CompanyName = companyName;
                defaultMeasurements.ItemName1 = "Plastic";
                defaultMeasurements.ItemType1 = "Kilogram";
                defaultMeasurements.ItemUnitPrice1 = "10";

                defaultMeasurements.ItemName2 = "Glass";
                defaultMeasurements.ItemType2 = "Bottle";
                defaultMeasurements.ItemUnitPrice2 = "5";

                defaultMeasurements.ItemName3 = "Polythene";
                defaultMeasurements.ItemType3 = "Kilogram";
                defaultMeasurements.ItemUnitPrice3 = "10";

                defaultMeasurements.ItemName4 = "E-waste";
                defaultMeasurements.ItemType4 = "Default Trash Bag";
                defaultMeasurements.ItemUnitPrice4 = "100";

                defaultMeasurements.ItemName5 = "Cardboard";
                defaultMeasurements.ItemType5 = "Default Trash Bag";
                defaultMeasurements.ItemUnitPrice5 = "100";

                using (dc4)
                {
                    dc4.trashMeasurementModel.Add(defaultMeasurements);
                    dc4.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CreateDefaultUnitConversions(string companyName)
        {
            try
            {
                DataContext dc5 = new DataContext();

                UnitConversion defaultUnitConversions = new UnitConversion();

                defaultUnitConversions.CompanyName = companyName;
                defaultUnitConversions.MeasurementType1 = "Bottle";
                defaultUnitConversions.Units1 = "0.5";

                defaultUnitConversions.MeasurementType2 = "Kilogram";
                defaultUnitConversions.Units2 = "10";

                defaultUnitConversions.MeasurementType3 = "Default Trash Bag";
                defaultUnitConversions.Units3 = "100";


                using (dc5)
                {
                    dc5.unitConversionModel.Add(defaultUnitConversions);
                    dc5.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CreateCompanyGeoCoordinates(string companyName, string recAddress)
        {
            try
            {
                DataContext dc6 = new DataContext();
                RecCompany recCompanyDetails = new RecCompany();
                //var address = "123 something st, somewhere";
                var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(recAddress));

                var request = WebRequest.Create(requestUri);
                var response = request.GetResponse();
                var xdoc = XDocument.Load(response.GetResponseStream());

                var result = xdoc.Element("GeocodeResponse").Element("result");
                var locationElement = result.Element("geometry").Element("location");
                var lat = locationElement.Element("lat");
                var lng = locationElement.Element("lng");

                recCompanyDetails.RecCompanyName = companyName;
                recCompanyDetails.Address = recAddress;
                recCompanyDetails.Lat = lat.ToString().Replace("<lat>","").Replace("</lat>","");
                recCompanyDetails.Lng = lng.ToString().Replace("<lng>", "").Replace("</lng>", "");


                Console.WriteLine("lat : " + lat + "  lng: " + lng);

                using (dc6)
                {
                    dc6.recCompanyModel.Add(recCompanyDetails);
                    dc6.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        #region Login Function
        // Login action
        public ActionResult Index()
        {

            return View("login");
        }

        // Login method
        [HttpPost]
        public ActionResult Login(string uname, string pass)
        {
            if (ModelState.IsValid)
            {
                SessionController s = new SessionController();
                try
                {
                    UserModel um = db.Umodel.Find(uname);

                    string hashPw = hash.HashPassword(pass, um.salt);

                    UserModel User = db.Umodel.Single(usr => usr.Username == uname
                        && usr.password == hashPw);

                    FormsAuthentication.SetAuthCookie(User.Username, true);

                    // Session["Role"] = Urole;
                    Session["Uname"] = User.Username;
                    Session["ProPic"] = User.ProfilePic;

                    return RedirectToAction("Home", "Home");
                }
                catch (System.InvalidOperationException e)
                {
                    ViewBag.errorMsg = "error";
                    return View();
                }
            }
            else
            {
                ViewBag.errorMsg = "error";
                return View();
            }
        }
        #endregion

        #region Revover Password Function
        // Recover Password page
        public ActionResult RecoverPass()
        {

            return View("ForgotPassword", null);
        }


        // New password entry
        public ActionResult NewPw(string pw, string username)
        {

            UserModel user = db.Umodel.Find(username);

            string salt = hash.CreateSalt();

            string hashPw = hash.HashPassword(pw, salt);

            user.password = hashPw;
            user.salt = salt;

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return View(path.dict["loginURL"]);
            }

            return View(path.dict["loginURL"]);
        }
#endregion

        #region Logout Function
        // Logout method
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
#endregion

        #region Common Functions
        public ActionResult CheckUserValidity(string username)
        {
            if (username == db.Umodel.Find(username).Username)
                return Json(new { res = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { res = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckPasswordValidity(string username, string password)
        {
            ///find passwrod then check password
            string salt = db.Umodel.Find(username).salt;

            string hashPw = hash.HashPassword(password, salt);

            UserModel User = db.Umodel.Single(usr => usr.Username == username
                && usr.password == hashPw);


            if (User.FirstName != null || User.FirstName != "")
                return Json(new { res = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { res = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MatchPasscode(string passcode, string username)
        {

            if (passcode == db.Umodel.Find(username).RecoverPasscode)
                return Json(new { res = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { res = false }, JsonRequestBehavior.AllowGet);
        }

#endregion
    }
}