using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorinGrain_BLL.Interfaces
{
    public interface IShipmentInfoBO
    {
        long ShipmentID { get; set; }

        long LocationID { get; set; }

        string Product { get; set; }

        long ProducerID { get; set; }

        long QuantityInBu { get; set; }

        decimal PricePerBushel { get; set; }
    }
}
