
using System;



namespace CTRL_ALT_DELETE_Enabled_Checker.Utilities
{
    /// <summary>
    /// Class for executing commands which make operations with registry 
    /// </summary>
    public class RegistryProcessingDeleteVal : ICommandInterface
    {
        private RegistryAccess registryAccess;
        private Log logger;
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryProcessing"/> class (Constructor)
        /// </summary>
        public RegistryProcessingDeleteVal(RegistryAccess registryAccess, Log logger) 
        { 
            this.registryAccess = registryAccess;
            this.logger = logger;
        }

        /// <summary>
        /// Executes commands which will be defined in users class
        /// </summary>
        public void Execute() => registryAccess.DeleteKeysArray();

        /// <summary>
        /// Undoes commands which will be defined in users class and reverts all changes that was made
        /// </summary>
        public void Undo()
        {
            
        }

    }
}
