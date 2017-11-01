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
    [SessionTimeOutFilter]
    [Authorize]
    public class HomeController : Controller
    {
        DataContext db = new DataContext();
        MeasurementTypes mt = new MeasurementTypes();

        #region View Admin Dashboard
        public ActionResult Home()
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.usermodel = um;

            return View(homeViewModel);
        }

        #endregion

        #region Trash Measurement Function
        public ActionResult LoadMeasurementTypes()
        {
            var measurementTyList = db.measurementTypesModel.ToArray();

            List<string> idList = new List<string>();
            List<string> typeList = new List<string>();

            foreach (var item in measurementTyList)
            {
                idList.Add(item.TableID.ToString());
                typeList.Add(item.MeasurementType1);
            }

            //ViewBag.idList = idList;
            //ViewBag.typeList = typeList;

            return Json(new { idList = idList, typeList = typeList }, JsonRequestBehavior.AllowGet);

            //return Json(measurementTyList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadTrashItems()
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            //var trashItemsMeasurementList = db.trashMeasurementModel.Where(c => c.CompanyName == um.CompanyName).Select(k =>k.)
            List<string> finalItemList, finalTypeList, finalUnitTypeList, finalUnitValueList;

            LoadTrashItemNamesTypesUnitConversions(um, out finalItemList, out finalTypeList, out finalUnitTypeList, out finalUnitValueList);

            return Json(new { itemList = finalItemList, typeList = finalTypeList, unitTypeList = finalUnitTypeList, unitValueList = finalUnitValueList }, JsonRequestBehavior.AllowGet);

        }

        public void LoadTrashItemNamesTypesUnitConversions(UserModel um, out List<string> finalItemList, out List<string> finalTypeList, out List<string> finalUnitTypeList, out List<string> finalUnitValueList)
        {
            var trashItemsMeasurementList = db.trashMeasurementModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();

            var unitConversionList = db.unitConversionModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();

            //for trash measurement

            finalItemList = FindTrashItemList(trashItemsMeasurementList);
            finalTypeList = FindTrashTypeList(trashItemsMeasurementList);

            //for unit converion 

            finalUnitTypeList = FindUnitTypeList(unitConversionList);
            finalUnitValueList = FindUnitValueList(unitConversionList);
        }

        public ActionResult EditTrashMeasurementTypes()
        {
            var measurementTypeList = db.measurementTypesModel.Select(t => t.MeasurementType1).ToList();

            return Json(new { measurementTypeList = measurementTypeList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveMeasurementTypes(string[] measurementTypeList)
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            UpdateMeasurementTypes(measurementTypeList, um);

            return Json(new { res = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        #endregion

        #region Add / Remove Trash Item Functions

        public ActionResult AddTrashItem(string itemName, string itemType)
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            try
            {
                UpdateItemMeasurementTable(itemName, itemType, um);

                UpdateTrashItemTable(itemName, um);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new { res = true }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult RemoveTrashItem(string[] itemList)
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);
            bool flag = false;

            try
            {
                using (DataContext dc = new DataContext())
                {

                    var trashItemList = dc.trashMeasurementModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();
                    var tableId = trashItemList.TableID;
                    List<string> RemoveItemList = new List<string>();
                    List<string> fullItemList = ItemListWithNullValues(trashItemList);
                    List<string> fullTypeList = TypeListWithNullValues(trashItemList);


                    //add items to be deleted to a list
                    for (int i = 0; i < itemList.Length; i++)
                    {
                        RemoveItemList.Add(itemList[i]);
                    }

                    //if customers still using that trash type, then it can't be deleted.

                    var customers = dc.customerTrashModel.Where(r => r.CompanyName == um.CompanyName).Where(c => c.CollectingStatus != "Collected");
                    
                    //if customers are still using. then it return true.else false. can delete only
                    // when flag is false.
                    flag = CheckForTrashTypeUsage(RemoveItemList, customers, flag);

                    if (!flag)
                    {
                        // fill that list with null values until it comes for the default state
                        for (int i = itemList.Length - 1; i < fullItemList.Count(); i++)
                        {
                            RemoveItemList.Add(null);
                        }


                        // if items about to delete is found set it to null
                        for (int i = 0; i < fullItemList.Count(); i++)
                        {
                            for (int j = 0; j < RemoveItemList.Count(); j++)
                            {
                                if (fullItemList[i] == RemoveItemList[j])
                                {
                                    //modify that item.. remove that item from the both tables ..set to null will be fine
                                    fullItemList[i] = null;
                                    fullTypeList[i] = null;
                                }
                            }
                        }

                        UpdateItemNames(fullItemList.ToArray(), um);
                        UpdateMeasurementTypes(fullTypeList.ToArray(), um);
                        ItemTableUpdateItemNames(fullItemList.ToArray(), um);

                        dc.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Can not delete. Because some customers still have that type of garbage");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new { res = flag }, JsonRequestBehavior.AllowGet);
        }

        private static bool CheckForTrashTypeUsage(List<string> RemoveItemList, IQueryable<CustomerTrash> customers, bool flag)
        {
            for (int i = 0; i < RemoveItemList.Count; i++)
            {
                foreach (var customer in customers)
                {
                    if (!String.IsNullOrEmpty(customer.ItemName1) && customer.ItemName1 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else if (!String.IsNullOrEmpty(customer.ItemName2) && customer.ItemName2 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else if (!String.IsNullOrEmpty(customer.ItemName3) && customer.ItemName3 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else if (!String.IsNullOrEmpty(customer.ItemName4) && customer.ItemName4 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else if (!String.IsNullOrEmpty(customer.ItemName5) && customer.ItemName5 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else if (!String.IsNullOrEmpty(customer.ItemName6) && customer.ItemName6 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else if (!String.IsNullOrEmpty(customer.ItemName7) && customer.ItemName7 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else if (!String.IsNullOrEmpty(customer.ItemName8) && customer.ItemName8 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else if (!String.IsNullOrEmpty(customer.ItemName9) && customer.ItemName9 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else if (!String.IsNullOrEmpty(customer.ItemName10) && customer.ItemName10 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else if (!String.IsNullOrEmpty(customer.ItemName11) && customer.ItemName11 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else if (!String.IsNullOrEmpty(customer.ItemName12) && customer.ItemName12 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else if (!String.IsNullOrEmpty(customer.ItemName13) && customer.ItemName13 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else if (!String.IsNullOrEmpty(customer.ItemName14) && customer.ItemName14 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else if (!String.IsNullOrEmpty(customer.ItemName15) && customer.ItemName15 == RemoveItemList[i])
                    {
                        flag = true;
                        break;
                    }
                    else
                    {
                        flag = false;
                    }

                }
            }

            return flag;
        }
        #endregion

        #region Unit Conversion Functions

        public List<string> FindUnitTypeList(UnitConversion unitConversionList)
        {
            List<string> unitTypeList = new List<string>();
            unitTypeList.Add(unitConversionList.MeasurementType1);
            unitTypeList.Add(unitConversionList.MeasurementType2);
            unitTypeList.Add(unitConversionList.MeasurementType3);

            return unitTypeList;
        }

        public List<string> FindUnitValueList(UnitConversion unitConversionList)
        {
            List<string> unitValueList = new List<string>();
            unitValueList.Add(unitConversionList.Units1);
            unitValueList.Add(unitConversionList.Units2);
            unitValueList.Add(unitConversionList.Units3);

            return unitValueList;
        }

        [HttpPost]
        public ActionResult SaveUnitValues(string[] unitValueList)
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            UpdateUnitTypes(unitValueList, um);

            return Json(new { res = true }, JsonRequestBehavior.AllowGet);
        }

        public void UpdateUnitTypes(string[] unitValueList, UserModel um)
        {
            DataContext dc1 = new DataContext();
            var unitConversionList = dc1.unitConversionModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();
            var tableId = unitConversionList.TableID;
            try
            {

                var newUnitConversionValues = new UnitConversion()
                {
                    TableID = tableId,
                    Units1 = unitValueList[0],
                    Units2 = unitValueList[1],
                    Units3 = unitValueList[2]
                };

                using (DataContext db2 = new DataContext())
                {
                    db2.unitConversionModel.Attach(newUnitConversionValues);
                    db2.Entry(newUnitConversionValues).Property(x => x.Units1).IsModified = true;
                    db2.Entry(newUnitConversionValues).Property(x => x.Units2).IsModified = true;
                    db2.Entry(newUnitConversionValues).Property(x => x.Units3).IsModified = true;

                    db2.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        #region Common Functions

        #region Functions Related to "Trash Items" Table
        public List<string> FindItemList(TrashItems trashItemsList)
        {

            List<string> finalItemList = new List<string>();

            List<string> itemList = ItemListWithNullValues(trashItemsList);

            for (int i = 0; i < itemList.Count(); i++)
            {
                if (itemList[i] != null && itemList[i] != "")
                {
                    finalItemList.Add(itemList[i]);
                }
            }

            return finalItemList;
        }

        private List<string> ItemListWithNullValues(TrashItems trashItemsList)
        {
            List<string> itemList = new List<string>();
            itemList.Add(trashItemsList.ItemName1);
            itemList.Add(trashItemsList.ItemName2);
            itemList.Add(trashItemsList.ItemName3);
            itemList.Add(trashItemsList.ItemName4);
            itemList.Add(trashItemsList.ItemName5);
            itemList.Add(trashItemsList.ItemName6);
            itemList.Add(trashItemsList.ItemName7);
            itemList.Add(trashItemsList.ItemName8);
            itemList.Add(trashItemsList.ItemName9);
            itemList.Add(trashItemsList.ItemName10);
            itemList.Add(trashItemsList.ItemName11);
            itemList.Add(trashItemsList.ItemName12);
            itemList.Add(trashItemsList.ItemName13);
            itemList.Add(trashItemsList.ItemName14);
            itemList.Add(trashItemsList.ItemName15);
            return itemList;
        }

        private void UpdateTrashItemTable(string itemName, UserModel um)
        {
            //update trash items table
            try
            {
                var trashItemsList = db.trashItemModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();
                var itemTableId = trashItemsList.TableID;

                List<string> finalItemLNameist = FindItemList(trashItemsList);

                finalItemLNameist.Add(itemName);

                for (int i = finalItemLNameist.Count() - 1; i < 15; i++)
                {
                    finalItemLNameist.Add(null);
                }

                ItemTableUpdateItemNames(finalItemLNameist.ToArray(), um);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void ItemTableUpdateItemNames(string[] itemNameList, UserModel um)
        {
            var trashItemNamesList = db.trashItemModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();
            var tableId = trashItemNamesList.TableID;

            var trashItem = new TrashItems()
            {
                TableID = tableId,
                ItemName1 = itemNameList[0],
                ItemName2 = itemNameList[1],
                ItemName3 = itemNameList[2],
                ItemName4 = itemNameList[3],
                ItemName5 = itemNameList[4],
                ItemName6 = itemNameList[5],
                ItemName7 = itemNameList[6],
                ItemName8 = itemNameList[7],
                ItemName9 = itemNameList[8],
                ItemName10 = itemNameList[9],
                ItemName11 = itemNameList[10],
                ItemName12 = itemNameList[11],
                ItemName13 = itemNameList[12],
                ItemName14 = itemNameList[13],
                ItemName15 = itemNameList[14]
            };

            using (DataContext db = new DataContext())
            {
                db.trashItemModel.Attach(trashItem);
                db.Entry(trashItem).Property(x => x.ItemName1).IsModified = true;
                db.Entry(trashItem).Property(x => x.ItemName2).IsModified = true;
                db.Entry(trashItem).Property(x => x.ItemName3).IsModified = true;
                db.Entry(trashItem).Property(x => x.ItemName4).IsModified = true;
                db.Entry(trashItem).Property(x => x.ItemName5).IsModified = true;
                db.Entry(trashItem).Property(x => x.ItemName6).IsModified = true;
                db.Entry(trashItem).Property(x => x.ItemName7).IsModified = true;
                db.Entry(trashItem).Property(x => x.ItemName8).IsModified = true;
                db.Entry(trashItem).Property(x => x.ItemName9).IsModified = true;
                db.Entry(trashItem).Property(x => x.ItemName10).IsModified = true;
                db.Entry(trashItem).Property(x => x.ItemName11).IsModified = true;
                db.Entry(trashItem).Property(x => x.ItemName12).IsModified = true;
                db.Entry(trashItem).Property(x => x.ItemName13).IsModified = true;
                db.Entry(trashItem).Property(x => x.ItemName14).IsModified = true;
                db.Entry(trashItem).Property(x => x.ItemName15).IsModified = true;
                db.SaveChanges();
            }
        }

        #endregion

        #region Functions Related to "TrashItem_Measurements" Table

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

        public List<string> FindTrashTypeList(TrashItemMeasurements trashItemsMeasurementList)
        {

            List<string> finalTypeList = new List<string>();

            List<string> typeList = TypeListWithNullValues(trashItemsMeasurementList);

            for (int i = 0; i < typeList.Count(); i++)
            {
                if (typeList[i] != null && typeList[i] != "")
                {
                    finalTypeList.Add(typeList[i]);
                }
            }

            return finalTypeList;
        }

        public List<string> TypeListWithNullValues(TrashItemMeasurements trashItemsMeasurementList)
        {
            List<string> typeList = new List<string>();
            typeList.Add(trashItemsMeasurementList.ItemType1);
            typeList.Add(trashItemsMeasurementList.ItemType2);
            typeList.Add(trashItemsMeasurementList.ItemType3);
            typeList.Add(trashItemsMeasurementList.ItemType4);
            typeList.Add(trashItemsMeasurementList.ItemType5);
            typeList.Add(trashItemsMeasurementList.ItemType6);
            typeList.Add(trashItemsMeasurementList.ItemType7);
            typeList.Add(trashItemsMeasurementList.ItemType8);
            typeList.Add(trashItemsMeasurementList.ItemType9);
            typeList.Add(trashItemsMeasurementList.ItemType10);
            typeList.Add(trashItemsMeasurementList.ItemType11);
            typeList.Add(trashItemsMeasurementList.ItemType12);
            typeList.Add(trashItemsMeasurementList.ItemType13);
            typeList.Add(trashItemsMeasurementList.ItemType14);
            typeList.Add(trashItemsMeasurementList.ItemType15);
            return typeList;
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

        public void UpdateMeasurementTypes(string[] measurementTypeList, UserModel um)
        {
            var trashItemsMeasurementList = db.trashMeasurementModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();
            var tableId = trashItemsMeasurementList.TableID;

            var trashItmMeasurement = new TrashItemMeasurements()
            {
                TableID = tableId,
                ItemType1 = measurementTypeList[0],
                ItemType2 = measurementTypeList[1],
                ItemType3 = measurementTypeList[2],
                ItemType4 = measurementTypeList[3],
                ItemType5 = measurementTypeList[4],
                ItemType6 = measurementTypeList[5],
                ItemType7 = measurementTypeList[6],
                ItemType8 = measurementTypeList[7],
                ItemType9 = measurementTypeList[8],
                ItemType10 = measurementTypeList[9],
                ItemType11 = measurementTypeList[10],
                ItemType12 = measurementTypeList[11],
                ItemType13 = measurementTypeList[12],
                ItemType14 = measurementTypeList[13],
                ItemType15 = measurementTypeList[14]
            };

            using (DataContext db = new DataContext())
            {
                db.trashMeasurementModel.Attach(trashItmMeasurement);
                db.Entry(trashItmMeasurement).Property(x => x.ItemType1).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemType2).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemType3).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemType4).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemType5).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemType6).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemType7).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemType8).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemType9).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemType10).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemType11).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemType12).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemType13).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemType14).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemType15).IsModified = true;
                db.SaveChanges();
            }
        }

        public void UpdateItemNames(string[] itemNameList, UserModel um)
        {
            var trashItemsMeasurementList = db.trashMeasurementModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();
            var tableId = trashItemsMeasurementList.TableID;

            var trashItmMeasurement = new TrashItemMeasurements()
            {
                TableID = tableId,
                ItemName1 = itemNameList[0],
                ItemName2 = itemNameList[1],
                ItemName3 = itemNameList[2],
                ItemName4 = itemNameList[3],
                ItemName5 = itemNameList[4],
                ItemName6 = itemNameList[5],
                ItemName7 = itemNameList[6],
                ItemName8 = itemNameList[7],
                ItemName9 = itemNameList[8],
                ItemName10 = itemNameList[9],
                ItemName11 = itemNameList[10],
                ItemName12 = itemNameList[11],
                ItemName13 = itemNameList[12],
                ItemName14 = itemNameList[13],
                ItemName15 = itemNameList[14]
            };

            using (DataContext db = new DataContext())
            {
                db.trashMeasurementModel.Attach(trashItmMeasurement);
                db.Entry(trashItmMeasurement).Property(x => x.ItemName1).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemName2).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemName3).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemName4).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemName5).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemName6).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemName7).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemName8).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemName9).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemName10).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemName11).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemName12).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemName13).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemName14).IsModified = true;
                db.Entry(trashItmMeasurement).Property(x => x.ItemName15).IsModified = true;
                db.SaveChanges();
            }
        }

        public void UpdateItemMeasurementTable(string itemName, string itemType, UserModel um)
        {
            var trashItemsMeasurementList = db.trashMeasurementModel.Where(c => c.CompanyName == um.CompanyName).FirstOrDefault();
            var tableId = trashItemsMeasurementList.TableID;

            //update trash measurement table 
            List<string> finalItemList = FindTrashItemList(trashItemsMeasurementList);
            List<string> finalTypeList = FindTrashTypeList(trashItemsMeasurementList);

            finalItemList.Add(itemName);
            finalTypeList.Add(itemType);

            for (int i = finalItemList.Count() - 1; i < 15; i++)
            {
                finalItemList.Add(null);
            }

            for (int i = finalTypeList.Count() - 1; i < 15; i++)
            {
                finalTypeList.Add(null);
            }

            UpdateItemNames(finalItemList.ToArray(), um);
            UpdateMeasurementTypes(finalTypeList.ToArray(), um);
        }

        #endregion

        #endregion
    }


}