using System.Windows;
using System.Windows.Input;

namespace DownloadApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            PreprocessingInitialization();
        }

        private void PreprocessingInitialization()
        {
            //this.ActiveControl = 
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchAndProcess.GetResults(DownloadTextBox.Text);
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var model = (DownloadDataModel) DownloadDataGrid.SelectedItem;
            if (model == null)
            {
                MessageBox.Show(
                    DownloadDataGrid.Items.Count != 0 ? "Select a Row from List" : "Please Search for Something first",
                    "Nothing to Download", MessageBoxButton.OK);
                return;
            }
            SearchAndProcess.DownloadSong(model.Url);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DownloadButton_Click(sender, e);
        }


        private void AddDataToListBox()
        {
            WebsiteComboBox.Items.Add("Emp3world");
            WebsiteComboBox.Items.Add("Mp3Skull");
            WebsiteComboBox.Items.Add("Temp");
            WebsiteComboBox.SelectedIndex = 0;
        }

        private void DownloadTextBox_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DownloadTextBox.Text = Clipboard.GetText();
        }

        private void TextBox_KeyEnterUpdate(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchAndProcess.GetResults(DownloadTextBox.Text);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DownloadProgrssBar.Visibility = Visibility.Hidden;
            DownloadingFileLabel.Visibility = Visibility.Hidden;
            AddDataToListBox();
        }

        private void StopDownloadButton_Click(object sender, RoutedEventArgs e)
        {
            SearchAndProcess.Client.DownloadFileCompleted -= SearchAndProcess.DownloadCompleted;
            SearchAndProcess.Client.CancelAsync();
            if (SearchAndProcess.DownloadCompletedFlag)
            {
                MessageBox.Show("Download Stopped", "", MessageBoxButton.OK);
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}