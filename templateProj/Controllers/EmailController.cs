using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Services.Description;
using templateProj.Models;
using templateProj.Property;

namespace templateProj.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {

        DataContext db = new DataContext();
        Paths path = new Paths();


        #region Send Recover Password (Email) Function

        //Send the generated passcode in an Email to the user
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SendEmail(string uname)
        {
            UserModel um;
            try
            {
                um = db.Umodel.Single(u => u.Username == uname);
            }
            catch (InvalidOperationException Ex)
            {
                um = null;
            }
            SendMailModel sm = new SendMailModel();

            if (um != null)
            {
                string pw = PassGen();

                var message = await EmailTemplate("RecoverPW");
                message
                    = message.Replace("@ViewBag.Name", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(um.Username));

                message
                    = message.Replace("@ViewBag.Pass", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pw));

                await MessageServices.SendEmail(um.Email, "Eco-Garb", message);
                @ViewBag.Error = "Nerror";
                @ViewBag.Uname = uname;

                sm.RecoverCode = pw;

                UpdateUserData(pw, uname);
            }
            else
            {
                @ViewBag.Error = "error";
            }

            return View("~/Views/Login/ForgotPassword.cshtml", sm);
        }

        // Send the email
        public static async Task<string> EmailTemplate(string template)
        {
            var templateFilePath
                = HostingEnvironment.MapPath("/Views/EmailTemplates/")
                    + template + ".cshtml";

            StreamReader objstreamreaderfile
                = new StreamReader(templateFilePath);

            var body = await objstreamreaderfile.ReadToEndAsync();

            objstreamreaderfile.Close();

            return body;
        }
        #endregion

        #region Update Recovery Code (database)

        private void UpdateUserData(string pw, string uname)
        {
            UserModel userModel = db.Umodel.Find(uname);
            userModel.RecoverPasscode = pw;

            db.Entry(userModel).State = EntityState.Modified;
            db.SaveChanges();
        }

        #endregion

        #region Generate Random Passcode
        //Random passcode generator
        public string PassGen()
        {
            string L = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string N = "0123456789";
            string pw = "";
            Random r = new Random();

            for (int i = 1; i <= 4; i++)
            {
                pw += L[r.Next(0, 25)];
                pw += N[r.Next(0, 9)];
            }
            return pw;
        }
        #endregion

    }
}