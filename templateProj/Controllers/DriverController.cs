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
                drivers driver = new drivers();
                driver.driverName = driverName;
                driver.NicID = NicID;
                driver.DrivingLicenseID = drivingLicenseID;
                driver.driverAge = age;
                driver.yearsOfExperience = yearsOfExperience;
                driver.contactNo = contactNo;
                driver.driverStatus = "Unoccupied";
                driver.workingHrsPerDay = 9;
                driver.LeftWorkingHrs = 9;

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
            var driverDetails = db.drivers.Where(d => d.driverID == id).FirstOrDefault();

            string nicNo = driverDetails.NicID;
            string driverName = driverDetails.driverName;
            string drivingLicense = driverDetails.DrivingLicenseID;
            int age = driverDetails.driverAge;
            int experience = driverDetails.yearsOfExperience;
            int contactNo = driverDetails.contactNo;

            return Json(new { success = true, nicNo= nicNo,driverName = driverName, drivingLicense = drivingLicense, age = age, experience = experience, contactNo = contactNo }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditDriver(string EditdriverNIC,int EditdriverID, string EditdriverName, string EditdriverlicenseID,int EditdriverAge, int EditdriverExperience, int EditdriverContactNo)
        {
            drivers driver = new drivers();
            driver.driverName = EditdriverName;
            driver.NicID = EditdriverNIC;
            driver.DrivingLicenseID = EditdriverlicenseID;
            driver.driverAge = EditdriverAge;
            driver.yearsOfExperience = EditdriverExperience;
            driver.contactNo = EditdriverContactNo;
            driver.driverID = EditdriverID;

            FormHandlingLayer fhl = new FormHandlingLayer();
            fhl.EditDriverInformation(driver);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteDriver(int deleteDriverID)
        {
            drivers driver = new drivers();
            driver.driverID = deleteDriverID;

            FormHandlingLayer fhl = new FormHandlingLayer();
            fhl.DeleteDriver(driver);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


    }
}