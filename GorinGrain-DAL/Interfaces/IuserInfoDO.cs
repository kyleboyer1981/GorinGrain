
namespace GorinGrain_DAL.Interfaces
{
   public interface IuserInfoDO
    {
        int UserID { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        int UserLevel { get; set; }
    }
}
