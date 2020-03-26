using System;



namespace CTRL_ALT_DELETE_Enabled_Checker.Utilities
{
    /// <summary>
    /// Class for executing commands which make operations with registry 
    /// </summary>
    public class RegistryProcessing : ICommandInterface
    {
        private RegistryAccess registryAccess;
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryProcessing"/> class (Constructor)
        /// </summary>
        public RegistryProcessing(RegistryAccess registryAccess) { this.registryAccess = registryAccess; }

        /// <summary>
        /// Executes commands which will be defined in users class
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Execute()
        {
            string s_disable_param = registryAccess.GetRegisterValue();
            if(Convert.ToInt32(s_disable_param) == 0)
            {
                registryAccess.SetRegisterValue(1);
            }
        }

        /// <summary>
        /// Undoes commands which will be defined in users class and reverts all changes that was made
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Undo()
        {
            
        }

    }
}
