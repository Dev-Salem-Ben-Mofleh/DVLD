using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccess.clsCountryData;
using System.Net;
using System.Security.Policy;

namespace DVLD_DataAccess
{
    public class clsDriverData
    {

        public static bool GetDriverInfoByDriverID(int DriverID, 
            ref int PersonID,ref int CreatedByUserID,ref DateTime CreatedDate )
            {
                bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_GetDriverByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@DriverID", DriverID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                PersonID = (int)reader["PersonID"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];


                            }
                            else
                            {
                                // The record was not found
                                isFound = false;
                            }

                        }
                    }
                }

                }
                catch (Exception ex)
                {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
                isFound = false;
                }
               

                return isFound;
            }

        public static bool GetDriverInfoByPersonID(int PersonID,ref int DriverID,
            ref int CreatedByUserID,ref DateTime CreatedDate)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_GetDriverByPersonID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PersonID", PersonID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                DriverID = (int)reader["DriverID"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];

                            }
                            else
                            {
                                // The record was not found
                                isFound = false;
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
                isFound = false;
            }
            

            return isFound;
        }

        public static DataTable GetAllDrivers()
            {

                DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();



                    using (SqlCommand command = new SqlCommand("sp_GetAllDrivers", connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;


                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.HasRows)

                            {
                                dt.Load(reader);
                            }

                        }
                    }
                }

                }

                catch (Exception ex)
                {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
            }
            

                return dt;

            }

        public static int AddNewDriver( int PersonID, int CreatedByUserID)
        {
            int DriverID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                          

                    using (SqlCommand command = new SqlCommand("SP_AddNewDriver", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);


                        SqlParameter outputIdParam = new SqlParameter("@DriverID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputIdParam);

                        command.ExecuteNonQuery();

                        DriverID = (int)command.Parameters["@DriverID"].Value;

                    }
                }
            }

            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);

            }




            return DriverID;

        }

        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    //we dont update the createddate for the driver.


                    using (SqlCommand command = new SqlCommand("sp_UpdateDriver", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        rowsAffected = command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
                return false;
            }

            

            return (rowsAffected > 0);
        }

    }
}
