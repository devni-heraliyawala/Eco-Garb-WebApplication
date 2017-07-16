using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    [Table("UnitConversion")]
    public class UnitConversion
    {
        [Key]
        public int TableID { get; set; }
        public string CompanyName { get; set; }
        public string MeasurementType1 { get; set; }
        public string MeasurementType2 { get; set; }
        public string MeasurementType3 { get; set; }
        public string Units1 { get; set; }
        public string Units2 { get; set; }
        public string Units3 { get; set; }
    }
}