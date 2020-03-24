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

        public CheckerService()
        {
            InitializeComponent();

            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            isCheckerNeedRun = true;
            threadChecker = new Thread(new ThreadStart(CheckRegisterValue));
            threadChecker.IsBackground = true;
            threadChecker.Start();
        }

        protected override void OnStop()
        {
        }


        private void CheckRegisterValue()
        {
            while (isCheckerNeedRun)
            {
                if (Convert.ToInt32(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "disablecad", 0)) == 0)
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "disablecad", 1);
                }

                Thread.Sleep(5000);
            }
        }
    }
}
