using Newtonsoft.Json;

namespace DemoRestSharp.Models
{
    public class Comment
    {
        [JsonProperty("postId")]
        public long PostId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
