using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess
{
    public class clsApplicationData
    {
      

        public static bool GetApplicationInfoByID(int ApplicationID, 
            ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID, 
            ref byte ApplicationStatus,ref DateTime LastStatusDate,
            ref float PaidFees, ref int CreatedByUserID)
            {
                bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_GetApplicationsByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                ApplicantPersonID = (int)reader["ApplicantPersonID"];
                                ApplicationDate = (DateTime)reader["ApplicationDate"];
                                ApplicationTypeID = (int)reader["ApplicationTypeID"];
                                ApplicationStatus = (byte)reader["ApplicationStatus"];
                                LastStatusDate = (DateTime)reader["LastStatusDate"];
                                PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                CreatedByUserID = (int)reader["CreatedByUserID"];


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

        public static DataTable GetAllApplications()
            {

                DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_GetAllApplications", connection))
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

        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
             byte ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID)
        {

            //this function will return the new person id if succeeded and -1 if not.
            int ApplicationID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("SP_AddNewApplications", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("ApplicantPersonID", @ApplicantPersonID);
                        command.Parameters.AddWithValue("ApplicationDate", @ApplicationDate);
                        command.Parameters.AddWithValue("ApplicationTypeID", @ApplicationTypeID);
                        command.Parameters.AddWithValue("ApplicationStatus", @ApplicationStatus);
                        command.Parameters.AddWithValue("LastStatusDate", @LastStatusDate);
                        command.Parameters.AddWithValue("PaidFees", @PaidFees);
                        command.Parameters.AddWithValue("CreatedByUserID", @CreatedByUserID);



                        SqlParameter outputIdParam = new SqlParameter("@ApplicationID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputIdParam);

                        command.ExecuteNonQuery();

                        ApplicationID = (int)command.Parameters["@ApplicationID"].Value;

                    
                }
                }
            }
            

            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);

            }

            


            return ApplicationID;
        }


        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
             byte ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_UpdateApplications", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("ApplicantPersonID", @ApplicantPersonID);
                        command.Parameters.AddWithValue("ApplicationDate", @ApplicationDate);
                        command.Parameters.AddWithValue("ApplicationTypeID", @ApplicationTypeID);
                        command.Parameters.AddWithValue("ApplicationStatus", @ApplicationStatus);
                        command.Parameters.AddWithValue("LastStatusDate", @LastStatusDate);
                        command.Parameters.AddWithValue("PaidFees", @PaidFees);
                        command.Parameters.AddWithValue("CreatedByUserID", @CreatedByUserID);



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

        public static bool DeleteApplication(int ApplicationID)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_DeleteApplications", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);



                        rowsAffected = command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
            }
            

            return (rowsAffected > 0);

        }

        public static bool IsApplicationExist(int ApplicationID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("SP_CheckApplicationExists", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);


                        SqlParameter returnParameter = new SqlParameter(@"ReturnVal", SqlDbType.TinyInt)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(returnParameter);

                        command.ExecuteNonQuery();

                        isFound = ((int)returnParameter.Value == 1);

                    
                }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            

            return isFound;
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
           
           //incase the ActiveApplication ID !=-1 return true.
            return (GetActiveApplicationID(PersonID, ApplicationTypeID) !=-1);
        }

        public static int GetActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            int ActiveApplicationID =-1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    
                    using (SqlCommand command = new SqlCommand("SP_GetActiveApplicationID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                        SqlParameter outputIdParam = new SqlParameter("@ActiveApplicationID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputIdParam);

                        command.ExecuteNonQuery();

                        PersonID = (int)command.Parameters["@ActiveApplicationID"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return ActiveApplicationID;
            }
            

            return ActiveApplicationID;
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, int ApplicationTypeID,int LicenseClassID)
        {
            int ActiveApplicationID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("GetActiveApplicationIDForLicenseClass", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                        SqlParameter returnParameter = new SqlParameter(@"ReturnVal", SqlDbType.TinyInt)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(returnParameter);

                        command.ExecuteNonQuery();

                        ActiveApplicationID = (int)returnParameter.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
                return ActiveApplicationID;
            }


            return ActiveApplicationID;
        }
      
        public static bool UpdateStatus(int ApplicationID, short NewStatus)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"Update  Applications  
                            set 
                                ApplicationStatus = @NewStatus, 
                                LastStatusDate = @LastStatusDate
                            where ApplicationID=@ApplicationID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@NewStatus", NewStatus);
                        command.Parameters.AddWithValue("LastStatusDate", DateTime.Now);


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
