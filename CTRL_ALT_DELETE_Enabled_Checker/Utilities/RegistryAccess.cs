using Microsoft.Win32;


namespace CTRL_ALT_DELETE_Enabled_Checker.Utilities
{
    public class RegistryAccess
    {
        #region Первоначальный узел реестра        
        /// <summary>
        /// The base registry key
        /// </summary>
        public static RegistryKey baseRegistryKey = Registry.LocalMachine;
        #endregion

        #region Значения SubKey
        public static string subKey = "";
        #endregion

        #region Метод получения значений реестра
        public static string GetRegisterValue(string sKeyName)
        {
            RegistryKey rk = baseRegistryKey;
            // Open a subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey(subKey);

            if (sk1 == null)
            {
                return "NULL";
            }
            else
            {
                try
                {
                    string s_result = (string)sk1.GetValue(sKeyName.ToUpper());
                    return s_result;
                }
                catch { return "NULL"; }
            }
        }
        #endregion

        #region Метод записи значений реестра
        public static bool SetRegisterValue(string sKeyName, object Value)
        {
            try
            {
                // Setting
                RegistryKey rk = baseRegistryKey;
                // I have to use CreateSubKey 
                // (create or open it if already exits), 
                // 'cause OpenSubKey open a subKey as read-only
                RegistryKey sk1 = rk.CreateSubKey(subKey);
                // Save the value
                sk1.SetValue(sKeyName.ToUpper(), Value);

                return true;
            }
            catch { return false; }
        }
        #endregion
    }
}