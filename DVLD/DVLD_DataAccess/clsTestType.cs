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
    public class clsTestTypeData
    {

        public static bool GetTestTypeInfoByID(int TestTypeID, 
            ref string TestTypeTitle, ref string TestDescription ,ref float TestFees)
            {
                bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_GetTestTypeByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                TestTypeTitle = (string)reader["TestTypeTitle"];
                                TestDescription = (string)reader["TestTypeDescription"];
                                TestFees = Convert.ToSingle(reader["TestTypeFees"]);

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

         public static DataTable GetAllTestTypes()
            {

                DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("sp_GetAllTestType", connection))
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

        public static int AddNewTestType( string Title,string Description, float Fees)
        {
            int TestTypeID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("SP_AddNewTestType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TestTypeTitle", Title);
                        command.Parameters.AddWithValue("@TestTypeDescription", Description);
                        command.Parameters.AddWithValue("@ApplicationFees", Fees);



                        SqlParameter outputIdParam = new SqlParameter("@PersonID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputIdParam);

                        command.ExecuteNonQuery();

                        TestTypeID = (int)command.Parameters["@PersonID"].Value;

                    }
                }
            }

            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);

            }



            return TestTypeID;

        }

        public static bool UpdateTestType(int TestTypeID,string Title,string Description, float Fees)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();



                    using (SqlCommand command = new SqlCommand("sp_UpdateTestType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        command.Parameters.AddWithValue("@TestTypeTitle", Title);
                        command.Parameters.AddWithValue("@TestTypeDescription", Description);
                        command.Parameters.AddWithValue("@TestTypeFees", Fees);


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
