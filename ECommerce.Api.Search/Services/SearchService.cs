using ECommerce.Api.Search.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;
        private readonly ILogger<SearchService> logger;

        public SearchService(IOrdersService ordersService, IProductsService productsService, ICustomersService customersService, ILogger<SearchService> logger)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customersService = customersService;
            this.logger = logger;
        }
        public async Task<(bool isSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var ordersResult = await ordersService.GetOrdersAsync(customerId);
            var productsResult = await productsService.GetProductsAsync();
            var customersResult = await customersService.GetCustomerAsync(customerId);

            if (ordersResult.isSuccess)
            {
                foreach(var order in ordersResult.Orders)
                {
                    foreach(var item in order.Items)
                    {
                        item.ProductName = productsResult.isSuccess ? productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name : "Product information is not availble";
                    }
                }

                var result = new
                {
                    Customer = customersResult.isSuccess ? customersResult.Customer : new { Name = "Customer information not available"}, 
                    Orders = ordersResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
