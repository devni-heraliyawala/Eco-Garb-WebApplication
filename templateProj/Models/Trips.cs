using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    [Table("Trips")]
    public class Trips
    {
        [Key]
        public int TableID { get; set; }

        public int RouteAddressID { get; set; }

        public string RouteLatList { get; set; }

        public string RouteLngList { get; set; }

        public string RouteDistance { get; set; }

        public string TripDate { get; set; }

        public string TripStatus { get; set; }

        public string AssignedDriverID { get; set; }

        public string AssignedDriverName { get; set; }

        public string AssignedVehicleID { get; set; }

        public string AssignedVehicleNo { get; set; }
        
    }
}