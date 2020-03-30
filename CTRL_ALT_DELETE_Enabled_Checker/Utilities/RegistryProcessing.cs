
using System;



namespace CTRL_ALT_DELETE_Enabled_Checker.Utilities
{
    /// <summary>
    /// Class for executing commands which make operations with registry 
    /// </summary>
    public class RegistryProcessing : ICommandInterface
    {
        private RegistryAccess registryAccess;
        private Log logger;
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryProcessing"/> class (Constructor)
        /// </summary>
        public RegistryProcessing(RegistryAccess registryAccess, Log logger) 
        { 
            this.registryAccess = registryAccess;
            this.logger = logger;
        }

        /// <summary>
        /// Executes commands which will be defined in users class
        /// </summary>
        public void Execute()
        {
            string s_disable_param = registryAccess.GetRegisterValue();
            if (s_disable_param.Trim() == "NOT_OPENED")
            {
                logger.LogMessageViaEventLog(registryAccess.BaseRegistryKey + @"\" + registryAccess.SubKey + @"\" + registryAccess.SKeyName + @" = " + s_disable_param);
                return;
            }

            if (s_disable_param.Trim() == "ERROR")
            {
                logger.LogMessageViaEventLog(registryAccess.BaseRegistryKey + @"\" + registryAccess.SubKey + @"\" + registryAccess.SKeyName + @" = " + s_disable_param);
                return;
            }

            if (Convert.ToInt32(s_disable_param) == 0)
            {
                registryAccess.SetRegisterValue(1);
                logger.LogMessageViaEventLog(registryAccess.BaseRegistryKey + @"\" + registryAccess.SubKey + @"\" + registryAccess.SKeyName + @" = 1; Enabled;");
            } 
        }

        /// <summary>
        /// Undoes commands which will be defined in users class and reverts all changes that was made
        /// </summary>
        public void Undo()
        {
            
        }

    }
}
