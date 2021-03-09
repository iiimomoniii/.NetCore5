using System.Collections.Generic;
using System.Threading.Tasks;
using Hero_Project.Entities;

namespace Hero_Project.NetCore5.Interfaces
{
    public interface IProductService
    {
        //use Task because run method by async
         Task<IEnumerable<Product>> FindAll();
         
         Task<Product> FindById(int id);

         Task Create(Product product);

         Task Update(Product product);

         Task Delete(Product product);

         Task<IEnumerable<Product>> Search(string name);
    }
}