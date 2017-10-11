using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace templateProj.Models
{
    [Table("Vehicles")]
    public class Vehicles
    {
        [Key]
        public int TableID { get; set; }
        public string VehicleNo { get; set; }

        public double Capacity { get; set; }
        public string Make { get; set; }
        public DateTime AddedDate { get; set; }
        public string VehicleStatus { get; set; }
        public string FuelLevel { get; set; }
        public string FuelConsumption { get; set; }
        public string DistanceToTravel { get; set; }
    }
}