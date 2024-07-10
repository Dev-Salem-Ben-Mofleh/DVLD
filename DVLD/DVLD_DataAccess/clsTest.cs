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
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace DVLD_DataAccess
{
    public class clsTestData
    {

        public static bool GetTestInfoByID(int TestID, 
            ref int TestAppointmentID,ref bool TestResult, 
            ref string Notes , ref int CreatedByUserID )
            {
                bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_GetTestByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TestID", TestID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                TestAppointmentID = (int)reader["TestAppointmentID"];
                                TestResult = (bool)reader["TestResult"];
                                if (reader["Notes"] == DBNull.Value)

                                    Notes = "";
                                else
                                    Notes = (string)reader["Notes"];

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


        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass
            (int PersonID,int LicenseClassID,int TestTypeID, ref int TestID,
              ref int TestAppointmentID, ref bool TestResult,
              ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_GetLastTestByPersonAndTestTypeAndLicenseClass", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;
                                TestID = (int)reader["TestID"];
                                TestAppointmentID = (int)reader["TestAppointmentID"];
                                TestResult = (bool)reader["TestResult"];
                                if (reader["Notes"] == DBNull.Value)

                                    Notes = "";
                                else
                                    Notes = (string)reader["Notes"];

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


        public static DataTable GetAllTests()
            {

                DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_GetAllTests", connection))
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

        public static int AddNewTest( int TestAppointmentID,  bool TestResult,
             string Notes,  int CreatedByUserID)
        {
            int TestID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    

                    using (SqlCommand command = new SqlCommand("SP_AddNewTest", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                        command.Parameters.AddWithValue("@TestResult", TestResult);

                        if (Notes != "" && Notes != null)
                            command.Parameters.AddWithValue("@Notes", Notes);
                        else
                            command.Parameters.AddWithValue("@Notes", System.DBNull.Value);



                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        SqlParameter outputIdParam = new SqlParameter("@TestID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputIdParam);

                        command.ExecuteNonQuery();

                        TestID = (int)command.Parameters["@TestID"].Value;

                    }
                }
            }

            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);

            }


            return TestID;

        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult,
             string Notes, int CreatedByUserID)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("SP_UpdateNewTest", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TestID", TestID);
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                        command.Parameters.AddWithValue("@TestResult", TestResult);
                        command.Parameters.AddWithValue("@Notes", Notes);
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

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestCount = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("dbo.GetPassedTestCount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                        SqlParameter returnParameter = new SqlParameter(@"ReturnVal", SqlDbType.TinyInt)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(returnParameter);

                        command.ExecuteNonQuery();

                        PassedTestCount = Convert.ToByte(returnParameter.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);

            }


            return PassedTestCount;



        }



    }
}
