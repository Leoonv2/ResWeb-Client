using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;

namespace ResWeb_Installer
{
    public partial class Form1 : Form
    {
    
    #region variables
        string serviceName = "ResWeb";

        bool installed = false;
        bool running = false;
#endregion
    
    #region handlers
        void check()
        {
            if (
                !ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals(serviceName))
            )
            {
                installed = false;
            }
            else
            {
                installed = true;
            }

            if (ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals(serviceName) && serviceController.Status == ServiceControllerStatus.Running))
            {
                running = true;
            }
            else
            {
                running = false;
            }

            if (running) { start.Text = "Stop Service"; } else { start.Text = "Start Service"; }
            if (installed) { install.Text = "Uninstall Service"; } else { install.Text = "Install Service"; }
        }
        void clearConsole()
        {
            outputbox.Clear();
        }

        void writeToConsole(string text, bool error = false)
        {
            clearConsole();
            if (error)
            {

                outputbox.Font = new Font(outputbox.Font, FontStyle.Bold);
                outputbox.SelectionColor = Color.Red;
                outputbox.AppendText(text + "\n");
            }
            else
            {

                outputbox.Font = new Font(outputbox.Font, FontStyle.Regular);
                outputbox.SelectionColor = Color.Black;
                outputbox.AppendText(text + "\n");
            }

        }

        #endregion
    
    #region Form1
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            writeToConsole("ResWeb Installer v1.0.0.0\nCreated by: Leoon And Luca");

            check();

        }

        #endregion
    
    #region buttons
        private void install_Click(object sender, EventArgs e)
        {
            if (!installed)
            {
                if (
                    !ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals(serviceName)))
                {
                    string installUtilPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe";
                    if (!File.Exists(installUtilPath))
                    {
                        writeToConsole("InstallUtil.exe not found.", true);

                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.Title = "Select InstallUtil.exe";
                        ofd.Filter = "InstallUtil.exe|InstallUtil.exe";
                        ofd.Multiselect = false;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            installUtilPath = ofd.FileName;
                        }
                        else return;
                    }

                    string servicePath = @"C:\Program Files\ResWeb\ResWeb";

                    if (!Directory.Exists(@"C:\Program Files\ResWeb\ResWeb"))
                    {
                        writeToConsole("ResWeb not found.", true);
                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.Title = "Select ResWeb.exe";
                        ofd.Filter = "Executable|*.exe";
                        ofd.InitialDirectory = System.Reflection.Assembly.GetEntryAssembly().Location;
                        ofd.Multiselect = false;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            servicePath = ofd.FileName;
                        }
                        else return;
                    }

                    clearConsole();

                    ProcessStartInfo psi = new ProcessStartInfo(installUtilPath);
                    psi.Arguments = string.Format("/i \"{0}\"", servicePath); // Pass the service path as an argument
                    psi.RedirectStandardOutput = true;
                    psi.UseShellExecute = false;
                    psi.CreateNoWindow = true;

                    // Start the InstallUtil.exe process
                    Process process = new Process();
                    process.StartInfo = psi;
                    process.Start();

                    // Wait for the process to complete
                    process.WaitForExit();

                    // Check if the installation was successful
                    if (process.ExitCode == 0)
                    {
                        writeToConsole("Service installed successfully.");
                        if (running) { start.Text = "Stop Service"; } else { start.Text = "Start Service"; }
                        
                    }
                    else
                    {
                        writeToConsole("Failed to install the service.", true);
                    }
                }
            }
            else
            {
                try
                {
                    string installUtilPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe";
                    if (!File.Exists(installUtilPath))
                    {
                        writeToConsole("InstallUtil.exe not found.", true);

                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.Title = "Select InstallUtil.exe";
                        ofd.Filter = "InstallUtil.exe|InstallUtil.exe";
                        ofd.Multiselect = false;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            installUtilPath = ofd.FileName;
                        }
                        else return;
                    }

                    string servicePath = @"C:\Program Files\ResWeb\ResWeb";

                    if (!Directory.Exists(@"C:\Program Files\ResWeb\ResWeb"))
                    {
                        writeToConsole("ResWeb not found.", true);
                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.Title = "Select ResWeb.exe";
                        ofd.Filter = "Executable|*.exe";
                        ofd.InitialDirectory = System.Reflection.Assembly.GetEntryAssembly().Location;
                        ofd.Multiselect = false;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            servicePath = ofd.FileName;
                        }
                        else return;
                    }

                    clearConsole();

                    ProcessStartInfo psi = new ProcessStartInfo(installUtilPath);
                    psi.Arguments = string.Format("/u \"{0}\"", servicePath); // Pass the service path as an argument
                    psi.RedirectStandardOutput = true;
                    psi.UseShellExecute = false;
                    psi.CreateNoWindow = true;

                    // Start the InstallUtil.exe process
                    Process process = new Process();
                    process.StartInfo = psi;
                    process.Start();

                    // Wait for the process to complete
                    process.WaitForExit();

                    // Check if the installation was successful
                    if (process.ExitCode == 0)
                    {
                        writeToConsole("Service uninstalled successfully.");
                        if (installed) { install.Text = "Uninstall Service"; } else { install.Text = "Install Service"; }

                    }
                    else
                    {
                        writeToConsole("Failed to uninstall the service.", true);
                    }

                    
                }
                catch (Exception ex)
                {
                    writeToConsole(
                        $"Failed to uninstall service '{serviceName}'. Error: {ex.Message}",
                        true
                    );
                }
            }
            check();
        }

        private void start_Click(object sender, EventArgs e)
        {
            if (!running)
            {
                ProcessStartInfo startServicePsi = new ProcessStartInfo("net",$"start {serviceName}");
                startServicePsi.RedirectStandardOutput = true;
                startServicePsi.UseShellExecute = false;
                startServicePsi.CreateNoWindow = true;

                Process startServiceProcess = new Process();
                startServiceProcess.StartInfo = startServicePsi;
                startServiceProcess.Start();

                startServiceProcess.WaitForExit();

                if (startServiceProcess.ExitCode == 0)
                {
                    writeToConsole("Service started successfully.");
                    if (running) { start.Text = "Stop Service"; } else { start.Text = "Start Service"; }
                    
                }
                else
                {
                    writeToConsole("Service Not Installed!.", true);
                }
            }
            else
            {
                ProcessStartInfo startServicePsi = new ProcessStartInfo("net",$"stop {serviceName}");
                startServicePsi.RedirectStandardOutput = true;
                startServicePsi.UseShellExecute = false;
                startServicePsi.CreateNoWindow = true;

                Process startServiceProcess = new Process();
                startServiceProcess.StartInfo = startServicePsi;
                startServiceProcess.Start();

                startServiceProcess.WaitForExit();

                if (startServiceProcess.ExitCode == 0)
                {
                    writeToConsole("Service stopped successfully.");

                    if (running) { start.Text = "Stop Service"; } else { start.Text = "Start Service"; }
                }
                else
                {
                    writeToConsole("Failed to stop the service.", true);
                }
            }
            check();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            check();
        }
#endregion
    }
    
}
