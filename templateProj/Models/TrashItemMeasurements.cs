using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    [Table("TrashItem_Measurements")]
    public class TrashItemMeasurements
    {
        [Key]
        public int TableID { get; set; }
        public string CompanyName { get; set; }

        public string ItemName1 { get; set; }
        public string ItemName2 { get; set; }
        public string ItemName3 { get; set; }
        public string ItemName4 { get; set; }
        public string ItemName5 { get; set; }
        public string ItemName6 { get; set; }
        public string ItemName7 { get; set; }
        public string ItemName8 { get; set; }
        public string ItemName9 { get; set; }
        public string ItemName10 { get; set; }


        public string ItemType1 { get; set; }
        public string ItemType2 { get; set; }
        public string ItemType3 { get; set; }
        public string ItemType4 { get; set; }
        public string ItemType5 { get; set; }
        public string ItemType6 { get; set; }
        public string ItemType7 { get; set; }
        public string ItemType8 { get; set; }
        public string ItemType9 { get; set; }
        public string ItemType10 { get; set; }

        public string ItemUnitPrice1 { get; set; }
        public string ItemUnitPrice2 { get; set; }
        public string ItemUnitPrice3 { get; set; }
        public string ItemUnitPrice4 { get; set; }
        public string ItemUnitPrice5 { get; set; }
        public string ItemUnitPrice6 { get; set; }
        public string ItemUnitPrice7 { get; set; }
        public string ItemUnitPrice8 { get; set; }
        public string ItemUnitPrice9 { get; set; }
        public string ItemUnitPrice10 { get; set; }
    }
}