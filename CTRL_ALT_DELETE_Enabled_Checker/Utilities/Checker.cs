using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTRL_ALT_DELETE_Enabled_Checker.Utilities
{
    /// <summary>
    /// Class for initiating (invoke) command for register checking
    /// </summary>
    public class Checker
    {
        ICommandInterface command;

        /// <summary>
        /// Initializes a new instance of the <see cref="Checker"/> class
        /// </summary>
        public Checker() { }

        /// <summary>
        /// Sets the command for executing
        /// </summary>
        /// <param name="command">The command.</param>
        public void SetCommand(ICommandInterface command)
        {
            this.command = command;
        }

        /// <summary>
        /// Starts the check register
        /// </summary>
        public void StartCheckRegister()
        {
            command.Execute();
        }

        /// <summary>
        /// Reverses the checker changes
        /// </summary>
        public void ReverseCheckerChanges()
        {
            command.Undo();
        }
    }
}