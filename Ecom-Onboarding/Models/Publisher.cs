using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Ecom_Onboarding.Models;

namespace Ecom_Onboarding.Models
{
    public class Publisher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public List<Game> Games { get; set; }

        public DateTime CreatedDate { get; }

        public Publisher()
        {
            CreatedDate = DateTime.UtcNow;
        }

    }
}
