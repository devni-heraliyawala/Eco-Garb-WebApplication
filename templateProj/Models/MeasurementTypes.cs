using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    [Table("MeasurementTypes")]
    public class MeasurementTypes
    {
        [Key]
        public int TableID { get; set; }

        public string MeasurementType1 { get; set; }

        public string MeasurementType2 { get; set; }

        public string MeasurementType3 { get; set; }


    }
}