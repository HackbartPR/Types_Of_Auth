using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class JwtApi
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonProperty("expiryTime")]
        public DateTime ExpiryTime { get; set; }
    }
}
