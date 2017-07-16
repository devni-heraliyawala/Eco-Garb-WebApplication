using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    public class HomeViewModel
    {
        public UserModel usermodel { get; set; }

        public TrashItems trashItemModel { get; set; }

        public MeasurementTypes measurementTypesModel { get; set; }

        public TrashItemMeasurements trashMeasurementModel { get; set; }

        public UnitConversion unitConversionModel { get; set; }

        public List<drivers> drivers { get; set; }
        public List<vehicles> vehicles { get; set; }
    }
}