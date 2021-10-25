using RestWithAspNet5.Hypermedia;
using RestWithAspNet5.Hypermedia.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestWithAspNet5.Data.VO
{
    public class PersonVO : ISupportsHypermedia
    {
        [JsonPropertyName("Codigo")]
        public long Id { get; set; }
        [JsonPropertyName("Nome")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonPropertyName("Endereco")]
        public string Address { get; set; }
        [JsonIgnore]
        public string Gender { get; set; }
        public bool Enabled { get; set; }

        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}
