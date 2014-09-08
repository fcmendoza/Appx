using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dropbox.Api;
using OAuthProtocol;

namespace AppexApi.Controllers {
    public interface IContentRepository {
        string GetTextContent(string filename, string directory);
        IEnumerable<FileInfo> GetFiles(string directory);
    }

    public class DropboxContentRepository : IContentRepository {
        public DropboxContentRepository() {
            _consumerKey    = System.Configuration.ConfigurationManager.AppSettings["AppKey"];
            _consumerSecret = System.Configuration.ConfigurationManager.AppSettings["AppSecret"];
            _oauthToken     = System.Configuration.ConfigurationManager.AppSettings["OAuthToken"];
        }

        public string GetTextContent(string filename, string directory) {
            var accessToken = new OAuthToken(token: _oauthToken.Substring(0, 16), secret: _oauthToken.Substring(18, 15));
            var api = new DropboxApi(_consumerKey, _consumerSecret, accessToken);
            var file = api.DownloadFile(root: "dropbox", path: String.Format("{0}/{1}", directory, filename));
            return file.Text;
        }

        public IEnumerable<FileInfo> GetFiles(string directory) {
            var accessToken = new OAuthToken(token: _oauthToken.Substring(0, 16), secret: _oauthToken.Substring(18, 15));
            var api = new DropboxApi(_consumerKey, _consumerSecret, accessToken);
            var files = api.GetFiles(root: "dropbox", path: directory);

            var dasFiles = files.Contents.Select(x => new FileInfo { Bytes = x.Bytes, 
                Icon            = x.Icon, 
                IsDirectory     = x.IsDirectory, 
                MimeType        = x.MimeType, 
                Modified        = x.Modified, 
                Path            = x.Path, 
                Revision        = x.Revision, 
                Root            = x.Root, 
                Size            = x.Size, 
                ThumbnailExists = x.ThumbnailExists 
            });

            return dasFiles;
        }

        private string _consumerKey;
        private string _consumerSecret;
        private string _oauthToken;
    }

    public class LocalContentRepository : IContentRepository {
        public LocalContentRepository(string directory) {
            if (String.IsNullOrWhiteSpace(directory)) {
                throw new ArgumentException("Missing 'directory' argument", "directory");
            }

            _localPath = directory;
        }
        public string GetTextContent(string filename, string directory) {
            directory = !String.IsNullOrWhiteSpace(directory) ? directory : _localPath;
            string text = System.IO.File.ReadAllText(System.IO.Path.Combine(directory, filename));
            return text;
        }

        public IEnumerable<FileInfo> GetFiles(string directory) {
            directory = !String.IsNullOrWhiteSpace(directory) ? directory : _localPath;
            var dir = new System.IO.DirectoryInfo(directory);
            var files = dir.GetFiles("*.txt");

            var dasFiles = files.Select(x => new FileInfo {
                Modified = x.LastWriteTime,
                Path = x.Name
            });

            return dasFiles;
        }

        private string _localPath;
    }

    public class FileInfo {
        public string Size { get; internal set; }
        public string Revision { get; internal set; }
        public bool ThumbnailExists { get; internal set; }
        public long Bytes { get; internal set; }
        public bool IsDirectory { get; internal set; }
        public string Root { get; internal set; }
        public string Icon { get; internal set; }
        public string MimeType { get; internal set; }
        public string Path { get; internal set; }
        public DateTimeOffset Modified { get; internal set; }
    }
}