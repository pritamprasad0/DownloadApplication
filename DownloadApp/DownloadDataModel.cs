using System;
using System.Collections.Generic;
using System.IO;

namespace DownloadApp
{
    public class DownloadDataModel
    {
        public string FileName { get; set; }
        public string Url { get; set; }
        public string FileSize { get; set; }
        public static readonly string Emp3Website = @"http://emp3world.biz/search/";
        public static readonly string Mp3skullWebsite = "";
        public static readonly string defaultDownloadPath = @".\";
        public static readonly string defaultEngine = Emp3Website;

        static DownloadDataModel()
        {
            var pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var pathDownload = Path.Combine(pathUser, "Downloads");
            defaultDownloadPath = pathDownload;
        }
    }
}