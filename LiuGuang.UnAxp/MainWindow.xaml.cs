using LiuGuang.UnAxp.ViewModels;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace LiuGuang.UnAxp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string initialDirectory = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
        }

        private MainWindowViewModel GetViewModel()
        {
            return DataContext as MainWindowViewModel;
        }

        /// <summary>
        /// 选择AXP文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAxpFile(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".axp",
                Filter = "AXP文件|*.axp"
            };
            if (!string.IsNullOrEmpty(initialDirectory))
            {
                dialog.InitialDirectory = initialDirectory;
            }
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                GetViewModel().AxpFilePath = dialog.FileName;
                initialDirectory = Path.GetDirectoryName(dialog.FileName);
            }
        }

        /// <summary>
        /// 选择保存目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectOutputPath(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetViewModel().OutputPath = dialog.SelectedPath;
            }
        }
    }
}
