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
using Hero_Project.NetCore5.Interfaces;

namespace Hero_Project.Controllers
{
    [ApiController]
    [Route("[controller]")] //...localhost:5001/products (dev)
    public class ProductsController : ControllerBase
    {
     
        private readonly IProductService productService;
        public ProductsController(IProductService  productService) => this.productService = productService;

        [HttpGet] //localhost:5001/products
        //Return Values By ProductResponse
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts()
        {
           //use async function findAll and response data by DTO (ProductResponse.FromProduct) 
           return  (await productService.FindAll())
                    .Select(ProductResponse.FromProduct)
                    .ToList();
        }

        [HttpGet("{id}")] //localhost:5001/products/123
        public async Task<ActionResult<ProductResponse>> GetProductById(int id) {
           
           var product = await productService.FindById(id);
            if (product == null) {
                return NotFound();
            } else {
                //return product and map product to ProductResponse
                return product.Adapt<ProductResponse>();
            }

        }

        [HttpGet("search")] //localhost:5001/products/search?name=iWatch
        public async Task<ActionResult<IEnumerable<ProductResponse>>> SearchProducts([FromQuery] string name = "") {
            // Products.Include (p => p.category) joining data between Products and Category
            return  (await productService.Search(name))
                    .Select(ProductResponse.FromProduct)
                    .ToList();
        }

        [HttpPost] //localhost:5001/products (json form)
        public async Task<ActionResult<Product>> AddProduct([FromForm] ProductRequest productRequest) {
            
            //using mapster.7.1.5 map data from productRequest to product
            var product = productRequest.Adapt<Product>();
            await productService.Create(product);
            return StatusCode((int) HttpStatusCode.Created);
        }
        
        [HttpPut("{id}")] //localhost:5001/products/123
        public async Task<ActionResult<Product>> UpdateProduct(int id,[FromForm] ProductRequest productRequest) {
            if (id != productRequest.ProductId){
                return BadRequest(); //return status 400
            }

            //get product
            var product = await productService.FindById(id);

            if (product == null){
                return NotFound(); //return status 404
            }
            
            //map data usign mapster 7.1.5 between productRequest and result (source Adapt destination)
            productRequest.Adapt(product);
            await productService.Update(product);
            return NoContent(); //return model
        }

        [HttpDelete("{id}")] //localhost:5001/products/123
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await productService.FindById(id);
            if ( product == null) {
                return NotFound();
            }
            await productService.Delete(product);
            return NoContent();
        }
        
    }
}
