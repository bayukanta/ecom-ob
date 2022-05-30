using Ecom_Onboarding.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ecom_Onboarding.DTO
{
    public class PublisherDTO
    {
        public Guid? Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public List<Game> Game { get; set; }
    }
}
