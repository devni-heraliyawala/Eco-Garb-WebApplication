using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    [Table("vehicles")]
    public class vehicles
    {
        [Key]
        public int vehicleID { get; set; }
        public string vehicleNo { get; set; }

        public double capacity { get; set; }
        public string Make { get; set; }
        public DateTime AddedDate { get; set; }
        public string TrashTypeCarried { get; set; }
        public string vehicleStatus { get; set; }
    }
}