using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GorinGrain.Models
{
    public class ProducerPO
    {
        public long ProducerID { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public string ContactName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }
    }
}