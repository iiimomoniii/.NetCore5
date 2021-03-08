using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Hero_Project.NetCore5.DTOs.Products
{
    public class ProductRequest
    {
        //validate ProductId
        //ProductId to be able to null
        public int? ProductId { get; set; }
        //validate Name
        //require Name
        [RequiredAttribute]
        //max lenght of Name
        [MaxLength(100, ErrorMessage = "Name, maximum length 100")]
        public string Name { get; set; }

        //validate Stock
        //require stock and range since 0 to 10000
        [Range(0,10000)]
        public int Stock { get; set; }
        //validate Price
        //require price since 0 to 1 million
        [Range(0, 1_000_000)]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        //Upload Images
        public List<IFormFile> FormFiles { get; set; }

    }
}