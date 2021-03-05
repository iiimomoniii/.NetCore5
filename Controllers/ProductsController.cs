using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hero_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hero_Project.Controllers
{
    [ApiController]
    [Route("[controller]")] //...localhost:5001/products (dev)
    public class ProductsController : ControllerBase
    {
        public ProductsController(DatabaseContext databaseContext)
        {
            var result = databaseContext.Products.ToList();
            var size = result.Count();
        }

        [HttpGet] //localhost:5001/products
        public ActionResult<List<string>> GetProducts()
        {
            var products = new List<string>();
            products.Add("iMac");
            products.Add("iPhone");
            return products;
        }

        [HttpGet("{id}")] //localhost:5001/products/123
        public ActionResult GetProductById(int id) => Ok(new { productId = id, name = "iPod"});

        [HttpGet("search")] //localhost:5001/products/search?name=iWatch
        public ActionResult SearchProducts([FromQuery] string name) =>  Ok(new { productId = 111, name = name});

        [HttpPost] //localhost:5001/products (json form)
        public ActionResult<Product> AddProduct([FromBody] Product model) {
            return CreatedAtAction(nameof(GetProductById), new {id = 111} , model); //return status 201
        }
        
        [HttpPut("{id}")] //localhost:5001/products/123
        public ActionResult<Product> UpdateProduct(int id,[FromForm] Product model ) {
            if (id != model.id){
                return BadRequest(); //return status 400
            }
            if (id != 1150){
                return NotFound(); //return status 404
            }
            return model; //return model
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

    //create Model for Product
    public class Product {
        public int id { get; set;}
        public string name {get; set;}
        public int price {get;set;}
    }
}
