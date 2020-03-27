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

        private string subKey = @"\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
        private string sValueName = "disablecad";

        private string subKeyAutoLogon = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";
        private string sDefaultDomainName = "DefaultDomainName";
        private string sDefaultUserName = "DefaultUserName";
        private string sDefaultPassword = "DefaultPassword";
        private string sAutoAdminLogon = "AutoAdminLogon";
        private string sForceAutoLogon = "ForceAutoLogon";
        private string sPowerdownAfterShutdown = "PowerdownAfterShutdown";

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
            // Блок проверки активности состояния входа чарез CTRL+ALT+DELETE, если активно - отключаем
            Checker checker = new Checker();
            RegistryAccess regAccess = new RegistryAccess();
            regAccess.BaseRegistryKey = Registry.LocalMachine;
            regAccess.SubKey = subKey;
            regAccess.SKeyName = sValueName;
            checker.SetCommand(new RegistryProcessing(regAccess));

            Checker checkerAutoLogon = new Checker();
            RegistryAccess regAccessAutoLogon = new RegistryAccess();
            regAccessAutoLogon.BaseRegistryKey = Registry.LocalMachine;
            regAccessAutoLogon.SubKey = subKeyAutoLogon;
            regAccessAutoLogon.SValueNames = new string[] { sDefaultDomainName, sDefaultUserName, sDefaultPassword, sAutoAdminLogon, sForceAutoLogon, sPowerdownAfterShutdown };
            regAccessAutoLogon.OValues = new object[] {"GAMMA", "qalab", "Klin2013", "1", "1", "1" };
            checkerAutoLogon.SetCommand(new RegistryProcessingAutoLogon(regAccessAutoLogon));


            while (isCheckerNeedRun)
            {
                //if (Convert.ToInt32(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "disablecad", 0)) == 0)
                //{
                //    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "disablecad", 1);
                //}

                checker.StartCheckRegister();            // Проверка и отключение 

                checkerAutoLogon.StartCheckRegister();

                Thread.Sleep(5000);
            }
        }
    }
}
