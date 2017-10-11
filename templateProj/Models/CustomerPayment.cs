using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    [Table("Customer_Payment")]
    public class CustomerPayment
    {
        [Key]
        public int TableID { get; set; }

        public string Username { get; set; }

        public string CompanyName { get; set; }

        public string Branch { get; set; }

        public string RecyCompanyName { get; set; }

        public string PaymentDate { get; set; }

        public string PaymentStatus { get; set; }

        public double TotalAmount { get; set; }

    }
}