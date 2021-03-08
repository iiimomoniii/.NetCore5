using System.Diagnostics;
using Hero_Project.Entities;
namespace Hero_Project.NetCore5.DTOs.Products
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }

        //Mapping for get Name of Category in Category DB by Join DB
        public string CategoryName { get; set; }

        //Manual Join Value between Product and Category
        public static ProductResponse FromProduct(Product product){
            return new ProductResponse{
                ProductId = product.ProductId,
                Name = product.Name,
                Image = product.Image,
                Stock = product.Stock,
                Price = product.Price,
                CategoryName = product.Category.Name
            };
        }
    }
}