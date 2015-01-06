using System;
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
using System.ComponentModel;
using KeygenWPF.Log;

namespace KeygenWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Generator generator = new Generator();
        Logger logger = new Logger();

        bool shiftKeyDown = false;


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
            }
            else
            {
                injector.Inject((String)((ComboBoxItem)cbVersion.SelectedItem).Content);
            }
            tbOutput.Text = "已打补丁，请输入Email，然后点击Generate生成序列号。";
        }
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            this.btnGenerate.IsEnabled = false;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            var context = TaskScheduler.FromCurrentSynchronizationContext();

            if (tbInput.Text.Length > 0)
            {
                tbOutput.Text = "正在努力计算中...";
                if (shiftKeyDown == false)
                {
                    bw.RunWorkerAsync(tbInput.Text);
                }
                else
                {
                    String licenseKey = generator.generateLicenseKey(tbInput.Text);
                    tbOutput.Text = "---BEGIN LICENSE KEY---\r\n" + licenseKey + "\r\n---END LICENSE KEY---";
                    Clipboard.SetText(tbOutput.Text);
                    tbOutput.AppendText("\r\n\r\n以上序列号已复制到剪贴板，请打开xmind->帮助->序列号->输入序列号，填入上面的Email地址和Ctrl+V粘贴序列号即可。");
                }
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Result is ConnectionExeption)
            {
                tbOutput.Text = "无法连接到服务器";
            }
            else if (e.Result is MaximumUsageException)
            {
                tbOutput.Text = "已超出最大使用次数";
            }
            else
            {
                tbOutput.Text = e.Result as String;
                Clipboard.SetText(tbOutput.Text);
                tbOutput.AppendText("\r\n\r\n以上序列号已复制到剪贴板，请打开xmind->帮助->序列号->输入序列号，填入上面的Email地址和Ctrl+V粘贴序列号即可。");
            }
            this.btnGenerate.IsEnabled = true;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var ret = logger.LogEmail((string)e.Argument);
            try
            {
                ret.Wait();
                e.Result = ret.Result;
            }
            catch (AggregateException ae)
            {
                ae.Handle((x) =>
                {
                    if (x is ConnectionExeption || x is MaximumUsageException)
                    {
                        e.Result = x;
                        return true;
                    }
                    else
                    {
                        e.Result = x;
                        return false;
                    }
                });
            }
        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                this.shiftKeyDown = true;
            }
        }

        private void Button_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                this.shiftKeyDown = false;
            }
        }

    }
}
