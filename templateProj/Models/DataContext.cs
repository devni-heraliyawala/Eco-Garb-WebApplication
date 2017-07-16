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

        public DbSet<drivers> drivers { get; set; }
        public DbSet<vehicles> vehicles { get; set; }

    }
} 