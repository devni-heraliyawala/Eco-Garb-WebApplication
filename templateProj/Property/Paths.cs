using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace templateProj.Property
{
    public class Paths
    {
        //private string path = Server.MapPath("~\"Property");
        private static string path = HttpContext.Current.Server.MapPath("~/Property/Paths.txt");
        private StreamReader File
            = new StreamReader(path);

        public IDictionary<string, string> dict 
            = new Dictionary<string, string>();

        private string line = "";

        public Paths()
        {
            ReadPaths();
        }
        private void ReadPaths()
        {
        string[] arr = new string[2];

            while ((line = File.ReadLine()) != "#[EoF]")
            {
                if (line != "")
                {
                    if (!line.StartsWith("#"))
                    {
                        arr = line.Split(',')[0].Split('=');

                        dict[arr[0]] = arr[1];
                    }
                }
            }
        }


    }
}