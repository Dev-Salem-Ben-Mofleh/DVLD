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
    public class clsApplicationTypeData
    {

        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID, 
            ref string ApplicationTypeTitle, ref float ApplicationFees)
            {
                bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_GetApplicationTypesByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                                ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);





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

        public static DataTable GetAllApplicationTypes()
            {

                DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();



                    using (SqlCommand command = new SqlCommand("sp_GetAllApplicationTypes", connection))
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

        public static int AddNewApplicationType( string Title, float Fees)
        {
            int ApplicationTypeID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
              
                            

                    using (SqlCommand command = new SqlCommand("SP_AddNewApplicationType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);
                        command.Parameters.AddWithValue("@ApplicationFees", Fees);



                        SqlParameter outputIdParam = new SqlParameter("@ApplicationTypeID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputIdParam);

                        command.ExecuteNonQuery();

                        ApplicationTypeID = (int)command.Parameters["@ApplicationTypeID"].Value;

                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);

            }

            


            return ApplicationTypeID;

        }

        public static bool UpdateApplicationType(int ApplicationTypeID,string Title, float Fees)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();



                    using (SqlCommand command = new SqlCommand("sp_UpdateApplicationType", connection))
                    {

                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        command.Parameters.AddWithValue("@Title", Title);
                        command.Parameters.AddWithValue("@Fees", Fees);

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
