using System.ComponentModel;
using System.ServiceProcess;


namespace CTRL_ALT_DELETE_Enabled_Checker
{
    [RunInstaller(true)]
    public partial class CheckerServiceInstaller : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;

        public CheckerServiceInstaller()
        {
            InitializeComponent();

            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "CTRL_ALT_DELETE_Enabled_Checker";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}