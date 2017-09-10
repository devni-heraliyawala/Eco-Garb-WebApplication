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
        public ActionResult CustomerPayment()
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.usermodel = um;

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


        [HttpPost]
        public ActionResult SaveUnitPriceValues(string[] unitPriceList)
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            UpdateUnitPriceList(unitPriceList, um);

            return Json(new { res = true }, JsonRequestBehavior.AllowGet);
        }

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
                    ItemUnitPrice15 = unitPriceList[14],
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
    }
}