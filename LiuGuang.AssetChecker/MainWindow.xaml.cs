using LiuGuang.AssetChecker.ViewModels;
using System.Windows;

namespace LiuGuang.AssetChecker
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private MainWindowViewModel GetViewModel()
        {
            return DataContext as MainWindowViewModel;
        }

        /// <summary>
        /// 选择客户端目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectClientPath(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetViewModel().ClientPath = dialog.SelectedPath;
            }
        }


        /// <summary>
        /// 选择明文目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectSourcePath(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetViewModel().SourcePath = dialog.SelectedPath;
            }
        }
    }
}
