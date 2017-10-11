using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using templateProj.Filters;
using templateProj.Models;

namespace templateProj.Controllers
{
    [SessionTimeOutFilter]
    [Authorize]
    public class CustomerPaymentController : Controller
    {
        DataContext db = new DataContext();
        // GET: CustomerPayment
        #region Load Unit Prices
        public ActionResult CustomerPayment()
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.usermodel = um;

            var unameList = db.customerModel.Where(c => c.RecyCompanyName == um.CompanyName).Select(n => n.Username).ToList();
            List<CustomerPayment> newPaymentModel = new List<CustomerPayment>();
            for (int i = 0; i < unameList.Count; i++)
            {
                var userName = unameList[i];

                newPaymentModel.Add(db.custPaymentModel.Where(u => u.Username == userName).OrderByDescending(t => t.TableID).FirstOrDefault());
            }
            homeViewModel.custPaymentModel = newPaymentModel;
            return View(homeViewModel);
        }

        public ActionResult LoadItemPrices()
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            //var trashItemsMeasurementList = db.trashMeasurementModel.Where(c => c.CompanyName == um.CompanyName).Select(k =>k.)
            var trashItemsMeasurementList = db.trashMeasurementModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();


            List<string> finalItemList = FindTrashItemList(trashItemsMeasurementList);

            List<string> finalUnitPriceList = FindUnitPriceList(trashItemsMeasurementList);


            return Json(new { itemList = finalItemList, unitPriceList = finalUnitPriceList }, JsonRequestBehavior.AllowGet);

        }

        public List<string> FindTrashItemList(TrashItemMeasurements trashItemsMeasurementList)
        {
            List<string> finalItemList = new List<string>();

            List<string> itemList = ItemListWithNullValues(trashItemsMeasurementList);

            for (int i = 0; i < itemList.Count(); i++)
            {
                if (itemList[i] != null && itemList[i] != "")
                {
                    finalItemList.Add(itemList[i]);
                }
            }

            return finalItemList;
        }

        public List<string> FindUnitPriceList(TrashItemMeasurements trashItemsMeasurementList)
        {
            List<string> finalUnitPriceList = new List<string>();

            List<string> priceList = UnitPriceListWithNullValues(trashItemsMeasurementList);

            for (int i = 0; i < priceList.Count(); i++)
            {
                if (priceList[i] != null && priceList[i] != "")
                {
                    finalUnitPriceList.Add(priceList[i]);
                }
            }

            return finalUnitPriceList;
        }

        public List<string> ItemListWithNullValues(TrashItemMeasurements trashItemsMeasurementList)
        {
            List<string> itemList = new List<string>();
            itemList.Add(trashItemsMeasurementList.ItemName1);
            itemList.Add(trashItemsMeasurementList.ItemName2);
            itemList.Add(trashItemsMeasurementList.ItemName3);
            itemList.Add(trashItemsMeasurementList.ItemName4);
            itemList.Add(trashItemsMeasurementList.ItemName5);
            itemList.Add(trashItemsMeasurementList.ItemName6);
            itemList.Add(trashItemsMeasurementList.ItemName7);
            itemList.Add(trashItemsMeasurementList.ItemName8);
            itemList.Add(trashItemsMeasurementList.ItemName9);
            itemList.Add(trashItemsMeasurementList.ItemName10);
            itemList.Add(trashItemsMeasurementList.ItemName11);
            itemList.Add(trashItemsMeasurementList.ItemName12);
            itemList.Add(trashItemsMeasurementList.ItemName13);
            itemList.Add(trashItemsMeasurementList.ItemName14);
            itemList.Add(trashItemsMeasurementList.ItemName15);
            return itemList;
        }

        public List<string> UnitPriceListWithNullValues(TrashItemMeasurements trashItemsMeasurementList)
        {
            List<string> priceList = new List<string>();
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice1);
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice2);
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice3);
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice4);
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice5);
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice6);
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice7);
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice8);
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice9);
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice10);
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice11);
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice12);
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice13);
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice14);
            priceList.Add(trashItemsMeasurementList.ItemUnitPrice15);
            return priceList;
        }

        #endregion

        #region Save Unit Prices
        [HttpPost]
        public ActionResult SaveUnitPriceValues(string[] unitPriceList)
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            UpdateUnitPriceList(unitPriceList, um);

            return Json(new { res = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update Unit Prices
        public void UpdateUnitPriceList(string[] unitPriceList, UserModel um)
        {
            var trashItemsMeasurementList = db.trashMeasurementModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();
            var tableId = trashItemsMeasurementList.TableID;
            try
            {

                var trashItmMeasurement = new TrashItemMeasurements()
                {
                    TableID = tableId,
                    ItemUnitPrice1 = unitPriceList[0],
                    ItemUnitPrice2 = unitPriceList[1],
                    ItemUnitPrice3 = unitPriceList[2],
                    ItemUnitPrice4 = unitPriceList[3],
                    ItemUnitPrice5 = unitPriceList[4],
                    ItemUnitPrice6 = unitPriceList[5],
                    ItemUnitPrice7 = unitPriceList[6],
                    ItemUnitPrice8 = unitPriceList[7],
                    ItemUnitPrice9 = unitPriceList[8],
                    ItemUnitPrice10 = unitPriceList[9],
                    ItemUnitPrice11 = unitPriceList[10],
                    ItemUnitPrice12 = unitPriceList[11],
                    ItemUnitPrice13 = unitPriceList[12],
                    ItemUnitPrice14 = unitPriceList[13],
                    ItemUnitPrice15 = unitPriceList[14]
                };

                using (DataContext db2 = new DataContext())
                {
                    db2.trashMeasurementModel.Attach(trashItmMeasurement);
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice1).IsModified = true;
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice2).IsModified = true;
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice3).IsModified = true;
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice4).IsModified = true;
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice5).IsModified = true;
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice6).IsModified = true;
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice7).IsModified = true;
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice8).IsModified = true;
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice9).IsModified = true;
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice10).IsModified = true;
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice11).IsModified = true;
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice12).IsModified = true;
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice13).IsModified = true;
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice14).IsModified = true;
                    db2.Entry(trashItmMeasurement).Property(x => x.ItemUnitPrice15).IsModified = true;
                    db2.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        #region Find Trash Item Details by temp ID

        public ActionResult FindTrashItemDetails(int id)
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);
            HomeController hc = new HomeController();
            RoutingAlgoController ra = new RoutingAlgoController();

            var custUsername = db.custPaymentModel.Where(o => o.TableID == id).Select(u => u.Username).FirstOrDefault();
            var trashItemsList = db.trashItemModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();
            var typeList = db.trashMeasurementModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();
            var qtyList = db.customerTrashModel.Where(u => u.Username == custUsername).OrderByDescending(t=>t.TableID).FirstOrDefault();

            List<string> finalItemLNameist = hc.FindItemList(trashItemsList);
            List<string> finalTypeList = hc.FindTrashTypeList(typeList);
            List<int> finalQtyList = ra.FindTrashQtyList(qtyList);

            var companyName = db.custPaymentModel.Where(i => i.TableID == id).FirstOrDefault().CompanyName + " - " + db.custPaymentModel.Where(i => i.TableID == id).FirstOrDefault().Branch;
            var totQty = db.customerTrashModel.Where(u => u.Username == custUsername).OrderByDescending(t => t.TableID).Select(q=>q.TotalQty).FirstOrDefault();
            var qtySummary = "Total Latest Quantity : " + totQty.ToString();
            return Json(new { qtySummary= qtySummary, companyName = companyName, typeList = finalTypeList, itemList = finalItemLNameist, qtyList = finalQtyList }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Find Customer Payment Details by table ID

        public ActionResult FindPaymentDetails(int id)
        {
            var custUsername = db.custPaymentModel.Where(o => o.TableID == id).Select(u => u.Username).FirstOrDefault();

            var companyName = db.custPaymentModel.Where(i => i.TableID == id).FirstOrDefault().CompanyName + " - " + db.custPaymentModel.Where(i => i.TableID == id).FirstOrDefault().Branch;
            var totQty = db.customerTrashModel.Where(u => u.Username == custUsername).OrderByDescending(t => t.TableID).Select(q => q.TotalQty).FirstOrDefault();
            var qtySummary = "Total Latest Quantity : " + totQty.ToString();
            var paymentDate = db.custPaymentModel.Where(t=>t.TableID ==id).Select(d=>d.PaymentDate).FirstOrDefault();
            var paymentStatus = db.custPaymentModel.Where(t => t.TableID == id).Select(s=>s.PaymentStatus).FirstOrDefault();
            var amount = db.custPaymentModel.Where(t => t.TableID == id).Select(d => d.TotalAmount).FirstOrDefault();
            List<string> paymentDetails = new List<string>();

            paymentDetails.Add(companyName);
            paymentDetails.Add(qtySummary);
            paymentDetails.Add(paymentDate);
            paymentDetails.Add(paymentStatus);
            paymentDetails.Add(amount.ToString());


            return Json(new { paymentDetails = paymentDetails },JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Proceed the Payment
        [HttpPost]
        public ActionResult ProceedPayment(int id)
        {
            try
            {
                var payment = new CustomerPayment() {TableID = id,PaymentDate = DateTime.Now.ToString("yyyy-MM-dd"),PaymentStatus = "Proceed" };
                Console.Write("ppppp : "+payment.PaymentDate);
                using (DataContext dc3 =new DataContext())
                {
                    db.custPaymentModel.Attach(payment);
                    db.Entry(payment).Property(p=>p.PaymentDate).IsModified = true;
                    db.Entry(payment).Property(p => p.PaymentStatus).IsModified = true;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                Console.Write(ex.Message);
                Debug.WriteLine(ex.Message);
            }

            return Json(new{ response = true},JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Monthly Amount report 
        public ActionResult AmountReport(int id)
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);
            HomeController hc = new HomeController();
            RoutingAlgoController ra = new RoutingAlgoController();

            var custUsername = db.custPaymentModel.Where(o => o.TableID == id).Select(u => u.Username).FirstOrDefault();
            var trashItemsList = db.trashItemModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();
            var typeList = db.trashMeasurementModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();
            var qtyList = db.customerTrashModel.Where(u => u.Username == custUsername).OrderByDescending(t => t.TableID).FirstOrDefault();

            List<string> finalItemLNameist = hc.FindItemList(trashItemsList);
            List<string> finalTypeList = hc.FindTrashTypeList(typeList);
            List<int> finalQtyList = ra.FindTrashQtyList(qtyList);
            List<string> finalUnitPriceList = FindUnitPriceList(typeList);
            List<string> amountList = new List<string>();

            var totAmount = 0.0;
            for (int i = 0; i < finalItemLNameist.Count; i++)
            {
                var currentAmt = finalQtyList[i] * Convert.ToDouble(finalUnitPriceList[i]);
                totAmount = totAmount + currentAmt;
                amountList.Add("    =    "+ currentAmt.ToString());
            }

            var companyName = db.custPaymentModel.Where(i => i.TableID == id).FirstOrDefault().CompanyName + " - " + db.custPaymentModel.Where(i => i.TableID == id).FirstOrDefault().Branch;
            var totAmt = db.custPaymentModel.Where(t=>t.TableID == id).Select(a=>a.TotalAmount).FirstOrDefault();
            var amountSummary = "Total Amount (Rs.) : " + totAmount.ToString();



            return Json(new { amountList = amountList, amountSummary = amountSummary, companyName = companyName, typeList = finalTypeList, itemList = finalItemLNameist, qtyList = finalQtyList,unitPriceList= finalUnitPriceList }, JsonRequestBehavior.AllowGet);

        }
        #endregion


    }
}