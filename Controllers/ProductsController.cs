using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hero_Project.Controllers
{
    [ApiController]
    [Route("[controller]")] //...localhost:5001/products (dev)
    public class ProductsController : ControllerBase
    {
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
        
    }
}
