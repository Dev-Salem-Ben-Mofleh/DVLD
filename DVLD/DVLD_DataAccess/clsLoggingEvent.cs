using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace DVLD_DataAccess
{
    public class clsLoggingEvent
    {
        public static void LoogingEvent(string ErrorMessage)
        {
            string sourceName = "DVLDApp";

            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");
            }
            EventLog.WriteEntry(sourceName, ErrorMessage, EventLogEntryType.Error);

        }
    }
}
