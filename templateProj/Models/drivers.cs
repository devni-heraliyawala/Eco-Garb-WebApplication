using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace templateProj.Models
{
    [Table("Drivers")]
    public class Drivers
    {
        
            [Key]
            public int TableID { get; set; }
            public string NicID { get; set; }

            public string DrivingLicenseID { get; set; }

            public string DriverName { get; set; }
            public int DriverAge { get; set; }
            public int ContactNo { get; set; }
            public string DriverStatus { get; set; }
            public int YearsOfExperience { get; set; }
            public int WorkingHrsPerDay { get; set; }
            public int LeftWorkingHrsPerDay { get; set; }
        
    }
}