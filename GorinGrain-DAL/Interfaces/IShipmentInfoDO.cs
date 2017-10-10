
namespace GorinGrain_DAL.Interfaces
{
    public interface IShipmentInfoDO
    {
        long ShipmentID { get; set; }

        long LocationID { get; set; }

        string Product { get; set; }

        long ProducerID { get; set; }

        long QuantityInBu { get; set; }

        decimal PricePerBushel { get; set; }
    }
}
