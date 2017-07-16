using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    public class FormHandlingLayer
    {
        public void EditDriverInformation(drivers driver)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBcon"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE drivers SET driverName=@driverName,NicID=@NicID, DrivingLicenseID=@DrivingLicenseID,driverAge=@driverAge,contactNo=@contactNo,yearsOfExperience=@yearsOfExperience WHERE driverID=@driverID";
                SqlCommand cmd = new SqlCommand(query, con);

                SqlParameter paramdriverName = new SqlParameter();
                paramdriverName.ParameterName = "@driverName";
                paramdriverName.Value = driver.driverName;
                cmd.Parameters.Add(paramdriverName);

                SqlParameter paramNicID = new SqlParameter();
                paramNicID.ParameterName = "@NicID";
                paramNicID.Value = driver.NicID;
                cmd.Parameters.Add(paramNicID);

                SqlParameter paramDrivingLicenseID = new SqlParameter();
                paramDrivingLicenseID.ParameterName = "@DrivingLicenseID";
                paramDrivingLicenseID.Value = driver.DrivingLicenseID;
                cmd.Parameters.Add(paramDrivingLicenseID);

                SqlParameter paramdriverAge = new SqlParameter();
                paramdriverAge.ParameterName = "@driverAge";
                paramdriverAge.Value = driver.driverAge;
                cmd.Parameters.Add(paramdriverAge);

                SqlParameter paramcontactNo = new SqlParameter();
                paramcontactNo.ParameterName = "@contactNo";
                paramcontactNo.Value = driver.contactNo;
                cmd.Parameters.Add(paramcontactNo);

                SqlParameter paramyearsOfExperience = new SqlParameter();
                paramyearsOfExperience.ParameterName = "@yearsOfExperience";
                paramyearsOfExperience.Value = driver.yearsOfExperience;
                cmd.Parameters.Add(paramyearsOfExperience);

                SqlParameter paramdriverID = new SqlParameter();
                paramdriverID.ParameterName = "@driverID";
                paramdriverID.Value = driver.driverID;
                cmd.Parameters.Add(paramdriverID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteDriver(drivers driver)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBcon"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE drivers  WHERE driverID=@driverID";
                SqlCommand cmd = new SqlCommand(query, con);


                SqlParameter paramdriverID = new SqlParameter();
                paramdriverID.ParameterName = "@driverID";
                paramdriverID.Value = driver.driverID;
                cmd.Parameters.Add(paramdriverID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddNewVehicle(vehicles vehicle)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBcon"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT vehicles(vehicleNo,capacity,Make,AddedDate,TrashTypeCarried,vehicleStatus)Values(@vehicleNo,@capacity,@Make,@AddedDate,@TrashTypeCarried,@vehicleStatus)";
                SqlCommand cmd = new SqlCommand(query, con);

                SqlParameter paramvehicleNo = new SqlParameter();
                paramvehicleNo.ParameterName = "@vehicleNo";
                paramvehicleNo.Value = vehicle.vehicleNo;
                cmd.Parameters.Add(paramvehicleNo);

                SqlParameter paramcapacity = new SqlParameter();
                paramcapacity.ParameterName = "@capacity";
                paramcapacity.Value = vehicle.capacity;
                cmd.Parameters.Add(paramcapacity);

                SqlParameter paramMake = new SqlParameter();
                paramMake.ParameterName = "@Make";
                paramMake.Value = vehicle.Make;
                cmd.Parameters.Add(paramMake);

                SqlParameter paramAddedDate = new SqlParameter();
                paramAddedDate.ParameterName = "@AddedDate";
                paramAddedDate.Value = vehicle.AddedDate;
                cmd.Parameters.Add(paramAddedDate);

                SqlParameter paramTrashTypeCarried = new SqlParameter();
                paramTrashTypeCarried.ParameterName = "@TrashTypeCarried";
                paramTrashTypeCarried.Value = vehicle.TrashTypeCarried;
                cmd.Parameters.Add(paramTrashTypeCarried);

                SqlParameter paramvehicleStatus = new SqlParameter();
                paramvehicleStatus.ParameterName = "@vehicleStatus";
                paramvehicleStatus.Value = vehicle.vehicleStatus;
                cmd.Parameters.Add(paramvehicleStatus);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EditVehicleDetails(vehicles vehicle)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBcon"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE vehicles SET vehicleNo=@vehicleNo, capacity=@capacity,Make=@Make,TrashTypeCarried=@TrashTypeCarried  WHERE vehicleID=@vehicleID";
                SqlCommand cmd = new SqlCommand(query, con);

                SqlParameter paramvehicleNo = new SqlParameter();
                paramvehicleNo.ParameterName = "@vehicleNo";
                paramvehicleNo.Value = vehicle.vehicleNo;
                cmd.Parameters.Add(paramvehicleNo);

                SqlParameter paramcapacity = new SqlParameter();
                paramcapacity.ParameterName = "@capacity";
                paramcapacity.Value = vehicle.capacity;
                cmd.Parameters.Add(paramcapacity);

                SqlParameter paramMake = new SqlParameter();
                paramMake.ParameterName = "@Make";
                paramMake.Value = vehicle.Make;
                cmd.Parameters.Add(paramMake);

                SqlParameter paramTrashTypeCarried = new SqlParameter();
                paramTrashTypeCarried.ParameterName = "@TrashTypeCarried";
                paramTrashTypeCarried.Value = vehicle.TrashTypeCarried;
                cmd.Parameters.Add(paramTrashTypeCarried);

                SqlParameter paramvehicleID = new SqlParameter();
                paramvehicleID.ParameterName = "@vehicleID";
                paramvehicleID.Value = vehicle.vehicleID;
                cmd.Parameters.Add(paramvehicleID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteVehicle(vehicles vehicle)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBcon"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE vehicles WHERE vehicleID=@vehicleID";
                SqlCommand cmd = new SqlCommand(query, con);
                
                SqlParameter paramvehicleID = new SqlParameter();
                paramvehicleID.ParameterName = "@vehicleID";
                paramvehicleID.Value = vehicle.vehicleID;
                cmd.Parameters.Add(paramvehicleID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}