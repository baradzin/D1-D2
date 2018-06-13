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
using System.Threading;

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

        private ILogger _logger;

        public FileSaver fileSaver;

        public InternetDownloader() { }

        public InternetDownloader(string startUrl, string path, int depthLevel,
            bool isVerbose, CrossingOption opt, string[] excludedExtension, ILogger logger)
        {
            this.StartUrl = startUrl;
            this.DirectoryPath = path;
            this.DepthLevel = depthLevel;
            this.IsVerbose = isVerbose;
            this.CrossingOption = opt;
            this.ExcludedExtension = excludedExtension;
            this._htmlParser = new HtmlParser();
            this._config = Configuration.Default.WithDefaultLoader();        
            this._logger = logger;
            this.fileSaver = new FileSaver(logger);
            this._logger.IsVerbose = isVerbose;
        }

        public async Task DownloadContent()
        {
            await DownloadContentAsync(StartUrl, 0, DirectoryPath);
        }

        private async Task DownloadContentAsync(string startUrl, int level, string folder)
        {
            _logger.WriteLine(startUrl + " processed");
            if (level > DepthLevel)
            {
                return;
            }

            var document = await BrowsingContext.New(_config).OpenAsync(startUrl);
            string fileName = GetDirectoryName(startUrl);

            string pathString = Path.Combine(folder, fileName);

            if (!Directory.Exists(pathString))
            {
                Directory.CreateDirectory(pathString);
            }

            fileSaver.SaveSourceHtml(document.Source.Text, fileName, pathString);

            foreach(var file in document.All.Where(x => x.HasAttribute("src")))
            {             
                await DownloadFile(document.Origin, file.Attributes["src"].Value, pathString);
            }

            foreach (IHtmlAnchorElement link in document.Links)
            {
                if (CrossingOption == CrossingOption.OnlyInternalRecources)
                {
                    if (link.HostName.Equals(document.Domain))
                    {
                        await DownloadContentAsync(link.Href, level + 1, pathString);
                    }
                } else {
                    if (link.HostName.Equals(document.Domain))
                    {
                        await DownloadContentAsync(link.Href, level + 1, pathString);
                    } else
                    {
                        await DownloadContentAsync(link.Href, level + 1, DirectoryPath);
                    }
                }         
            }
        }

        private async Task DownloadFile(string baseUrl, string url, string folder)
        {
            try
            {
                var type = url.Substring(url.LastIndexOf('.'));
                if (ExcludedExtension.Contains(type.Substring(1))) return;

                using (var client = new HttpClient())
                {
                    var fileName = Path.Combine(folder,
                        GetDirectoryName(url) + type);
                    client.BaseAddress = new Uri(baseUrl);
                    using (var response = client.GetAsync(url).GetAwaiter().GetResult())
                    {
                        using (var imageFile = new FileStream(fileName, FileMode.Create))
                        {
                            await response.Content.CopyToAsync(imageFile);
                            _logger.WriteLine($"Image from : {url} was downloaded");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to load the image: {ex.Message}");
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
