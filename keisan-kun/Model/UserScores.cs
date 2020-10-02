using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace keisan_kun.Model
{
    [JsonObject("UserScores")]
    public class UserScores
    {
        [JsonProperty("Scores")]
        public List<UserModel> m_Users { get; set; } = new List<UserModel>();
    }
}
