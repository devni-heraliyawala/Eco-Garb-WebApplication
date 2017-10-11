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

        public List<Drivers> drivers { get; set; }
        public List<Vehicles> vehicles { get; set; }
        public List<Trips> trips { get; set; }

        public List<Customer> customerModel { get; set; }

        public List<CustomerTrash> customerTrashModel { get; set; }

        public List<RecCompany> recCompanyModel { get; set; }

        public List<CustomerPayment> custPaymentModel { get; set; }

    }
}