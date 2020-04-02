

using CTRL_ALT_DELETE_Enabled_Checker.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;

namespace UnitTest_CAD_Project
{
    [TestClass]
    public class RegistryAccessTest
    {
        Log logger = null;
        /// <summary>
        /// You should run Visual Studio as an Administrator to be able to pass the test
        /// If you try to 
        /// </summary>
        [TestMethod]
        //[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]       
        public void TestRegistryOverwritting()
        {            
            if (logger == null)
                logger = new Log();

            string subKeyAutoLogon = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";
            string sDefaultDomainName = "DefaultDomainName";
            string sDefaultUserName = "DefaultUserName";
            string sDefaultPassword = "DefaultPassword";
            string sAutoAdminLogon = "AutoAdminLogon";
            string sForceAutoLogon = "ForceAutoLogon";
            string sPowerdownAfterShutdown = "PowerdownAfterShutdown";

            Checker checkerAutoLogon = new Checker();
            RegistryAccess regAccessAutoLogon = new RegistryAccess();
            regAccessAutoLogon.BaseRegistryKey = Registry.LocalMachine;
            regAccessAutoLogon.SubKey = subKeyAutoLogon;
            regAccessAutoLogon.SValueNames = new string[] { sDefaultDomainName, sDefaultUserName, sDefaultPassword, sAutoAdminLogon, sForceAutoLogon, sPowerdownAfterShutdown };
            regAccessAutoLogon.OValues = new object[] { "GAMMA", "qalab", "Klin2013", 1, 1, 1 };

            RegistryProcessingAutoLogon rpAuto = new RegistryProcessingAutoLogon(regAccessAutoLogon, logger);
            checkerAutoLogon.SetCommand(rpAuto);

            checkerAutoLogon.StartCheckRegister();

            bool IsOverwrittingSuccess = rpAuto.IsRegistryOverwrritten;
            Assert.AreEqual(true, IsOverwrittingSuccess);
        }


        [TestMethod]
        //[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]       
        public void TestScreenSaverRegistryOverwritting()
        {
            if (logger == null)
                logger = new Log();

           string subKeyScreenSaver = @"SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop";
           string sScreenSaveActive = "ScreenSaveActive";
           string sScreenSaveTimeOut = "ScreenSaveTimeOut";
           string sScreenSaverIsSecure = "ScreenSaverIsSecure"; 
           string sSCRNSAVE = "SCRNSAVE.EXE";

            Checker checkerScreenSaver = new Checker();
            RegistryAccess regAccessScreenSaver = new RegistryAccess();
            regAccessScreenSaver.BaseRegistryKey = Registry.CurrentUser;
            regAccessScreenSaver.SubKey = subKeyScreenSaver;
            regAccessScreenSaver.SValueNames = new string[] { sScreenSaveActive, sScreenSaveTimeOut, sScreenSaverIsSecure, sSCRNSAVE };
            regAccessScreenSaver.OValues = new object[] { "0", "99999", "0", "logon.scr" };
            checkerScreenSaver.SetCommand(new RegistryProcessingAutoLogon(regAccessScreenSaver, logger));

            RegistryProcessingAutoLogon rpAuto = new RegistryProcessingAutoLogon(regAccessScreenSaver, logger);
            checkerScreenSaver.SetCommand(rpAuto);

            checkerScreenSaver.StartCheckRegister();

            bool IsOverwrittingSuccess = rpAuto.IsRegistryOverwrritten;
            Assert.AreEqual(true, IsOverwrittingSuccess);
        }
    }
}