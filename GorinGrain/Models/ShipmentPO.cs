using GorinGrain_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GorinGrain.Models
{
    public class ShipmentPO
    {
        
        public long ShipmentID { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "The location is a required field.")]
        public long LocationID { get; set; }

        //need select list for dropdowns
        public SelectList LocationDDL { get; set; }

        [Required(ErrorMessage ="Please enter the name of the commodity")]
        public string Product { get; set; }
        
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "The producer is a required field.")]
        public long ProducerID { get; set; }
        
        //need select list for dropdowns
        public SelectList ProducerDDL{ get; set; }
        
        [Required]
        [Range(0, int.MaxValue)]
        public long QuantityInBu { get; set; }

        [Required]
        public decimal PricePerBushel { get; set; }

        
    }
}