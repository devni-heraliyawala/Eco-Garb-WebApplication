using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    [Table("Customer_Main")]
    public class Customer
    {
        [Key]
        public int TableID { get; set; }

        public string Username { get; set; }

        public string Branch { get; set; }

        public string CompanyName { get; set; }

        public string RecyCompanyName { get; set; }

        public string CustomerName { get; set; }

        public string TrashStatus { get; set; }

        public string Address { get; set; }

        public string Lat { get; set; }

        public string Lng { get; set; }

        public string ContactNo { get; set; }

        public string Email { get; set; }

        public string Customer_Rate_Company { get; set; }

        public string Company_Rate_Customer { get; set; }

        public string Password { get; set; }

    }
}