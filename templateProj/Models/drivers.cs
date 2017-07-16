using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    [Table("drivers")]
    public class drivers
    {
        [Key]
        public int driverID { get; set; }
        public string NicID { get; set; }

        public string DrivingLicenseID { get; set; }
        
        public string driverName { get; set; }
        public int driverAge { get; set; }
        public int contactNo { get; set; }
        public string driverStatus { get; set; }
        public int yearsOfExperience { get; set; }
        public int workingHrsPerDay { get; set; }
        public int LeftWorkingHrs { get; set; }
    }
}