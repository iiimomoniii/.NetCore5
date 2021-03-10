using System.Threading.Tasks;
using Hero_Project.Entities;

namespace Hero_Project.NetCore5.Interfaces
{
    public interface IAccountService
    {
         Task Register(Account account);
         Task Login (string username, string password);
    }
}