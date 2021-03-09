using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hero_Project.Data;
using Hero_Project.Entities;
using Hero_Project.NetCore5.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hero_Project.NetCore5.Services
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IUploadFileService uploadFileService;
        public ProductService(DatabaseContext databaseContext, IUploadFileService uploadFileService) {
            this.databaseContext = databaseContext;
            this.uploadFileService = uploadFileService; 
        }
        public async Task<IEnumerable<Product>> FindAll()
        {
            // Products.Include (p => p.category) joining data between Products and Category
            return await databaseContext.Products.Include(p => p.Category)
                        .OrderByDescending(p => p.ProductId)
                        .ToListAsync();
        }

        public async Task<Product> FindById(int id)
        {
            // Products.Include (p => p.category) joining data between Products and Category
            return await databaseContext.Products.Include(p => p.Category)
                        //SingleOrDefault is WHERE in SQL if more and less then id will retern is null
                        //p => p.ProductId == id is return ONLY one productId is equal id
                        .SingleOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<Product>> Search(string name)
        {
             // Products.Include (p => p.category) joining data between Products and Category
            return await databaseContext.Products.Include(p => p.Category)
                         .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                         .ToListAsync();
        }
        public async Task Create(Product product)
        {
            databaseContext.Products.Add(product);
            await databaseContext.SaveChangesAsync();
        }

        public async Task Update(Product product)
        {
            databaseContext.Products.Update(product);
            await databaseContext.SaveChangesAsync();
            
        }

        public async Task Delete(Product product)
        {
            databaseContext.Products.Remove(product);
            await databaseContext.SaveChangesAsync();
        }
        public async Task<(string errorMessage, string imageName)> UploadImage(List<IFormFile> formFiles)
        {
            String errorMessage = String.Empty;
            String imageName = String.Empty;
            if (uploadFileService.IsUpload(formFiles)) {
                errorMessage = uploadFileService.Validation(formFiles);
                if (String.IsNullOrEmpty(errorMessage)) {
                    imageName = (await uploadFileService.UploadImages(formFiles))[0];
                }
            }
            return (errorMessage, imageName);
        }
    }
}