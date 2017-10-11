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

        public JsonResult AddVehicle(string vehicleNo, string make, double capacity)
        {
            int count = db.vehicles.Where(v => v.VehicleNo == vehicleNo).Count();
            if(count>0)
            {
                return Json(new { success = true, Message = "Exsist" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Vehicles vehicle = new Vehicles();
                vehicle.VehicleNo = vehicleNo;
                vehicle.Capacity = capacity; //tonnage
                vehicle.Make = make;
                vehicle.AddedDate = DateTime.Now;
                vehicle.VehicleStatus = "Unassigned";
                vehicle.FuelLevel = "130"; //Lt
                vehicle.FuelConsumption = "4.2"; //Km/L
                vehicle.DistanceToTravel = Convert.ToString(Convert.ToDouble(vehicle.FuelLevel) * Convert.ToDouble(vehicle.FuelConsumption));


                DataContext dc2 = new DataContext();
                using (dc2)
                {
                    dc2.vehicles.Add(vehicle);
                    dc2.SaveChanges();
                }

                return Json(new { success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getVehicleDetails(int id)
        {
            var vehicleDetails = db.vehicles.Where(v => v.TableID == id).FirstOrDefault();
            var vehicleNo = vehicleDetails.VehicleNo;
            var make = vehicleDetails.Make;
            var capacity = vehicleDetails.Capacity;
            var status = vehicleDetails.VehicleStatus;
           

            return Json(new { success = true, vehicleNo= vehicleNo, make= make, capacity= capacity, status = status  }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditVehicleDetails(int EditVehicleID, string EditVehicleNo, string EditVehicleMake, double EditVehicleCapacity)
        {
            using (var db = new DataContext())
            {
                var vehicleResult = db.vehicles.Where(v => v.TableID == EditVehicleID).FirstOrDefault();
                vehicleResult.VehicleNo = EditVehicleNo;
                vehicleResult.Make = EditVehicleMake;
                vehicleResult.Capacity = EditVehicleCapacity;
                db.SaveChanges();
            }
            

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteVehicle(int deleteVehicleID)
        {
            using (var db = new DataContext())
            {
                var vehicleResult = db.vehicles.Where(v => v.TableID == deleteVehicleID).FirstOrDefault();
                db.vehicles.Remove(vehicleResult);
                db.SaveChanges();
            }
            
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}