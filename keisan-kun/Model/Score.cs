using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace keisan_kun.Model
{
    [JsonObject("Score")]
    public class Score
    {
        [JsonProperty("operator")]
        public string m_Operator { get; set; }
        [JsonProperty("total_challenge")]
        public int m_TotalChallenge { get; set; }
        [JsonProperty("correct")]
        public int m_Correct { get; set; }
        [JsonProperty("incorrect")]
        public int m_Incorrect { get; set; }

    }
}
