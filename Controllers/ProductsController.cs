using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hero_Project.Data;
using Hero_Project.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hero_Project.Controllers
{
    [ApiController]
    [Route("[controller]")] //...localhost:5001/products (dev)
    public class ProductsController : ControllerBase
    {
        private readonly DatabaseContext databaseContext;
        public ProductsController(DatabaseContext databaseContext) => this.databaseContext = databaseContext;

        [HttpGet] //localhost:5001/products
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
           return databaseContext.Products.OrderByDescending(p => p.ProductId).ToList();
        }

        [HttpGet("{id}")] //localhost:5001/products/123
        public ActionResult<Product> GetProductById(int id) {
            var result = databaseContext.Products.Find(id);
            if (result == null) {
                return NotFound();
            } else {
                return result;
            }

        }

        [HttpGet("search")] //localhost:5001/products/search?name=iWatch
        public ActionResult<IEnumerable<Product>> SearchProducts([FromQuery] string name = "") {
            var result = databaseContext.Products
                         .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                          .ToList();
            return result;
        }

        [HttpPost] //localhost:5001/products (json form)
        public ActionResult<Product> AddProduct([FromBody] Product model) {
            databaseContext.Products.Add(model);
            databaseContext.SaveChanges();
            return StatusCode((int) HttpStatusCode.Created);
        }
        
        [HttpPut("{id}")] //localhost:5001/products/123
        public ActionResult<Product> UpdateProduct(int id,[FromForm] Product model ) {
            if (id != model.ProductId){
                return BadRequest(); //return status 400
            }

            var result = databaseContext.Products.Find(id);

            if (result == null){
                return NotFound(); //return status 404
            }

            result.Name = model.Name;
            result.Price = model.Price;
            result.Stock = model.Stock;
            result.CategoryId = model.CategoryId;
            //Auto Mapper
            //Mapster
            databaseContext.Products.Update(result);
            databaseContext.SaveChanges();

            return NoContent(); //return model
        }

        [HttpDelete("{id}")] //localhost:5001/products/123
        public ActionResult<Product> DeleteProduct(int id)
        {
           if (id != 1150) {
               return NotFound();
           }
           return NoContent();
        }
        
    }
}
