
using System.ComponentModel.DataAnnotations;

namespace GorinGrain.Models
{
    public class LocationPO
    {
        public long LocationID { get; set; }

        [Required]
        [Display(Name = "Facility Name")]
        public string LocationName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        public long MaxCapacity { get; set; }

    }
}