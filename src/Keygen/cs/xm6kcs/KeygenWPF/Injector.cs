using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows;
using System.IO;

namespace KeygenWPF
{
    public class Injector
    {
        public const String jar3_4_1A = "net.xmind.verify_3.4.1.201401221918.jar";
        public const String jar3_4_1B = "org.xmind.meggy_3.4.1.201401221918.jar";
        public const String jar3_5_0A = "net.xmind.verify_3.5.0.201410310637.jar";
        public const String jar3_5_0B = "org.xmind.meggy_3.5.0.201410310637.jar";
        public const String jar3_5_1A = "net.xmind.verify_3.5.1.201411201906.jar";
        public const String jar3_5_1B = "org.xmind.meggy_3.5.1.201411201906.jar";
        public const String keyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\xmind\shell\open\command";
        public const String keyPath2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\XMind Ltd\XMind";

        public void Inject()
        {
            Inject(GetPath(), GetVersoin());
        }

        public void Inject(String version)
        {
            Inject(GetPath(), version);
        }
        public void Inject(String path, String version)
        {
            String jar1 = "";
            String jar2 = "";
            byte[] jarContent1 = null;
            byte[] jarContent2 = null;
            switch(version)
            {
                case "3.4.1":
                    jar1 = jar3_4_1A;
                    jar2 = jar3_4_1B;
                    jarContent1 = Properties.Resources.net_xmind_verify_3_4_1_201401221918;
                    jarContent2 = Properties.Resources.org_xmind_meggy_3_4_1_201401221918;
                    break;
                case "3.5.0":
                    jar1 = jar3_5_0A;
                    jar2 = jar3_5_0B;
                    jarContent1 = Properties.Resources.net_xmind_verify_3_5_0_201410310637;
                    jarContent2 = Properties.Resources.org_xmind_meggy_3_5_0_201410310637;
                    break;
                case "3.5.1":
                    jar1 = jar3_5_1A;
                    jar2 = jar3_5_1B;
                    jarContent2 = Properties.Resources.net_xmind_verify_3_5_1_201411201906;
                    jarContent2 = Properties.Resources.org_xmind_meggy_3_5_1_201411201906;
                    break;
                default:
                    jar1 = jar3_5_1A;
                    jar2 = jar3_5_1B;
                    jarContent2 = Properties.Resources.net_xmind_verify_3_5_1_201411201906;
                    jarContent2 = Properties.Resources.org_xmind_meggy_3_5_1_201411201906;
                    break;
            }
            String dest = path + "\\plugins\\";
            if(!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }
            System.IO.FileStream fs = new System.IO.FileStream(dest + jar1, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            fs.Write(jarContent1, 0, jarContent1.Length);
            fs.Close();

            fs = new FileStream(dest + jar2, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            fs.Write(jarContent2, 0, jarContent2.Length);
            fs.Close();
            
        }

        public String GetVersoin()
        {
            String version = "";

            var subkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\XMind Ltd\XMind");
            if (subkey == null)
            {
                subkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\XMind Ltd\XMind");
            }
            if(subkey == null)
            {
                MessageBoxResult result = MessageBox.Show("无法找到XMind安装目录，要手动选择吗？", "Not found", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.CheckPathExists = true;
                    ofd.Title = "选择XMind目录";
                    ofd.CheckFileExists = true;
                    ofd.CheckPathExists = true;
                    ofd.DefaultExt = ".exe";
                    ofd.FileName = "XMind.exe";
                    ofd.Filter = "XMind executable|XMind.exe|All files|*.*";
                    ofd.ValidateNames = false;
                    var ret = ofd.ShowDialog();
                    String path;
                    if (ret.HasValue && ret.Value == true)
                    {
                        try
                        {
                            path = Path.GetDirectoryName(ofd.FileName) + "\\plugins";
                            String[] filenames = Directory.GetFiles(path, "net.xmind.verify_*.jar", SearchOption.TopDirectoryOnly);
                            if (filenames.Length > 0)
                            {
                                String name = filenames[0];
                                version = name.Substring(17, 5);
                            }
                        }
                        catch(System.IO.DirectoryNotFoundException ex)
                        {
                            version = "3.5.1";
                        }
                    }
                    else
                    {
                        version = "3.5.1";
                    }

                }
            }
            else
            {
                version = (String)subkey.GetValue("Version");
            }
            return version;
        }

        public String GetPath()
        {
            //throw new NotImplementedException();
            String path = "";
            
            var subkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\XMind Ltd\XMind");
            if (subkey == null)
            {
                subkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\XMind Ltd\XMind");
            }

            if (subkey == null)
            {
                MessageBoxResult result = MessageBox.Show("无法找到XMind安装目录，要手动选择吗？", "Not found", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.CheckPathExists = true;
                    sfd.Title = "选择XMind目录";
                    sfd.OverwritePrompt = false;
                    sfd.ValidateNames = false;
                    sfd.FileName = "xmind.exe";
                    var ret = sfd.ShowDialog();
                    if(ret.HasValue && ret.Value == true)
                    {
                        path = Path.GetDirectoryName(sfd.FileName);
                    }
                    else
                    {
                        path = ".";
                    }
                    
                }
            }
            else
            {
                path = (String)subkey.GetValue("Path");
            }

            return path;
        }
    }
}
