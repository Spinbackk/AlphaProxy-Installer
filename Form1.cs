using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace LCProxy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
                message.Result = (IntPtr)HTCAPTION;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/cosmetics");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string contents = System.IO.File.ReadAllText("C:\\Windows\\System32\\drivers\\etc\\hosts");
            if (contents.Contains("51.195.220.243 assetserver.lunarclientprod.com"))
                contents = contents.Replace("51.195.220.243 assetserver.lunarclientprod.com", "");
            if (contents.Contains("194.163.177.249 assetserver.lunarclientprod.com"))
                contents = contents.Replace("194.163.177.249 assetserver.lunarclientprod.com", "");
            System.IO.File.WriteAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", contents);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Installing Alpha Proxy!";
            installLCP();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FileSecurity accessControl = System.IO.File.GetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts");
            accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference)new SecurityIdentifier(WellKnownSidType.WorldSid, (SecurityIdentifier)null), FileSystemRights.FullControl, AccessControlType.Allow));
            System.IO.File.SetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts", accessControl);
            string contents = System.IO.File.ReadAllText("C:\\Windows\\System32\\drivers\\etc\\hosts");
            if (contents.Contains("198.251.84.251 assetserver.lunarclientprod.com"))
                contents = contents.Replace("198.251.84.251 assetserver.lunarclientprod.com", "");
            System.IO.File.WriteAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", contents);
            accessControl.RemoveAccessRule(new FileSystemAccessRule((IdentityReference)new SecurityIdentifier(WellKnownSidType.WorldSid, (SecurityIdentifier)null), FileSystemRights.FullControl, AccessControlType.Allow));
            System.IO.File.SetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts", accessControl);
            button2.Text = "Uninstalled Alpha Proxy!";
        }
        private void installLCP()
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\JavaSoft\\Java Runtime Environment\\1.8");
            if (registryKey != null)
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile("https://assets.lunarproxy.me/server.cer", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LCPcert.cer"));
                    string str1 = (string)registryKey.GetValue("JavaHome");
                    string str2 = "zulu";
                    foreach (FileSystemInfo fileSystemInfo in new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.lunarclient\\jre").GetFileSystemInfos("*" + str2 + "*"))
                    {
                        string fullName = fileSystemInfo.FullName;
                        Process.Start(str1 + "\\bin\\keytool.exe", "-keystore \"" + fullName + "\\lib\\security\\cacerts\" -trustcacerts -importcert -alias lcproxy -storepass changeit -file \"" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LCPcert.cer") + "\" -noprompt");
                    }
                    webClient.Dispose();
                }
                if (!System.IO.File.Exists("C:\\Windows\\System32\\drivers\\etc\\hosts"))
                    System.IO.File.Create("C:\\Windows\\System32\\drivers\\etc\\hosts");
                FileSecurity accessControl = System.IO.File.GetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts");
                accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference)new SecurityIdentifier(WellKnownSidType.WorldSid, (SecurityIdentifier)null), FileSystemRights.FullControl, AccessControlType.Allow));
                System.IO.File.SetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts", accessControl);
                string contents = System.IO.File.ReadAllText("C:\\Windows\\System32\\drivers\\etc\\hosts");
                if (!contents.Contains("198.251.84.251 assetserver.lunarclientprod.com"))
                    contents += "\n198.251.84.251 assetserver.lunarclientprod.com";
                System.IO.File.WriteAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", contents);
                accessControl.RemoveAccessRule(new FileSystemAccessRule((IdentityReference)new SecurityIdentifier(WellKnownSidType.WorldSid, (SecurityIdentifier)null), FileSystemRights.FullControl, AccessControlType.Allow));
                System.IO.File.SetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts", accessControl);
                button1.Text = "Installed Alpha Proxy!";
            }
            else
                new Thread(new ThreadStart(this.noJava)).Start();
        }
        private void noJava()
        {
            if (MessageBox.Show("You are missing JDK 1.8, would you like to install it?\nThis may take up to 5 minutes.", "Missing JDK", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (WebClient webClient = new WebClient())
                {
                    Process.Start("");
                }
            }
            else
            {
                int num = (int)MessageBox.Show("The installation cannot continue.", "Missing JDK", MessageBoxButtons.OK);
                Application.Exit();
            }
        }
    }
}
