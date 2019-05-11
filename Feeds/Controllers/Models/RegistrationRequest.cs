using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds.Controllers.Models
{
    public class RegistrationRequest
    {
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [Required]
        [JsonProperty(PropertyName = "passwordHash")]
        public string PasswordHash { get; set; }

        [Required]
        [JsonProperty(PropertyName = "asAdmin")]
        public bool AsAdmin { get; set; }
        
    }
}
