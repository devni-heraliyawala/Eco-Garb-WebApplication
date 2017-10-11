using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using templateProj.Models;

namespace templateProj.Controllers
{
    public class RoutingAlgoController : Controller
    {
        DataContext db = new DataContext();
        HomeController hc = new HomeController();
        Vector2D sourceVector;
        //List<Vector2D> verticesList = new List<Vector2D>();
        //DijkstrasController dij = new DijkstrasController();
        #region Load Default Customer Locations

        /// <summary>
        /// This function returns the defalut customer locations with additional info which needs for info windows
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadDefaultCustomerLocations()
        {
            //calculates the total trash quantites and update related tables 
            CalculateTotalQty();
            //FindBestRoute();

            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            var addressList = db.customerModel.Where(c => c.RecyCompanyName == um.CompanyName).Where(b => b.TrashStatus != "Collected").Select(a => a.Address).ToArray();
            var cnameList = db.customerModel.Where(c => c.RecyCompanyName == um.CompanyName).Where(b => b.TrashStatus != "Collected").Select(a => a.CompanyName).ToArray();
            var branchList = db.customerModel.Where(c => c.RecyCompanyName == um.CompanyName).Where(b => b.TrashStatus != "Collected").Select(a => a.Branch).ToArray();
            var latList = db.customerModel.Where(c => c.RecyCompanyName == um.CompanyName).Where(b => b.TrashStatus != "Collected").Select(a => a.Lat).ToArray();
            var lngList = db.customerModel.Where(c => c.RecyCompanyName == um.CompanyName).Where(b => b.TrashStatus != "Collected").Select(a => a.Lng).ToArray();

            var totalQtyList = db.customerTrashModel.Where(c => c.CompanyName == um.CompanyName).Where(b => b.CollectingStatus != "Collected").Select(a => a.TotalQty).ToArray();


            return Json(new { addressList = addressList, cnameList = cnameList, branchList = branchList, totalQtyList = totalQtyList, latList = latList, lngList = lngList }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Calculates the Total Trash Quantities

        /// <summary>
        /// Calculate the total trash quantites for each related customers by considering
        /// the unit conversion and update the totalQty column value 
        /// </summary>

        public void CalculateTotalQty()
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);
            List<string> finalItemList, finalTypeList, finalUnitTypeList, finalUnitValueList;
            List<int> finalQtyList;
            List<string> finalTotalQtyList = new List<string>();

            hc.LoadTrashItemNamesTypesUnitConversions(um, out finalItemList, out finalTypeList, out finalUnitTypeList, out finalUnitValueList);

            var trashQtyLists = db.customerTrashModel.Where(c => c.CompanyName == um.CompanyName).Where(t => t.CollectingStatus == "Requested");

            //do the total calculation for the whole related customers. 
            foreach (var rowTrashQtyList in trashQtyLists)
            {
                //add quantites to an array
                finalQtyList = FindTrashQtyList(rowTrashQtyList);

                //check for new new item existance
                if (finalQtyList.Count < finalItemList.Count)
                {
                    int diff = finalItemList.Count - finalQtyList.Count;

                    for (int i = 0; i < diff; i++)
                    {
                        finalQtyList.Add(0);
                    }
                }

                // calculate total qty
                var totalQty = 0;
                for (int i = 0; i < finalItemList.Count(); i++)
                {
                    if (finalTypeList[i] == "Bottle")
                    {
                        totalQty = totalQty + finalQtyList[i] * Convert.ToInt32(finalUnitValueList[0]);

                    }
                    else if (finalTypeList[i] == "Kilogram")
                    {
                        totalQty = totalQty + finalQtyList[i] * Convert.ToInt32(finalUnitValueList[1]);
                    }
                    else if (finalTypeList[i] == "Default Trash Bag")
                    {
                        totalQty = totalQty + finalQtyList[i] * Convert.ToInt32(finalUnitValueList[2]);
                    }

                }
                finalTotalQtyList.Add(totalQty.ToString());
            }

            //update the total column
            UpdateFinalTotalQty(finalTotalQtyList.ToArray(), um);
        }

        private void UpdateFinalTotalQty(string[] finalTotalQtyList, UserModel um)
        {
            var totalTrashQtyLists = db.customerTrashModel.Where(c => c.CompanyName == um.CompanyName).Where(t => t.CollectingStatus == "Requested");
            var i = 0;
            foreach (var totalTrashQty in totalTrashQtyLists)
            {

                var customerTrash = new CustomerTrash()
                {
                    TableID = totalTrashQty.TableID,
                    TotalQty = Convert.ToDouble(finalTotalQtyList[i++])

                };

                using (DataContext db = new DataContext())
                {
                    db.customerTrashModel.Attach(customerTrash);
                    db.Entry(customerTrash).Property(x => x.TotalQty).IsModified = true;
                    db.SaveChanges();
                }
            }


        }

        private List<int> QtyListWithNullValues(CustomerTrash rowTrashQtyList)
        {
            List<int> qtyList = new List<int>();
            qtyList.Add(rowTrashQtyList.QtyItem1);
            qtyList.Add(rowTrashQtyList.QtyItem2);
            qtyList.Add(rowTrashQtyList.QtyItem3);
            qtyList.Add(rowTrashQtyList.QtyItem4);
            qtyList.Add(rowTrashQtyList.QtyItem5);
            qtyList.Add(rowTrashQtyList.QtyItem6);
            qtyList.Add(rowTrashQtyList.QtyItem7);
            qtyList.Add(rowTrashQtyList.QtyItem8);
            qtyList.Add(rowTrashQtyList.QtyItem9);
            qtyList.Add(rowTrashQtyList.QtyItem10);
            qtyList.Add(rowTrashQtyList.QtyItem11);
            qtyList.Add(rowTrashQtyList.QtyItem12);
            qtyList.Add(rowTrashQtyList.QtyItem13);
            qtyList.Add(rowTrashQtyList.QtyItem14);
            qtyList.Add(rowTrashQtyList.QtyItem15);
            return qtyList;
        }

        public List<int> FindTrashQtyList(CustomerTrash rowTrashQtyList)
        {
            List<int> finalQtyList = new List<int>();

            List<int> qtyList = QtyListWithNullValues(rowTrashQtyList);

            for (int i = 0; i < qtyList.Count(); i++)
            {
                if (qtyList[i] >= 0)
                {
                    finalQtyList.Add(qtyList[i]);
                }
            }

            return finalQtyList;
        }

        #endregion

        #region Find the Best Route Based on the Specified Quantity Value

        public void QualifiedCustomerDetailLists(UserModel um, out string[] addressList, out string[] cnameList, out string[] branchList, out string[] latList, out string[] lngList, out string[] totalQtyList)
        {
            addressList = db.customerModel.Where(c => c.RecyCompanyName == um.CompanyName).Where(b => b.TrashStatus == "Requested").Select(a => a.Address).ToArray();
            cnameList = db.customerModel.Where(c => c.RecyCompanyName == um.CompanyName).Where(b => b.TrashStatus == "Requested").Select(a => a.CompanyName).ToArray();
            branchList = db.customerModel.Where(c => c.RecyCompanyName == um.CompanyName).Where(b => b.TrashStatus == "Requested").Select(a => a.Branch).ToArray();
            latList = db.customerModel.Where(c => c.RecyCompanyName == um.CompanyName).Where(b => b.TrashStatus == "Requested").Select(a => a.Lat).ToArray();
            lngList = db.customerModel.Where(c => c.RecyCompanyName == um.CompanyName).Where(b => b.TrashStatus == "Requested").Select(a => a.Lng).ToArray();
            totalQtyList = db.customerTrashModel.Where(c => c.CompanyName == um.CompanyName).Where(b => b.CollectingStatus == "Requested").Select(a => a.TotalQty.ToString()).ToArray();
        }
        #endregion

        #region Update Customer, Customer trash, Trip statuses
        public ActionResult AddNewRoute(string routeLatList, string routeLngList, string routeDistance)
        {
            try
            {
                DataContext dc = new DataContext();
                Trips newTrip = new Trips();

                newTrip.RouteLatList = routeLatList;
                newTrip.RouteLngList = routeLngList;
                newTrip.RouteDistance = routeDistance;
                newTrip.TripDate = DateTime.Now.ToShortDateString();
                newTrip.TripStatus = "Open";

                using (dc)
                {
                    dc.trips.Add(newTrip);
                    dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error : " + ex.Message);
                Debug.WriteLine("error : " + ex.Message);
            }

            UpdateCustomerModelStatus(routeLatList, routeLngList);

            return Json(new {response = true },JsonRequestBehavior.AllowGet);
        }


        private void UpdateCustomerModelStatus(string routeLatList, string routeLngList)
        {
            DataContext dc1 = new DataContext();
            //DataContext dc2 = new DataContext();
            string[] latList = routeLatList.Split(',').Select(n => n.ToString()).ToArray();
            string[] lngList = routeLngList.Split(',').Select(n => n.ToString()).ToArray();
            List<int> tableIds = new List<int>();
            List<string> customers = new List<string>();
            for (int i = 1; i < latList.Length; i++)
            {
                var lat = latList[i];
                var lng = lngList[i];
                var cust = dc1.customerModel.Where(l => l.Lat == lat).Where(g => g.Lng == lng).FirstOrDefault();
                tableIds.Add(cust.TableID);
                customers.Add(cust.Username);
            }

            using (var dc2 = new DataContext())
            {
                foreach (var result in dc2.customerModel.Where(x => tableIds.Contains(x.TableID)).ToList())
                {
                    result.TrashStatus = "Assigned";
                }
                dc2.SaveChanges();
            }

            UpdateCustomerTrashModelStatus(customers);


        }

        private void UpdateCustomerTrashModelStatus(List<string> customers)
        {
            using (var dc3 = new DataContext())
            {
                foreach (var result in dc3.customerTrashModel.Where(x => customers.Contains(x.Username)).ToList())
                {
                    result.CollectingStatus = "Assigned";
                }
                dc3.SaveChanges();
            }
        }

        #endregion

    }
}