using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            injector.inject(injector.findPathFromRegistory());
            tbOutput.Text = "已打补丁，请输入Email，然后点击Generate生成序列号。";
        }
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            if (tbInput.Text.Length > 0)
            {
                logger.LogEmail(tbInput.Text);
                String licenseKey = generator.generateLicenseKey(tbInput.Text);
                tbOutput.Text = "---BEGIN LICENSE KEY---\r\n" + licenseKey + "\r\n---END LICENSE KEY---";
                Clipboard.SetText(tbOutput.Text);
                tbOutput.AppendText("\r\n\r\n以上序列号已复制到剪贴板，请打开xmind->帮助->序列号->输入序列号，填入上面的Email地址和Ctrl+V粘贴序列号即可。");
            }
        }

    }
}
