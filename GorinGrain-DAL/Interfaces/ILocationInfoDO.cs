
namespace GorinGrain_DAL
{
     public interface ILocationInfoDO
    {
        long LocationID { get; set; }

        string LocationName { get; set; }

        string Address { get; set; }

        string Phone { get; set; }
        
        long MaxCapacity { get; set; }

    }
}
