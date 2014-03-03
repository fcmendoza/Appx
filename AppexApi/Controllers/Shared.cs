﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dropbox.Api;
using OAuthProtocol;

namespace AppexApi.Controllers {
    public class Shared {
        public Shared() {
            _consumerKey    = System.Configuration.ConfigurationManager.AppSettings["AppKey"];
            _consumerSecret = System.Configuration.ConfigurationManager.AppSettings["AppSecret"];
            _oauthToken     = System.Configuration.ConfigurationManager.AppSettings["OAuthToken"];
        }

        public DropboxApi GetDropBoxApiInstance() {
            var accessToken = new OAuthToken(token: _oauthToken.Substring(0, 16), secret: _oauthToken.Substring(18, 15));
            return new DropboxApi(_consumerKey, _consumerSecret, accessToken);
        }

        public string GetTextFromFile(string filename, string directory = "Books") {
            var accessToken = new OAuthToken(token: _oauthToken.Substring(0, 16), secret: _oauthToken.Substring(18, 15));
            var api = new DropboxApi(_consumerKey, _consumerSecret, accessToken);
            var file = api.DownloadFile(root: "dropbox", path: String.Format("{0}/{1}", directory, filename));
            return file.Text;
        }

        private string _consumerKey;
        private string _consumerSecret;
        private string _oauthToken;
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