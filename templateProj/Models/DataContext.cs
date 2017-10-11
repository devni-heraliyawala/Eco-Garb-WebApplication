using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DBcon")         
        {
        } 
 
        public DbSet<UserModel> Umodel { get; set; }

        public DbSet<TrashItems> trashItemModel { get; set; }
        
        public DbSet<MeasurementTypes> measurementTypesModel { get; set; }

        public DbSet<TrashItemMeasurements> trashMeasurementModel { get; set; }

        public DbSet<UnitConversion> unitConversionModel { get; set; }

        public DbSet<Drivers> drivers { get; set; }

        public DbSet<Vehicles> vehicles { get; set; }

        public DbSet<Trips> trips { get; set; }

        public DbSet<Customer> customerModel { get; set; }
        public DbSet<CustomerTrash> customerTrashModel { get; set; }

        public DbSet<RecCompany> recCompanyModel { get; set; }

        public DbSet<CustomerPayment> custPaymentModel { get; set; }
    }
} 