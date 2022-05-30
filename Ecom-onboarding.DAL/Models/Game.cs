using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ecom_Onboarding.DAL.Models;

namespace Ecom_Onboarding.DAL.Models
{
    public class Game
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Key]
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
