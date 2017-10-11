using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using templateProj.Models;

namespace templateProj.Controllers
{
    public class TripController : Controller
    {
        DataContext db = new DataContext();

        public ActionResult TripDataProfile()
        {

            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            var trips = db.trips.ToList();
            var availableDrivers = db.drivers.Where(d => d.DriverStatus == "Unoccupied").ToList();

            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.usermodel = um;
            homeViewModel.trips = trips;
            homeViewModel.drivers = availableDrivers;
            

            return View(homeViewModel);

        }

        public JsonResult AssignDriverToTrip(int driverID,int tripID)
        {
           
                var assignedDriver = db.drivers.Where(d => d.TableID == driverID).FirstOrDefault();
                var assignedDriverName = assignedDriver.DriverName;

            using (var db1 = new DataContext())
            {
                var driverResult = db1.drivers.Where(d => d.TableID == driverID).FirstOrDefault();
                driverResult.DriverStatus = "Occupied";
                db1.SaveChanges();
            }

            using (var db = new DataContext())
                {
                    var tripResult = db.trips.Where(t => t.TableID == tripID).FirstOrDefault();
                    tripResult.AssignedDriverID = Convert.ToString(driverID);
                    tripResult.AssignedDriverName = assignedDriverName;
                    db.SaveChanges();

                    
                }

                
                return Json(new { sucess = true, Msg = "Success" }, JsonRequestBehavior.AllowGet);
            
            
        }

        public JsonResult getAssignedDriverID(int id)
        {
            var tripRes = db.trips.Where(t => t.TableID == id).FirstOrDefault();
            var assignedDriverID = tripRes.AssignedDriverID;

            return Json(new { success = true, assignedDriverID=assignedDriverID }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAvailableVehicles(int id)
        {
            var tripRes = db.trips.Where(t => t.TableID == id).FirstOrDefault();
            var assignedVehicleID = tripRes.AssignedVehicleID;
           // var distance = tripRes.RouteDistance;

            IList<Vehicles> vehicleList = new List<Vehicles>();

            var tripResult = db.trips.Where(t => t.TableID == id).FirstOrDefault();
            var distance = tripResult.RouteDistance;

            var availableVehicles = db.vehicles.Where(v => v.VehicleStatus == "Unassigned").ToList();
            foreach(var item in availableVehicles)
            {
                if(Convert.ToDouble(item.DistanceToTravel)>= Convert.ToDouble(distance))
                {
                    vehicleList.Add(item);
                }
            }
            int vehicleListCount = vehicleList.Count();

            return Json(new { success = true, vehicleList = vehicleList, vehicleListCount = vehicleListCount, assignedVehicleID= assignedVehicleID }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AssignVehicleToTrip(int vehicleID,int tripID)
        {
            var vehicleResult = db.vehicles.Where(v => v.TableID == vehicleID).FirstOrDefault();
            var vehicleNo = vehicleResult.VehicleNo;
            var fuelLevel = vehicleResult.FuelLevel;

            var Trip = db.trips.Where(t => t.TableID == tripID).FirstOrDefault();
            var tripDistance = Trip.RouteDistance;

            var newFuelLevel = Convert.ToDouble(fuelLevel) - ((1 / 4.2) * Convert.ToDouble(tripDistance));
            var distanceCanTravel = 4.2 * newFuelLevel;

            using (var db1 = new DataContext())
            {
                var vehicleResults = db1.vehicles.Where(v => v.TableID == vehicleID).FirstOrDefault();
                vehicleResults.VehicleStatus = "Assigned";
                vehicleResults.FuelLevel = Convert.ToString(newFuelLevel);
                vehicleResults.DistanceToTravel = Convert.ToString(distanceCanTravel);

                db1.SaveChanges();
            }

            using (var db2 = new DataContext())
            {
                var tripResult = db2.trips.Where(t => t.TableID == tripID).FirstOrDefault();
                tripResult.AssignedVehicleID = Convert.ToString(vehicleID);
                tripResult.AssignedVehicleNo = vehicleNo;
                db2.SaveChanges();
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FuelManagement()
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            var vehicles = db.vehicles.ToList();

            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.usermodel = um;
            homeViewModel.vehicles = vehicles;

            return View(homeViewModel);
        }

        public JsonResult getVehicleStatus(int vehicleNoID)
        {
            var vehicle = db.vehicles.Where(d => d.TableID == vehicleNoID).FirstOrDefault();
            var vehicleStatus = vehicle.VehicleStatus;
            var fuelEfficiency = vehicle.FuelConsumption;
            var tonnage = "10";
            var availableFuelLevel = vehicle.FuelLevel;
            return Json(new { success = true, vehicleStatus = vehicleStatus, fuelEfficiency= fuelEfficiency, tonnage= tonnage, availableFuelLevel= availableFuelLevel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult xx()
        {
            //string uname = HttpContext.Session["Uname"].ToString();
            //UserModel um = db.Umodel.Find(uname);

            //var vehicles = db.vehicles.ToList();

            //HomeViewModel homeViewModel = new HomeViewModel();
            //homeViewModel.usermodel = um;
            //homeViewModel.vehicles = vehicles;

            return View();
        }

    }
}