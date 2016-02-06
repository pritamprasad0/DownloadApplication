using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;
using HtmlAgilityPack;
using Microsoft.Win32;

namespace DownloadApp
{
    internal static class SearchAndProcess
    {
        private static string _filepath = string.Empty;

        public static readonly WebClient Client = new WebClient();

        public static bool DownloadCompletedFlag;
        
        public static void GetResults(string searchquery)
        {
            switch (((MainWindow) Application.Current.MainWindow).WebsiteComboBox.Text)
            {
                case "Emp3world" : DownloadFromMp3World(searchquery);
                    break;
                case "Mp3Skull" : DownloadFromMp3Skull(searchquery);
                    break;
            }
        }

        private static void DownloadFromMp3Skull(string searchquery)
        {
        }

        private static void DownloadFromMp3World(string searchquery)
        {
            ((MainWindow) Application.Current.MainWindow).DownloadDataGrid.Items.Clear();
            var resultsBuffer = new byte[8192];
            var searchString = new StringBuilder();
            searchString.Append(DownloadDataModel.Emp3Website);
            searchString.Append(searchquery.Trim().Replace(" ", "_"));
            searchString.Append("_mp3_download.html");

            var request = (HttpWebRequest) WebRequest.Create(searchString.ToString());
            var response = (HttpWebResponse) request.GetResponse();
            var resultStream = response.GetResponseStream();

            var sb = new StringBuilder();
            var count = 0;

            do
            {
                if (resultStream != null)
                    count = resultStream.Read(resultsBuffer, 0, resultsBuffer.Length);
                if (count != 0)
                {
                    var tempString = Encoding.ASCII.GetString(resultsBuffer, 0, count);
                    sb.Append(tempString);
                }
            } while (count > 0);

            var webPageSource = sb.ToString();
            var html = new HtmlDocument {OptionOutputAsXml = true};
            html.LoadHtml(webPageSource);
            var doc = html.DocumentNode;
            var divResultBox = doc.SelectSingleNode("//div[@id='results_box']");
            var songDivs = divResultBox.SelectNodes(".//div[@class='song_item']");
            if (songDivs == null)
            {
                MessageBox.Show("File Not Found", "Not Found", MessageBoxButton.OK);
                return;
            }
            foreach (var songsDiv in songDivs)
            {
                var linkDiv = songsDiv.SelectSingleNode(".//a[@class='btn' and @rel='nofollow']");
                var nameDiv = songsDiv.SelectSingleNode(".//span[@id='song_title']");
                var sizeDiv = songsDiv.SelectSingleNode(".//div[@class='song_size']");
                var songName = nameDiv.InnerText;
                if (songName == string.Empty)
                {
                    songName = "Unknown";
                }
                var size = sizeDiv.InnerText.Trim();
                if (size == string.Empty)
                {
                    size = "Unknown";
                }
                var uri = linkDiv.GetAttributeValue("href", "Not Found");
                if (uri.Contains(".mp3"))
                {
                    ((MainWindow) Application.Current.MainWindow).DownloadDataGrid.Items.Add(new DownloadDataModel
                    {
                        FileName = songName,
                        Url = uri,
                        FileSize = size
                    });
                }
            }
        }

        public static void DownloadSong(string selectedItem)
        {
            DownloadCompletedFlag = false;
            var uri = new Uri(selectedItem);
            var savefieDialog = new SaveFileDialog
            {
                DefaultExt = "mp3",
                AddExtension = true,
                Filter = "MP3 File | *.mp3"
            };
            _filepath = string.Empty;
            var name = String.Empty;
            if (savefieDialog.ShowDialog() == true)
            {
                name = savefieDialog.FileName;
            }
            if (name == string.Empty)
            {
                var tempsb = new StringBuilder();
                tempsb.Append(DownloadDataModel.defaultDownloadPath);
                tempsb.Append(@"\");
                tempsb.Append(Path.GetFileName(uri.AbsolutePath));
                name = tempsb.ToString();
            }

            ((MainWindow) Application.Current.MainWindow).FileNameLabel.Content = Path.GetFileName(name);
            ((MainWindow) Application.Current.MainWindow).DownloadingFileLabel.Visibility = Visibility.Visible;
            ((MainWindow) Application.Current.MainWindow).DownloadProgrssBar.Visibility = Visibility.Visible;

            Client.DownloadProgressChanged += DownloadFileProgressChanged;
            Client.DownloadFileCompleted += DownloadCompleted;
            _filepath = name;
            Client.DownloadFileAsync(uri, name);
        }

        public static void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            DownloadCompletedFlag = true;
            var dialog = new DownloadCompletedBox(_filepath);
            dialog.ShowDialog();
        }


        private static void DownloadFileProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ((MainWindow) Application.Current.MainWindow).DownloadProgrssBar.Value = e.ProgressPercentage;
        }
    }
}