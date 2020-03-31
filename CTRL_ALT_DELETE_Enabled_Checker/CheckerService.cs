using CTRL_ALT_DELETE_Enabled_Checker.Utilities;
using Microsoft.Win32;
using System;
using System.ServiceProcess;
using System.Threading;

namespace CTRL_ALT_DELETE_Enabled_Checker
{
    public partial class CheckerService : ServiceBase
    {
        private Thread threadChecker = null;
        private bool isCheckerNeedRun = false;

        // Отключение CTRL+ALT+DELETE при входе
        private string subKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
        private string sValueName = "disablecad";

        // Параметры autologon
        private string subKeyAutoLogon = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";
        private string sDefaultDomainName = "DefaultDomainName";
        private string sDefaultUserName = "DefaultUserName";
        private string sDefaultPassword = "DefaultPassword";
        private string sAutoAdminLogon = "AutoAdminLogon";
        private string sForceAutoLogon = "ForceAutoLogon";
        private string sPowerdownAfterShutdown = "PowerdownAfterShutdown";

        // Отключение экрана Legal Notice перед входом в систему
        private string sLegalNoticeCaption = "legalnoticecaption";
        private string sLegalNoticeText = "legalnoticetext";

        // Отключение засыпания экрана
        private string subKeyScreenSaver = @"SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop";
        private string sScreenSaveActive = "ScreenSaveActive";
        private string sScreenSaveTimeOut = "ScreenSaveTimeOut";
        private string sScreenSaverIsSecure = "ScreenSaverIsSecure";
        private string sSCRNSAVE = "SCRNSAVE.EXE";

        private Log logger = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckerService"/> class.
        /// </summary>
        public CheckerService()
        {
            InitializeComponent();

            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;           
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            isCheckerNeedRun = true;
            threadChecker = new Thread(new ThreadStart(CheckRegisterValue));
            threadChecker.IsBackground = true;
            threadChecker.Start();
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            isCheckerNeedRun = false;
            if (threadChecker.IsAlive)
            {
                threadChecker.Abort();
                Thread.Sleep(6000);
                threadChecker = null;
            }
        }


        private void CheckRegisterValue()
        {
            if (logger == null)
                logger = new Log();

            // Блок проверки активности состояния входа чарез CTRL+ALT+DELETE, если активно - отключаем
            Checker checker = new Checker();
            RegistryAccess regAccess = new RegistryAccess();
            regAccess.BaseRegistryKey = Registry.LocalMachine;
            regAccess.SubKey = subKey;
            regAccess.SKeyName = sValueName;
            checker.SetCommand(new RegistryProcessing(regAccess, logger));

            Checker checkerAutoLogon = new Checker();
            RegistryAccess regAccessAutoLogon = new RegistryAccess();
            regAccessAutoLogon.BaseRegistryKey = Registry.LocalMachine;
            regAccessAutoLogon.SubKey = subKeyAutoLogon;
            regAccessAutoLogon.SValueNames = new string[] { sDefaultDomainName, sDefaultUserName, sDefaultPassword, sAutoAdminLogon, sForceAutoLogon, sPowerdownAfterShutdown };
            regAccessAutoLogon.OValues = new object[] {"GAMMA", "qalab", "Klin2013", 1, 1, 1 };
            checkerAutoLogon.SetCommand(new RegistryProcessingAutoLogon(regAccessAutoLogon, logger));

            Checker checkerDeleteVal = new Checker();
            RegistryAccess regAccessDeleteVal = new RegistryAccess();
            regAccessDeleteVal.BaseRegistryKey = Registry.LocalMachine;
            regAccessDeleteVal.SubKey = subKey;
            regAccessDeleteVal.SValueNames = new string[] { sLegalNoticeCaption, sLegalNoticeText };
            checkerDeleteVal.SetCommand(new RegistryProcessingDeleteVal(regAccessDeleteVal, logger));

            Checker checkerScreenSaver = new Checker();
            RegistryAccess regAccessScreenSaver = new RegistryAccess();
            regAccessScreenSaver.BaseRegistryKey = Registry.LocalMachine;
            regAccessScreenSaver.SubKey = subKeyAutoLogon;
            regAccessScreenSaver.SValueNames = new string[] { sScreenSaveActive, sScreenSaveTimeOut, sScreenSaverIsSecure, sSCRNSAVE };
            regAccessScreenSaver.OValues = new object[] { "0", "99999", "0", "logon.scr" };
            checkerScreenSaver.SetCommand(new RegistryProcessingAutoLogon(regAccessScreenSaver, logger));

            while (isCheckerNeedRun)
            {
                //if (Convert.ToInt32(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "disablecad", 0)) == 0)
                //{
                //    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "disablecad", 1);
                //}

                checker.StartCheckRegister();            // Проверка и отключение 

                checkerAutoLogon.StartCheckRegister();   // Установка параметров автологона

                checkerDeleteVal.StartCheckRegister();   // Удаление параметров экрана Legal Notice

                checkerScreenSaver.StartCheckRegister(); // Отключение экрана блокировки

                Thread.Sleep(5000);
            }
        }
    }
}
