using Newtonsoft.Json;

namespace JSONKeyYouTubeID.Models
{
    public partial class Exercise
    {
        [JsonProperty("name")]
        private string? _name;
        [JsonProperty("force")]
        private string? _force;
        [JsonProperty("primaryMuscles")]
        private List<string>? _primaryMuscles;
        [JsonProperty("secondaryMuscles")]
        private List<string>? _secondaryMuscles;
        [JsonProperty("instructions")]
        private List<string>? _instructions;
        [JsonProperty("category")]
        private string? _category;
        [JsonProperty("id")]
        private string? _id;
        [JsonProperty("youtube_id")]
        private string? _youtube_id;

        [JsonIgnore]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [JsonIgnore]
        public string Force
        {
            get { return _force; }
            set { _force = value; }
        }

        [JsonIgnore]
        public List<string> PrimaryMuscles
        {
            get { return _primaryMuscles; }
            set { _primaryMuscles = value; }
        }

        [JsonIgnore]
        public List<string> SecondaryMuscles
        {
            get { return _secondaryMuscles; }
            set { _secondaryMuscles = value; }
        }

        [JsonIgnore]
        public List<string> Instructions
        {
            get { return _instructions; }
            set { _instructions = value; }
        }

        [JsonIgnore]
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        [JsonIgnore]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [JsonIgnore]
        public string YouTubeId
        {
            get { return _youtube_id; }
            set { _youtube_id = value; }
        }
    }

}
