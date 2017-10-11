using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    [Table("RecCompany_Master")]
    public class RecCompany
    {
        [Key]
        public string RecCompanyName { get; set; }

        public string Address { get; set; }

        public string Lat { get; set; }

        public string Lng { get; set; }
    }
}