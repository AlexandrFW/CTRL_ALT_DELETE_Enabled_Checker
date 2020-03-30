namespace CTRL_ALT_DELETE_Enabled_Checker.Utilities
{
    /// <summary>
    /// Class for executing commands which make operations with registry 
    /// </summary>
    public class RegistryProcessingAutoLogon : ICommandInterface
    {
        private RegistryAccess registryAccess;

        /// <summary>
        /// The is registry overwrritten successfuly = TRUE
        /// </summary>
        public bool IsRegistryOverwrritten = false;
        private Log logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryProcessing"/> class (Constructor)
        /// </summary>
        public RegistryProcessingAutoLogon(RegistryAccess registryAccess, Log logger)
        {
            this.registryAccess = registryAccess;
            this.logger = logger;
        }

        /// <summary>
        /// Executes commands which will be defined in users class
        /// </summary>
        public void Execute()
        {
            IsRegistryOverwrritten = registryAccess.SetRegisterValue();

            if (!IsRegistryOverwrritten)
                logger.LogMessageViaEventLog(registryAccess.BaseRegistryKey + @"\" + registryAccess.SubKey + @"\" + registryAccess.SKeyName + @" Values is " + (IsRegistryOverwrritten ? "overwritten" : "not overwritten"));
        }

        /// <summary>
        /// Undoes commands which will be defined in users class and reverts all changes that was made
        /// </summary>
        public void Undo()
        {

        }
    }
}