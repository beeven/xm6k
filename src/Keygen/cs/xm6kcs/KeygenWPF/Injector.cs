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
        public const String jar1 = "net.xmind.verify_3.5.0.201410310637.jar";
        public const String jar2 = "org.xmind.meggy_3.5.0.201410310637.jar";
        public const String keyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\xmind\shell\open\command";
        public const String keyPath2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\XMind Ltd\XMind";
        public void inject(String path)
        {
            String dest = path + "\\plugins\\";
            if(!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }
            System.IO.FileStream fs = new System.IO.FileStream(dest + jar1, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            fs.Write(Properties.Resources.net_xmind_verify_3_5_0_201410310637, 0, Properties.Resources.net_xmind_verify_3_5_0_201410310637.Length);
            fs.Close();

            fs = new FileStream(dest + jar2, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            fs.Write(Properties.Resources.org_xmind_meggy_3_5_0_201410310637, 0, Properties.Resources.org_xmind_meggy_3_5_0_201410310637.Length);
            fs.Close();
            
        }

        public String findPathFromRegistory()
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
