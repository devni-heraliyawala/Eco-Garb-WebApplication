using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace templateProj.Models
{

    [Table("user")]
    public class UserModel
    {
        public int id { get; set; }

        public string CompanyName { get; set; }
        [Key]
        public string Username { get; set; }

        public string password { get; set; }

        public string salt { get; set; }

        public string Email { get; set; }

        public string ProfilePic { get; set; }

        public string FirstName { get; set; }

        public string DoB { get; set; }

        public string RecoverPasscode { get; set; }

        public string ContactNo { get; set; }

        public string Address { get; set; }

        public string Notes { get; set; }


    }
}