using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTRL_ALT_DELETE_Enabled_Checker.Utilities
{
    /// <summary>
    /// Interface for realize Command pattern behavior
    /// </summary>
    public interface ICommandInterface
    {
        /// <summary>
        /// Executes commands which will be defined in users class
        /// </summary>
        void Execute();

        /// <summary>
        /// Undoes commands which will be defined in users class and reverts all changes that was made
        /// </summary>
        void Undo();
    }
}
