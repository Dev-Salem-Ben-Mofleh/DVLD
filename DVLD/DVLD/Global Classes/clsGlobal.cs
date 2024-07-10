using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Global_Classes;
using DVLD_Buisness;
using Microsoft.Win32;

namespace DVLD.Classes
{
    internal static  class clsGlobal
    {
        public static clsUser CurrentUser;

        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";

            string valueName = "LoginDVLD";
            string UsernamePassword = Username +','+ Password;


            if(Username=="")
            {
                string keyPathSelet = @"SOFTWARE\DVLD";

                try
                {
                    // Open the registry key in read/write mode with explicit registry view
                    using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                    {
                        using (RegistryKey key = baseKey.OpenSubKey(keyPathSelet, true))
                        {
                            if (key != null)
                            {
                                // Delete the specified value
                                key.DeleteValue(valueName);
                                return true;
                            }
                            else
                            {
                                return false;                           
                            }
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                  //  clsLoggingEvent.LoogingEvent("Error: " );
                }
                catch (Exception ex)
                {
                    clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
                }

            }

            try
            {
                // Write the value to the Registry
                Registry.SetValue(keyPath, valueName, UsernamePassword, RegistryValueKind.String);
                return true;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;

            }
        }

        public static bool GetStoredCredential(ref string Username, ref string Password)
        {

            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
            string valueName = "LoginDVLD";

            try
            {
                // Read the value from the Registry
                string value = Registry.GetValue(keyPath, valueName, null) as string;


                if (value != null)
                {
                    string line = value;
                    string[] result = line.Split(new string[] { "," }, StringSplitOptions.None);
                    Username = result[0];
                    Password = result[1];
                    return true;
                }
                else
                {
                    return false;

                }

            }
            catch (Exception ex)
            {
                clsLoggingEvent.LoogingEvent("Error: " + ex.Message);
                return false;

            }
        }
    }
}
