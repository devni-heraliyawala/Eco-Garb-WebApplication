using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using templateProj.Models;

namespace templateProj.Controllers
{
    public class VehicleController : Controller
    {
        DataContext db = new DataContext();

        public ActionResult VehicleProfile()
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            var vechicles = db.vehicles.ToList();

            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.usermodel = um;
            homeViewModel.vehicles = vechicles;

            return View(homeViewModel);
        }

        public JsonResult AddVehicle(string vehicleNo, string make, double capacity, string trashType)
        {
            int count = db.vehicles.Where(v => v.vehicleNo == vehicleNo).Count();
            if(count>0)
            {
                return Json(new { success = true, Message = "Exsist" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                vehicles vehicle = new vehicles();
                vehicle.vehicleNo = vehicleNo;
                vehicle.capacity = capacity;
                vehicle.Make = make;
                vehicle.AddedDate = DateTime.Now;
                vehicle.TrashTypeCarried = trashType;
                vehicle.vehicleStatus = "Unassigned";

                FormHandlingLayer fhl = new FormHandlingLayer();
                fhl.AddNewVehicle(vehicle);

                return Json(new { success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getVehicleDetails(int id)
        {
            var vehicleDetails = db.vehicles.Where(v => v.vehicleID == id).FirstOrDefault();
            var vehicleNo = vehicleDetails.vehicleNo;
            var make = vehicleDetails.Make;
            var capacity = vehicleDetails.capacity;
            var status = vehicleDetails.vehicleStatus;
            var type = vehicleDetails.TrashTypeCarried;

            return Json(new { success = true, vehicleNo= vehicleNo, make= make, capacity= capacity, status = status , type = type }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditVehicleDetails(int EditVehicleID, string EditVehicleNo, string EditVehicleMake, double EditVehicleCapacity, string EditVehicleType)
        {
            vehicles vehicle = new vehicles();
            vehicle.vehicleID = EditVehicleID;
            vehicle.vehicleNo = EditVehicleNo;
            vehicle.Make = EditVehicleMake;
            vehicle.capacity = EditVehicleCapacity;
            vehicle.TrashTypeCarried = EditVehicleType;

            FormHandlingLayer fhl = new FormHandlingLayer();
            fhl.EditVehicleDetails(vehicle);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteVehicle(int deleteVehicleID)
        {
            vehicles vehicle = new vehicles();
            vehicle.vehicleID = deleteVehicleID;

            FormHandlingLayer fhl = new FormHandlingLayer();
            fhl.DeleteVehicle(vehicle);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}