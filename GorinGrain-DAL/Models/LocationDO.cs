using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorinGrain_DAL.Models
{
    public class LocationDO : ILocationInfoDO
    {
        public long LocationID { get; set; }

        public string LocationName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public long MaxCapacity { get; set; }

    }

}
