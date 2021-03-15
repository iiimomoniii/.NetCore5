using System.Threading.Tasks;
using Hero_Project.Entities;

namespace Hero_Project.NetCore5.Interfaces
{
    public interface IAccountService
    {
         Task Register(Account account);
         Task<Account> Login (string username, string password);

         string GenerateToken(Account account);

         Account GetInfo (string accessToken) ;
    }
}