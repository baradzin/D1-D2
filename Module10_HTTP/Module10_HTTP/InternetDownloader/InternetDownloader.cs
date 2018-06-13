using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Microsoft.CSharp;
using AngleSharp;
using AngleSharp.Parser.Html;
using AngleSharp.Dom.Html;
using AngleSharp.Dom;

namespace InternetDownloaderLib
{
    public class InternetDownloader
    {
        public string StartUrl { get; set; }

        public string DirectoryPath { get; set; }

        public int DepthLevel { get; set; }

        public bool IsVerbose { get; set; }

        public CrossingOption CrossingOption { get; set; }

        public string[] ExcludedExtension { get; set; }

        private HtmlParser _htmlParser;

        IConfiguration _config;

        private const int maxFolderNameLimit = 65;

        public FileSaver fileSaver;

        public InternetDownloader() { }

        public InternetDownloader(string startUrl, string path, int depthLevel,
            bool isVerbose, CrossingOption opt, string[] excludedExtension)
        {
            this.StartUrl = startUrl;
            this.DirectoryPath = path;
            this.DepthLevel = depthLevel;
            this.IsVerbose = isVerbose;
            this.CrossingOption = opt;
            this.ExcludedExtension = excludedExtension;
            this._htmlParser = new HtmlParser();
            this._config = Configuration.Default.WithDefaultLoader();
            this.fileSaver = new FileSaver();
        }

        public void DownloadContent()
        {
            DownloadContent(StartUrl, 0, DirectoryPath);
        }

        private void DownloadContent(string startUrl, int level, string folder)
        {
            Console.WriteLine(startUrl + " processed");
            if (level > DepthLevel)
            {
                return;
            }

            var document = BrowsingContext.New(_config).OpenAsync(startUrl).GetAwaiter().GetResult();
            string fileName = GetDirectoryName(startUrl);

            string pathString = Path.Combine(folder, fileName);

            if (!Directory.Exists(pathString))
            {
                Directory.CreateDirectory(pathString);
            }

            fileSaver.SaveSourceHtml(document.Source.Text, fileName, pathString);

            foreach(var img in document.Images)
            {
                DownloadImage(img.ToString(), pathString);
            }

            foreach (IHtmlAnchorElement link in document.Links)
            {
                DownloadContent(link.Href, level + 1, pathString);
            }
        }

        private void DownloadImage(string url, string folder)
        {
            try
            {
                var type = url.Substring(url.LastIndexOf('.'));
                if (ExcludedExtension.Contains(type.Substring(1))) return;

                using (var client = new HttpClient())
                {
                    var fileName = Path.Combine(folder,
                        GetDirectoryName(url) + type);

                    using (var response = client.GetAsync(url).GetAwaiter().GetResult())
                    {
                        using (var imageFile = new FileStream(fileName, FileMode.Create))
                        {
                            response.Content.CopyToAsync(imageFile).GetAwaiter().GetResult();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load the image: {0}", ex.Message);
            }
        }

        private string GetDirectoryName(string path)
        {
            try
            {
                var url = path.Replace(".", "").Replace(@"/", "").Replace(":", "")
                    .Replace("?", "").Replace("|", "");

                return url.Substring(url.Length - maxFolderNameLimit > 0 ? url.Length - maxFolderNameLimit : 0);
            }
            catch (UriFormatException)
            {
                throw new UriFormatException("Url has incorrect format");
            }
        }
    }

    public enum CrossingOption
    {
        OnlyInternalRecources = 1,
        AllRecources = 2,
    }
}
