using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthProtocol {
    public class OAuthToken {
        public OAuthToken(string token, string secret) {
            Token = token;
            Secret = secret;
        }
        public string Token { get; private set; }
        public string Secret { get; private set; }
    }
}
