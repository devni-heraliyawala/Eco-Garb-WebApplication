using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using templateProj.Filters;
using templateProj.Models;

namespace templateProj.Controllers
{
    [SessionTimeOutFilter]
    [Authorize]
    public class CustomerRatingsController : Controller
    {
        DataContext db = new DataContext();
        // GET: CustomerRatings
        #region Load Default Data for Ratings Page

        public ActionResult CustomerRatings()
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.usermodel = um;
            homeViewModel.customerModel = db.customerModel.Where(c => c.RecyCompanyName == um.CompanyName).ToList(); ;
            return View(homeViewModel);
        }

        #endregion

        #region View Rating Details 

        public JsonResult GetRatingDetails(int id)
        {
            var customerDetails = db.customerModel.Where(c => c.TableID == id).FirstOrDefault();
            var companyName = customerDetails.CompanyName + " - " + customerDetails.Branch;
            var serviceRate = customerDetails.Customer_Rate_Company;
            var custRate = customerDetails.Company_Rate_Customer;
            var qty = db.customerTrashModel.Where(c => c.Username == customerDetails.Username);
            var totalQty = 0.0;

            foreach (var item in qty)
            {
                totalQty = totalQty + item.TotalQty;
            }

            //var qty = db.customerTrashModel.Where(c => c.Username == customerDetails.Username).Where(s => s.CollectingStatus == "Collected").Sum(t => Convert.ToDouble(t.TotalQty));

            return Json(new { success = true, companyName = companyName, serviceRate = serviceRate, qty = totalQty, custRate = custRate }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Change / Save Customer Ratings

        [HttpPost]
        public JsonResult SaveCustomerRatings(int id, string custRate)
        {
            try
            {
                UserModel um = db.Umodel.Find(HttpContext.Session["Uname"]);
                Customer custDetails = db.customerModel.Where(c => c.TableID == id).FirstOrDefault();

                if (custRate == "Rate the Customer" || custRate == "")
                {
                    custDetails.Company_Rate_Customer = null;
                }
                else
                {
                    custDetails.Company_Rate_Customer = custRate;
                }

                if (ModelState.IsValid)
                {
                    db.Entry(custDetails).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return Json(new { success = true });
        }

        #endregion

        #region View Total Trash Quantity Report

        public ActionResult TrashItemDetails(int id)
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);
            HomeController hc = new HomeController();
            RoutingAlgoController ra = new RoutingAlgoController();

            var custUsername = db.customerModel.Where(o => o.TableID == id).Select(u => u.Username).FirstOrDefault();
            var trashItemsList = db.trashItemModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();
            var typeList = db.trashMeasurementModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();
            var collectedLists = db.customerTrashModel.Where(u => u.Username == custUsername)/*.Where(s=>s.CollectingStatus!="Collected")*/;

            List<string> finalItemLNameist = hc.FindItemList(trashItemsList);
            List<string> finalTypeList = hc.FindTrashTypeList(typeList);
            List<List<int>> collectedQtyLists = new List<List<int>>();

            List<int> finalQtyList = new List<int>();
            foreach (var item in collectedLists)
            {
                collectedQtyLists.Add(ra.FindTrashQtyList(item));
            }

            var sum = 0;
            for (int i = 0; i < collectedQtyLists[0].Count; i++)
            {
                for (int j = 0; j < collectedQtyLists.Count; j++)
                {
                    sum = sum + collectedQtyLists[j][i];

                }
                finalQtyList.Add(sum);
                sum = 0;
            }


            //List<int> finalQtyList = ra.FindTrashQtyList(qtyList);

            var companyName = db.customerModel.Where(i => i.TableID == id).FirstOrDefault().CompanyName + " - " + db.customerModel.Where(i => i.TableID == id).FirstOrDefault().Branch;

            return Json(new { companyName = companyName, typeList = finalTypeList, itemList = finalItemLNameist, qtyList = finalQtyList }, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}