using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    [Table("Customer_Trash")]
    public class CustomerTrash
    {
        [Key]
        public int TableID { get; set; }

        public string Username { get; set; }

        public string CompanyName { get; set; }

       // public string CustomerName { get; set; }

        public string RequestedDate { get; set; }

        public string CollectedDate { get; set; }

        public string CollectingStatus { get; set; }

        public double TotalQty { get; set; }


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

        public string ItemName11 { get; set; }

        public string ItemName12 { get; set; }

        public string ItemName13 { get; set; }

        public string ItemName14 { get; set; }

        public string ItemName15 { get; set; }


        public int QtyItem1 { get; set; }

        public int QtyItem2 { get; set; }

        public int QtyItem3 { get; set; }

        public int QtyItem4 { get; set; }

        public int QtyItem5 { get; set; }

        public int QtyItem6 { get; set; }

        public int QtyItem7 { get; set; }

        public int QtyItem8 { get; set; }

        public int QtyItem9 { get; set; }

        public int QtyItem10 { get; set; }

        public int QtyItem11 { get; set; }

        public int QtyItem12 { get; set; }

        public int QtyItem13 { get; set; }

        public int QtyItem14 { get; set; }

        public int QtyItem15 { get; set; }





    }
}