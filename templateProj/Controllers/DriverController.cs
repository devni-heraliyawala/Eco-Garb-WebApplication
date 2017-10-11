using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using templateProj.Filters;
using templateProj.Models;

namespace templateProj.Controllers
{
    public class DriverController : Controller
    {
        DataContext db = new DataContext();
        
        public ActionResult DriverProfiles()
        {
           
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);
            
           
            var drivers = db.drivers.ToList();

            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.usermodel = um;
            homeViewModel.drivers = drivers;

            return View(homeViewModel);
            
        }

        public JsonResult AddDriver(string driverName, string NicID, string drivingLicenseID, int age, int yearsOfExperience,int contactNo)
        {
            var driverCount = db.drivers.Where(d => d.NicID == NicID).Count();

            if(driverCount>0)
            {
                return Json(new { success = true, Message = "Exsist" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Drivers driver = new Drivers();
                driver.DriverName = driverName;
                driver.NicID = NicID;
                driver.DrivingLicenseID = drivingLicenseID;
                driver.DriverAge = age;
                driver.YearsOfExperience = yearsOfExperience;
                driver.ContactNo = contactNo;
                driver.DriverStatus = "Unoccupied";
                driver.WorkingHrsPerDay = 9;
                driver.LeftWorkingHrsPerDay = 9;

                DataContext dc2 = new DataContext();
                using (dc2)
                {
                    dc2.drivers.Add(driver);
                    dc2.SaveChanges();
                }

                return Json(new { success = true ,Message="Success"}, JsonRequestBehavior.AllowGet);
            }
            
        }

        public JsonResult getDriverDetails(int id)
        {
            var driverDetails = db.drivers.Where(d => d.TableID == id).FirstOrDefault();

            string nicNo = driverDetails.NicID;
            string driverName = driverDetails.DriverName;
            string drivingLicense = driverDetails.DrivingLicenseID;
            int age = driverDetails.DriverAge;
            int experience = driverDetails.YearsOfExperience;
            int contactNo = driverDetails.ContactNo;

            return Json(new { success = true, nicNo= nicNo,driverName = driverName, drivingLicense = drivingLicense, age = age, experience = experience, contactNo = contactNo }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditDriver(string EditdriverNIC,int EditdriverID, string EditdriverName, string EditdriverlicenseID,int EditdriverAge, int EditdriverExperience, int EditdriverContactNo)
        {
            using (var db = new DataContext())
            {
                var driverResult = db.drivers.Where(d => d.TableID == EditdriverID).FirstOrDefault();
                driverResult.DriverName = EditdriverName;
                driverResult.NicID = EditdriverNIC;
                driverResult.DrivingLicenseID = EditdriverlicenseID;
                driverResult.DriverAge = EditdriverAge;
                driverResult.YearsOfExperience = EditdriverExperience;
                driverResult.ContactNo = EditdriverContactNo;
                db.SaveChanges();
            }
            
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteDriver(int deleteDriverID)
        {
            using (var db = new DataContext())
            {
                var driverResult = db.drivers.Where(d => d.TableID == deleteDriverID).FirstOrDefault();
                db.drivers.Remove(driverResult);
                db.SaveChanges();

            }
            
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


    }
}