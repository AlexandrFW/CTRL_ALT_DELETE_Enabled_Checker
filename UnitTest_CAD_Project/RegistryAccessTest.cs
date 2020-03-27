using Microsoft.VisualStudio.TestTools.UnitTesting;

using CTRL_ALT_DELETE_Enabled_Checker;
using CTRL_ALT_DELETE_Enabled_Checker.Utilities;
using Microsoft.Win32;
using System.Security.Permissions;

namespace UnitTest_CAD_Project
{
    [TestClass]
    public class RegistryAccessTest
    {
        /// <summary>
        /// You should run Visual Studio as an Administrator to be able to pass the test
        /// If you try to 
        /// </summary>
        [TestMethod]
        //[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        public void TestRegistryOverwritting()
        {
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
            regAccessAutoLogon.OValues = new object[] { "GAMMA", "qalab", "Klin2013", "1", "1", "1" };

            RegistryProcessingAutoLogon rpAuto = new RegistryProcessingAutoLogon(regAccessAutoLogon);
            checkerAutoLogon.SetCommand(rpAuto);

            checkerAutoLogon.StartCheckRegister();

            bool IsOverwrittingSuccess = rpAuto.IsRegistryOverwrritten;
            Assert.AreEqual(true, IsOverwrittingSuccess);
        }

    }
}