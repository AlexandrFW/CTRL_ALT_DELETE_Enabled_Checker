using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTRL_ALT_DELETE_Enabled_Checker.Utilities
{
    /// <summary>
    /// Логирование данных 
    /// </summary>
    public class Log
    {
        EventLog _EventLog = new System.Diagnostics.EventLog();

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        public Log() { CreateEventLogger(); }

        private void CreateEventLogger()
        {
            _EventLog = new System.Diagnostics.EventLog();
            _EventLog.Source = "CTRL_ALT_DELETE_Enabled_Checker";
            _EventLog.Log = "Application";

            ((ISupportInitialize)(_EventLog)).BeginInit();
            if (!EventLog.SourceExists(_EventLog.Source))
            {
                EventLog.CreateEventSource(_EventLog.Source, _EventLog.Log);
            }
            ((ISupportInitialize)(_EventLog)).EndInit();
        }

        /// <summary>
        /// Logs the message. Create the log-file if it does not exist
        /// </summary>
        /// <param name="sMsg">The message which should logged</param>
        public void LogMessage(string sMsg)
        {
            if (File.Exists(Environment.CurrentDirectory + @"\applog.txt"))
                File.AppendAllText(Environment.CurrentDirectory + @"\applog.txt", sMsg);
            else
                File.Create(Environment.CurrentDirectory + @"\applog.txt");
        }

        /// <summary>
        /// Logs the message using Windows event log
        /// </summary>
        /// <param name="sMsg">The message which should logged</param>
        public void LogMessageViaEventLog(string sMsg)
        {
            _EventLog.WriteEntry(sMsg, EventLogEntryType.Information);
        }
    }
}
