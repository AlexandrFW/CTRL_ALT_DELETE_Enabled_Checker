using Microsoft.Win32;
using System;
using System.Security.Permissions;
using System.Security.Principal;

namespace CTRL_ALT_DELETE_Enabled_Checker.Utilities
{
    /// <summary>
    /// Class reciever 
    /// </summary>
    public class RegistryAccess
    {

        #region Конструктор        
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryAccess"/> class.
        /// </summary>
        public RegistryAccess() { System.AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal); }
        #endregion


        #region Первоначальный узел реестра 
        /// <summary>
        /// The base registry key
        /// </summary>
        public RegistryKey BaseRegistryKey { get; set; } = Registry.LocalMachine;
        #endregion

        #region Значения SubKey   
        /// <summary>
        /// The sub key of the specified registry branch 
        /// </summary>        
        public string SubKey { get; set; } = "";
        #endregion

        #region Значения SubKey   
        /// <summary>
        /// The key of the specified registry value
        /// </summary>        
        public string SKeyName { get; set; } = "";
        #endregion

        #region Значения Value
        /// <summary>
        /// The value of the specified registry that should be installed
        /// </summary>        
        public object OValue { get; set; } = "";
        #endregion

        #region Массив имён Values
        /// <summary>
        /// The array of value names of the specified registry that should be installed
        /// </summary>        
        public string[] SValueNames { get; set; }
        #endregion

        #region Массив значений Values
        /// <summary>
        /// The array of values of the specified registry that should be installed
        /// </summary>        
        public object[] OValues { get; set; }
        #endregion

        #region Метод получения значений реестра        
        /// <summary>
        /// Gets the register value.
        /// </summary>
        /// <returns></returns>
        public string GetRegisterValue()
        {
            RegistryKey rk = BaseRegistryKey;
            // Open a subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey(SubKey);

            if (sk1 == null)
            {
                return "NULL";
            }
            else
            {
                try
                {
                    string s_result = (string)sk1.GetValue(SKeyName.ToUpper());
                    return s_result;
                }
                catch { return "NULL"; }
            }
        }
        #endregion

        #region Метод записи значений реестра        
        /// <summary>
        /// Sets the register value
        /// </summary>
        /// <param name="Value">The value that should be written</param>
        /// <returns></returns>
        public bool SetRegisterValue(object Value)
        {
            try
            {
                // Setting
                RegistryKey rk = BaseRegistryKey;
                // I have to use CreateSubKey 
                // (create or open it if already exits), 
                // 'cause OpenSubKey open a subKey as read-only
                RegistryKey sk1 = rk.CreateSubKey(SubKey);
                // Save the value
                sk1.SetValue(SKeyName.ToUpper(), Value);
                sk1.Close();

                return true;
            }
            catch { return false; }
        }
        #endregion


        #region Метод записи значений реестра        
        /// <summary>
        /// Sets the register value by enumeration an array values (Inside one registry subkey only)
        /// </summary>
        /// <returns></returns>
        //[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        public bool SetRegisterValue()
        {
            try
            {
                // Setting
                RegistryKey rk = BaseRegistryKey;
                // I have to use CreateSubKey 
                // (create or open it if already exits), 
                // 'cause OpenSubKey open a subKey as read-only
                RegistryKey sk1 = rk.CreateSubKey(SubKey);
                // Save the value

                for (int i = 0; i < SValueNames.Length; i++)
                {
                    sk1.SetValue(SValueNames[i], OValues[i]);
                    //Console.WriteLine($"Value name: {SValueNames[i]}, Value: {OValues[i]}"); // Для тестирования
                }
                sk1.Close();

                return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); Console.WriteLine(ex.StackTrace); return false; }
        }
        #endregion
    }
}