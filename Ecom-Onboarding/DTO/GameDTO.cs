using Newtonsoft.Json;
using System;

namespace Ecom_Onboarding.DTO
{
    public class GameDTO
    {
        public string Name { get; set; }
        public string Genre { get; set; }

        [JsonIgnore]
        public PublisherDTO Publisher { get; set; }
    }
}
