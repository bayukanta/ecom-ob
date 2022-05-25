using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Ecom_Onboarding.Models;
using Newtonsoft.Json;

namespace Ecom_Onboarding.Models
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Genre { get; set; }
        [JsonIgnore]
        public Publisher Publisher { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; }

        public Game()
        {
            CreatedDate = DateTime.UtcNow;
        }

    }
}
