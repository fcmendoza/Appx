using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Cryptography;
using System.Text;

namespace AppexApi.Controllers
{
    public class AuthenticationController : ApiController
    {
        [HttpGet]
        public User ValidateAndGetUser(string username, string password) {
            // first validate the credentials are valid
            bool isValid = true;

            // return found user or 404 otherwise.
            if (isValid) {
                return new User { Username = username, IsActive = true, IsAdmin = false };
            }
            else {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound) {
                    Content = new StringContent(string.Format("User with login '{0}' does not exist.", username)),
                    ReasonPhrase = "User Not Found"
                };
                throw new HttpResponseException(response);
            }
        }

        public bool ValidatePassword(string password) {
            var hashKey = System.Configuration.ConfigurationManager.AppSettings["HashKey"];
            return GetHashKey(password) == hashKey.ToString();
        }

        private string GetHashKey(string password) {
            HashAlgorithm algorithm = SHA1.Create();
            var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(password ?? String.Empty));
            return BitConverter.ToString(hash).Replace("-", String.Empty).ToLower();
        }

        [HttpGet]
        public IEnumerable<Role> GetUserRoles(string username) {
            return new List<Role> { new Role { Name = "Admin" } }; 
        } 
    }

    public class User {
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class Role {
        public string Name { get; set; }
    }

}
