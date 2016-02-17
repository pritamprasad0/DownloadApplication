using System.Diagnostics;
using System.IO;
using System.Windows;

namespace DownloadApp
{
    /// <summary>
    /// Interaction logic for DownloadCompletedBox.xaml
    /// </summary>
    public partial class DownloadCompletedBox
    {
        private string _filepath = null;

        public DownloadCompletedBox(string path)
        {
            InitializeComponent();
            _filepath = path;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow) Application.Current.MainWindow).DownloadProgrssBar.Value = 0;
            ((MainWindow) Application.Current.MainWindow).FileNameLabel.Content = "";
            ((MainWindow) Application.Current.MainWindow).DownloadingFileLabel.Visibility = Visibility.Hidden;
            ((MainWindow) Application.Current.MainWindow).DownloadProgrssBar.Visibility = Visibility.Hidden;
            Close();
        }

        private void OpenFileLocButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Directory.GetParent(_filepath).FullName);
            Close();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("wmplayer.exe", _filepath);
            Close();
        }
    }
}