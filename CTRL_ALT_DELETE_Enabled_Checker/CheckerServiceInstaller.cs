using System.ComponentModel;
using System.ServiceProcess;


namespace CTRL_ALT_DELETE_Enabled_Checker
{
    /// <summary>
    /// Service installer
    /// </summary>
    /// <seealso cref="System.Configuration.Install.Installer" />
    [RunInstaller(true)]
    public partial class CheckerServiceInstaller : System.Configuration.Install.Installer
    {
        private readonly ServiceProcessInstaller processInstaller;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckerServiceInstaller"/> class.
        /// </summary>
        public CheckerServiceInstaller()
        {
            InitializeComponent();

            ServiceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };
            ServiceInstaller.StartType = ServiceStartMode.Automatic;
            ServiceInstaller.ServiceName = "CTRL_ALT_DELETE_Enabled_Checker";
            Installers.Add(processInstaller);
            Installers.Add(ServiceInstaller);
        }

        /// <summary>
        /// Gets the service installer
        /// </summary>
        /// <value>
        /// The service installer
        /// </value>
        public ServiceInstaller ServiceInstaller { get; }
    }
}