using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    [Table("TrashItems")]
    public class TrashItems
    {
        [Key]
        public int TableID { get; set; }
        public string CompanyName { get; set; }

        public string ItemName1 { get; set; }
        public string ItemName2 { get; set; }
        public string ItemName3 { get; set; }
        public string ItemName4 { get; set; }
        public string ItemName5 { get; set; }
        public string ItemName6 { get; set; }
        public string ItemName7 { get; set; }
        public string ItemName8 { get; set; }
        public string ItemName9 { get; set; }
        public string ItemName10 { get; set; }
        public string ItemName11 { get; set; }
        public string ItemName12 { get; set; }
        public string ItemName13 { get; set; }
        public string ItemName14 { get; set; }
        public string ItemName15 { get; set; }

    }
}