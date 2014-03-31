using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OAuthProtocol;

namespace Dropbox.Api {
    public class DropboxApi {
        private readonly OAuthToken _accessToken;
        private readonly string _consumerKey;
        private readonly string _consumerSecret;

        public DropboxApi(string consumerKey, string consumerSecret, OAuthToken accessToken) {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _accessToken = accessToken;
        }

        private string GetResponse(Uri uri) {
            var oauth = new OAuth();
            var requestUri = oauth.SignRequest(uri, _consumerKey, _consumerSecret, _accessToken);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = WebRequestMethods.Http.Get;
            var response = request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            return reader.ReadToEnd();
        }

        private static T ParseJson<T>(string json) where T : class, new() {
            var jobject = JObject.Parse(json);
            return JsonConvert.DeserializeObject<T>(jobject.ToString());
        }

        public Account GetAccountInfo() {
            var uri = new Uri(new Uri(DropboxRestApi.BaseUri), "account/info");
            var json = GetResponse(uri);
            return ParseJson<Account>(json);
        }

        public File GetFiles(string root, string path) {
            var uri = new Uri(new Uri(DropboxRestApi.BaseUri), String.Format("metadata/{0}/{1}", root, path));
            var json = GetResponse(uri);
            return ParseJson<File>(json);
        }

        public FileSystemInfo CreateFolder(string root, string path) {
            var uri = new Uri(new Uri(DropboxRestApi.BaseUri), String.Format("fileops/create_folder?root={0}&path={1}",
                root, UpperCaseUrlEncode(path)));

            var json = GetResponse(uri);
            return ParseJson<FileSystemInfo>(json);
        }

        public FileSystemInfo DownloadFile(string root, string path) {
            var uri = new Uri(String.Format("https://api-content.dropbox.com/1/files/{0}/{1}", root, path));

            var oauth = new OAuth();
            var requestUri = oauth.SignRequest(uri, _consumerKey, _consumerSecret, _accessToken);

            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = WebRequestMethods.Http.Get;
            var response = request.GetResponse();

            var metadata = response.Headers["x-dropbox-metadata"];
            var file = ParseJson<FileSystemInfo>(metadata);

            using (Stream responseStream = response.GetResponseStream())
            using (MemoryStream memoryStream = new MemoryStream()) {
                byte[] buffer = new byte[1024];
                int bytesRead;
                do {
                    bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                    memoryStream.Write(buffer, 0, bytesRead);
                } while (bytesRead > 0);

                file.Data = memoryStream.ToArray();

                if (response.Headers["Content-Type"] != null && response.Headers["Content-Type"].ToUpper().Contains("UTF-8")) {
                    file.Text = System.Text.Encoding.UTF8.GetString(file.Data);
                }
                else {
                    file.Text = System.Text.Encoding.Default.GetString(file.Data);
                }
            }

            return file;
        }

        private static string UpperCaseUrlEncode(string s) {
            char[] temp = HttpUtility.UrlEncode(s).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++) {
                if (temp[i] == '%') {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }
            return new string(temp);
        }
    }

    public class DropboxRestApi {
        public const string ApiVersion = "1";
        public const string BaseUri = "https://api.dropbox.com/" + ApiVersion + "/";
        public const string AuthorizeBaseUri = "https://www.dropbox.com/" + ApiVersion + "/";
        public const string ApiContentServer = "https://api-content.dropbox.com/1/files/{0}/{1}";
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Account {
        [JsonProperty(PropertyName = "uid")]
        public int Id { get; internal set; }

        [JsonProperty(PropertyName = "referral_link")]
        public string ReferralLink { get; internal set; }

        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; internal set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; internal set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; internal set; }

        [JsonProperty(PropertyName = "quota_info")]
        public Quota Quota { get; internal set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class File {
        [JsonProperty(PropertyName = "size")]
        public string Size { get; internal set; }

        [JsonProperty(PropertyName = "rev")]
        public string Revision { get; internal set; }

        [JsonProperty(PropertyName = "thumb_exists")]
        public bool ThumbnailExists { get; internal set; }

        [JsonProperty(PropertyName = "bytes")]
        public long Bytes { get; internal set; }

        [JsonProperty(PropertyName = "is_dir")]
        public bool IsDirectory { get; internal set; }

        [JsonProperty(PropertyName = "root")]
        public string Root { get; internal set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; internal set; }

        [JsonProperty(PropertyName = "mime_type")]
        public string MimeType { get; internal set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; internal set; }

        [JsonProperty(PropertyName = "contents")]
        public IEnumerable<File> Contents { get; internal set; }

        [JsonProperty(PropertyName = "modified")]
        public DateTimeOffset Modified { get; internal set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Quota {
        [JsonProperty(PropertyName = "quota")]
        public long Total { get; internal set; }

        [JsonProperty(PropertyName = "shared")]
        public long Shared { get; internal set; }

        [JsonProperty(PropertyName = "normal")]
        public long Normal { get; internal set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class FileSystemInfo {
        [JsonProperty(PropertyName = "size")]
        public string Size { get; internal set; }

        [JsonProperty(PropertyName = "rev")]
        public string Revision { get; internal set; }

        [JsonProperty(PropertyName = "thumb_exists")]
        public bool ThumbnailExists { get; internal set; }

        [JsonProperty(PropertyName = "bytes")]
        public long Bytes { get; internal set; }

        [JsonProperty(PropertyName = "modified")]
        public DateTime Modified { get; internal set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; internal set; }

        [JsonProperty(PropertyName = "is_dir")]
        public bool IsDirectory { get; internal set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; internal set; }

        [JsonProperty(PropertyName = "root")]
        public string Root { get; internal set; }

        [JsonProperty(PropertyName = "is_deleted")]
        public bool IsDeleted { get; internal set; }

        public byte[] Data { get; internal set; }

        public String Text { get; internal set; }

        public void Save(string path) {
            using (var fileStream = new FileStream(
                path, FileMode.Create, FileAccess.ReadWrite)) {
                fileStream.Write(Data, 0, Data.Length);
            }
        }
    }
}
