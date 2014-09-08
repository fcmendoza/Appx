using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Dropbox.Api;
using OAuthProtocol;

namespace AppexApi.Controllers {
    public class Shared {
        public Shared(IContentRepository repo) {
            _repo = repo;
        }

        public IEnumerable<FileInfo> GetFiles(string directory) {
            return _repo.GetFiles(directory);
        }

        /// <summary>
        /// If the timeout is not provided or less than zero, the result will be cached by default. Set the timeout to zero to not get the result from cache.
        /// When greater than zero the result will be cached for the time specified.
        /// </summary>
        public string GetTextFromFile(string filename, string directory = "Books", int cacheTimeoutInSeconds = -1) {

            int timeout = int.TryParse(ConfigurationManager.AppSettings["NotesCacheTimeoutInSeconds"], out timeout) ? timeout : 15; // default timeout
            timeout = cacheTimeoutInSeconds >= 0 ? cacheTimeoutInSeconds : timeout;

            return timeout > 0 
                ? GetFileContentFromCache(filename, directory, timeout)
                : _repo.GetTextContent(filename: filename, directory: directory);
        }

        private string GetFileContentFromCache(string filename, string directory, int cacheTimeoutInSeconds) {
            string key = String.Format("{0}_{1}", directory, filename);

            int timeout = cacheTimeoutInSeconds;

            var content = HttpRuntime
                .Cache.GetOrStore<String> (
                    key:        key,
                    expiration: new TimeSpan(0, 0, timeout),
                    generator:  () => _repo.GetTextContent(filename: filename, directory: directory)
                );

            return content;
        }

        IContentRepository _repo;
    }

    public static class CacheExtensions {
        // From http://stackoverflow.com/questions/445050/how-can-i-cache-objects-in-asp-net-mvc
        public static T GetOrStore<T>(this System.Web.Caching.Cache cache, string key, TimeSpan expiration, Func<T> generator) {
            var result = cache.Get(key);

            if (result == null) {
                result = generator();
                cache.Insert(
                    key: key, 
                    value: result, 
                    dependencies: null, 
                    absoluteExpiration: System.Web.Caching.Cache.NoAbsoluteExpiration, 
                    slidingExpiration: expiration
                );
            }
            return (T)result;
        }
    }

    public static class TimeZoneHelper {
        public static DateTime UtcToPacific(DateTime utcDate) {
            utcDate = utcDate.Kind != DateTimeKind.Utc ?  DateTime.SpecifyKind(utcDate, kind: DateTimeKind.Utc) : utcDate; // in case the date is indeed UTC but for some reason its Kind type is not.
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneName.Pacific.ToString());
            var datetime = TimeZoneInfo.ConvertTimeFromUtc(utcDate, timezone); // it automatically takes care of daylight saving time            
            return datetime;
        }

        public static DateTime UtcToCentral(DateTime utcDate) {
            utcDate = utcDate.Kind != DateTimeKind.Utc ? DateTime.SpecifyKind(utcDate, kind: DateTimeKind.Utc) : utcDate; // in case the date is indeed UTC but for some reason its Kind type is not.
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneName.Central.ToString()); 
            var datetime = TimeZoneInfo.ConvertTimeFromUtc(utcDate, timezone); // it automatically takes care of daylight saving time            
            return datetime;
        }
    }

    public sealed class TimeZoneName {
        // From http://stackoverflow.com/questions/424366/c-sharp-string-enums
        private readonly String name;
        private readonly int value;

        // "Pacific Standard Time" it's the ID regardless of daylight saving time (i.e. no matters if right now is daylight saving time, the 'ID name' 
        // remains the same). Same for the rest of the time zone names.
        public static readonly TimeZoneName Pacific = new TimeZoneName(1, "Pacific Standard Time");
        public static readonly TimeZoneName Central = new TimeZoneName(2, "Central Standard Time"); 
        public static readonly TimeZoneName Mountain = new TimeZoneName(3, "Mountain Standard Time");

        private TimeZoneName(int value, String name) {
            this.name = name;
            this.value = value;
        }

        public override String ToString() {
            return name;
        }
    }
}