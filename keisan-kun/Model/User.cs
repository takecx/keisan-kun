using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace keisan_kun.Model
{
    [JsonObject("user")]
    public class UserModel
    {
        [JsonProperty("id")]
        public int m_UserID { get; set; }
        [JsonProperty("name")]
        public string m_Username { get; set; }
        [JsonProperty("Scores")]
        public List<Score> m_Scores { get; set; } = new List<Score>();
    }
}
