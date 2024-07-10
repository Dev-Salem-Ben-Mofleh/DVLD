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
    public class clsDetainedLicenseData
    {

        public static bool GetDetainedLicenseInfoByID(int DetainID, 
            ref int LicenseID, ref DateTime DetainDate,
            ref float FineFees,ref int CreatedByUserID, 
            ref bool IsReleased, ref DateTime ReleaseDate, 
            ref int ReleasedByUserID,ref int ReleaseApplicationID)
            {
                bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_GetDetainedLicenseByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@DetainID", DetainID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                LicenseID = (int)reader["LicenseID"];
                                DetainDate = (DateTime)reader["DetainDate"];
                                FineFees = Convert.ToSingle(reader["FineFees"]);
                                CreatedByUserID = (int)reader["CreatedByUserID"];

                                IsReleased = (bool)reader["IsReleased"];

                                if (reader["ReleaseDate"] == DBNull.Value)

                                    ReleaseDate = DateTime.MaxValue;
                                else
                                    ReleaseDate = (DateTime)reader["ReleaseDate"];


                                if (reader["ReleasedByUserID"] == DBNull.Value)

                                    ReleasedByUserID = -1;
                                else
                                    ReleasedByUserID = (int)reader["ReleasedByUserID"];

                                if (reader["ReleaseApplicationID"] == DBNull.Value)

                                    ReleaseApplicationID = -1;
                                else
                                    ReleaseApplicationID = (int)reader["ReleaseApplicationID"];

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

        
        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID,
         ref int DetainID, ref DateTime DetainDate,
         ref float FineFees, ref int CreatedByUserID,
         ref bool IsReleased, ref DateTime ReleaseDate,
         ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_GetDetainedLicenseByLicenseID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@LicenseID", LicenseID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                DetainID = (int)reader["DetainID"];
                                DetainDate = (DateTime)reader["DetainDate"];
                                FineFees = Convert.ToSingle(reader["FineFees"]);
                                CreatedByUserID = (int)reader["CreatedByUserID"];

                                IsReleased = (bool)reader["IsReleased"];

                                if (reader["ReleaseDate"] == DBNull.Value)

                                    ReleaseDate = DateTime.MaxValue;
                                else
                                    ReleaseDate = (DateTime)reader["ReleaseDate"];


                                if (reader["ReleasedByUserID"] == DBNull.Value)

                                    ReleasedByUserID = -1;
                                else
                                    ReleasedByUserID = (int)reader["ReleasedByUserID"];

                                if (reader["ReleaseApplicationID"] == DBNull.Value)

                                    ReleaseApplicationID = -1;
                                else
                                    ReleaseApplicationID = (int)reader["ReleaseApplicationID"];

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

        public static DataTable GetAllDetainedLicenses()
            {

                DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_GetAllDetained", connection))
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
        
        public static int AddNewDetainedLicense(
            int LicenseID,  DateTime DetainDate,
            float FineFees,  int CreatedByUserID)
        {
            int DetainID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("SP_AddNewDetainedLecinse", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        command.Parameters.AddWithValue("@DetainDate", DetainDate);
                        command.Parameters.AddWithValue("@FineFees", FineFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        command.Parameters.AddWithValue("@IsReleased", 0);




                        SqlParameter outputIdParam = new SqlParameter("@DetainID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputIdParam);

                        command.ExecuteNonQuery();

                        DetainID = (int)command.Parameters["@DetainID"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);

            }



            return DetainID;

        }

        public static bool UpdateDetainedLicense(int DetainID, 
            int LicenseID, DateTime DetainDate,
            float FineFees, int CreatedByUserID)
        {

            int rowsAffected = 0;
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_UpdateDetained", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@DetainedLicenseID", DetainID);
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        command.Parameters.AddWithValue("@DetainDate", DetainDate);
                        command.Parameters.AddWithValue("@FineFees", FineFees);
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


        public static bool ReleaseDetainedLicense(int DetainID,
                 int ReleasedByUserID, int ReleaseApplicationID)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_UpdateDetainedReleased", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@DetainID", DetainID);
                        command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
                        command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
                        command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);

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

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("SP_CheckIsDetained", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@LicenseID", LicenseID);


                        SqlParameter returnParameter = new SqlParameter(@"ReturnVal", SqlDbType.TinyInt)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(returnParameter);

                        command.ExecuteNonQuery();

                        IsDetained = ((int)returnParameter.Value == 1);

                }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);

            }

            



            return IsDetained;
            ;

        }

    }
}
