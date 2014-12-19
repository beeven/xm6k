﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeygenWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Generator generator = new Generator();
        Log logger = new Log();
        public MainWindow()
        {
            InitializeComponent();
            tbOutput.Text = "1. 先点击Patch对XMind打补丁\n2. 输入Email, 点击Generate生成序列号";
        }

        private void PatchButton_Click(object sender, RoutedEventArgs e)
        {
            Injector injector = new Injector();
            if (cbVersion.SelectedIndex <= 0)
            {
                injector.Inject();
            } else
            {
                injector.Inject((String)((ComboBoxItem)cbVersion.SelectedItem).Content);
            }
            tbOutput.Text = "已打补丁，请输入Email，然后点击Generate生成序列号。";
        }
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            if (tbInput.Text.Length > 0)
            {
                tbOutput.Text = "正在连接服务器";
                var ret = logger.LogEmail(tbInput.Text);
                ret.ContinueWith((x)=>
                {
                    //if (x.IsCompleted)
                    //{
                    //    tbOutput.Text = x.Result;
                    //}
                    //else
                    //{
                        String licenseKey = generator.generateLicenseKey(tbInput.Text);
                        tbOutput.Text = "---BEGIN LICENSE KEY---\r\n" + licenseKey + "\r\n---END LICENSE KEY---";
                    //}
                    Clipboard.SetText(tbOutput.Text);
                    tbOutput.AppendText("\r\n\r\n以上序列号已复制到剪贴板，请打开xmind->帮助->序列号->输入序列号，填入上面的Email地址和Ctrl+V粘贴序列号即可。");
                },context);
                
            }
        }

    }
}
