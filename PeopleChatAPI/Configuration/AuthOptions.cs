using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PeopleChatAPI.Configuration
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string EncryptionKey { get; set; }
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key) =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    }
}
