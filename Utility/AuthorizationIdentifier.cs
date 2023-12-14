using System;
using Newtonsoft.Json;

namespace HealthyLife.Utility
{
    public class AuthorizationIdentifier
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }
        
        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }
    }
}