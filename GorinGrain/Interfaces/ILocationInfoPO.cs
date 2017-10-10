
namespace GorinGrain.Interfaces
{
    public interface ILocationInfoPO
    {
        long LocationID { get; set; }

        string LocationName { get; set; }

        string Address { get; set; }

        string Phone { get; set; }

        long MaxCapacity { get; set; }
    }
}
