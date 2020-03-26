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
            Checker checker = new Checker();
            RegistryAccess regAccess = new RegistryAccess();
            regAccess.BaseRegistryKey = Registry.LocalMachine;
            regAccess.SubKey = subKey;
            regAccess.SKeyName = sValueName;
            checker.SetCommand(new RegistryProcessing(regAccess));


            while (isCheckerNeedRun)
            {
                //if (Convert.ToInt32(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "disablecad", 0)) == 0)
                //{
                //    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "disablecad", 1);
                //}

                checker.StartCheckRegister(); // Проверка и отключение 

                Thread.Sleep(5000);
            }
        }
    }
}
