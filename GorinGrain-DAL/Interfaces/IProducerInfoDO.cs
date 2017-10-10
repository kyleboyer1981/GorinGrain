
namespace GorinGrain_DAL.Interfaces
{
   public  interface IProducerInfoDO
    {
        long ProducerID { get; set; }

        string CompanyName { get; set; }

        string ContactName { get; set; }

        string Phone { get; set; }

        string Address { get; set; }


    }
}
