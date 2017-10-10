using GorinGrain_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorinGrain_DAL.Models
{
    public class ShipmentDO : IShipmentInfoDO 
    {
        public long ShipmentID { get; set; }

        public long LocationID { get; set; }

        public string Product { get; set; }

        public long ProducerID { get; set; }

        public long QuantityInBu { get; set; }

        public decimal PricePerBushel { get; set; }
    }
}
