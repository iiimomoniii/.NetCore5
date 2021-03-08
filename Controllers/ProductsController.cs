using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hero_Project.Data;
using Hero_Project.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Hero_Project.NetCore5.DTOs.Products;
using Mapster;

namespace Hero_Project.Controllers
{
    [ApiController]
    [Route("[controller]")] //...localhost:5001/products (dev)
    public class ProductsController : ControllerBase
    {
        private readonly DatabaseContext databaseContext;
        public ProductsController(DatabaseContext databaseContext) => this.databaseContext = databaseContext;

        [HttpGet] //localhost:5001/products
        //Return Values By ProductResponse
        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
        {
        // Products.Include (p => p.category) joining data between Products and Category
           return databaseContext.Products.Include(p => p.Category)
           .OrderByDescending(p => p.ProductId)
           // type of return data obj
           .Select(ProductResponse.FromProduct)
           .ToList();
        }

        [HttpGet("{id}")] //localhost:5001/products/123
        public ActionResult<ProductResponse> GetProductById(int id) {
            // Products.Include (p => p.category) joining data between Products and Category
            var result = databaseContext.Products.Include(p => p.Category)
            //SingleOrDefault is WHERE in SQL if more and less then id will retern is null
            //p => p.ProductId == id is return ONLY one productId is equal id
            .SingleOrDefault(p => p.ProductId == id);
            if (result == null) {
                return NotFound();
            } else {
                //retun Product Obj is productId is equal id
                return ProductResponse.FromProduct(result);
            }

        }

        [HttpGet("search")] //localhost:5001/products/search?name=iWatch
        public ActionResult<IEnumerable<ProductResponse>> SearchProducts([FromQuery] string name = "") {
            // Products.Include (p => p.category) joining data between Products and Category
            var result = databaseContext.Products.Include(p => p.Category)
                         .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                         //p => p.ProductId == id is return Name LIKE (SQL) name 
                         .Select(ProductResponse.FromProduct)
                         .ToList();
            return result;
        }

        [HttpPost] //localhost:5001/products (json form)
        public ActionResult<Product> AddProduct([FromForm] ProductRequest productRequest) {
            
            //using mapster.7.1.5 map data from productRequest to product
            var product = productRequest.Adapt<Product>();
            databaseContext.Products.Add(product);
            databaseContext.SaveChanges();
            return StatusCode((int) HttpStatusCode.Created);
        }
        
        [HttpPut("{id}")] //localhost:5001/products/123
        public ActionResult<Product> UpdateProduct(int id,[FromForm] ProductRequest productRequest) {
            if (id != productRequest.ProductId){
                return BadRequest(); //return status 400
            }

            var result = databaseContext.Products.Find(id);

            if (result == null){
                return NotFound(); //return status 404
            }
            
            //map data usign mapster 7.1.5 between productRequest and result (source Adapt destination)
            productRequest.Adapt(result);

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
