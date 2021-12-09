using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectMangadex.Models
{
    public class User
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
