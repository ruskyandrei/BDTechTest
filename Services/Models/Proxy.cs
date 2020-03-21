using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class Proxy
    {
        [JsonProperty("ip")]
        public string Host { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }
    }
}
