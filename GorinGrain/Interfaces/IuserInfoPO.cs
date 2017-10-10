
namespace GorinGrain.Interfaces
{
    public interface IuserInfoPO
    {
        int UserID { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        int UserLevel { get; set; }
    }
}
