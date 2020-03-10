using AutoMapper;
using ECommerce.Api.Products.Database;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext dbContext;
        private readonly ILogger<ProductsProvider> logger;
        private readonly IMapper mapper;

        public ProductsProvider(ProductsDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Products.Any())
            {
                for (var i = 0; i < 5; i++)
                { 
                    dbContext.Products.Add(new Database.Product() { Id = i+1, Name = "Product " + i.ToString(), Price = i, Inventory = i });
                }

                //dbContext.Products.Add(new Database.Product() { Id = 1, Name = "Keyboard", Price = 75, Inventory = 25 });
                //dbContext.Products.Add(new Database.Product() { Id = 2, Name = "Mouse", Price = 40, Inventory = 60 });
                //dbContext.Products.Add(new Database.Product() { Id = 3, Name = "Monitor", Price = 150, Inventory = 10 });
                //dbContext.Products.Add(new Database.Product() { Id = 4, Name = "Mousepad", Price = 20, Inventory = 30 });
                //dbContext.Products.Add(new Database.Product() { Id = 5, Name = "Router", Price = 60, Inventory = 15 });

                dbContext.SaveChanges();
                
            }
        }

        public async Task<(bool isSuccess, IEnumerable<Models.Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await dbContext.Products.ToListAsync();
                if(products != null && products.Any())
                {
                    var result = mapper.Map<IEnumerable<Database.Product>, IEnumerable<Models.Product>>(products);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
