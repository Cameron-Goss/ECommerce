using ECommerce.Api.Customers.Db;
using ECommerce.Api.Search.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<CustomersService> logger;

        public CustomersService(IHttpClientFactory httpClientFactory, ILogger<CustomersService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<(bool isSuccess, dynamic Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                using(var client = this.httpClientFactory.CreateClient("CustomersService"))
                {
                    logger?.LogInformation("Querying Customers Service");
                    var response = await client.GetAsync($"api/customers/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsByteArrayAsync();
                        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                        var result = JsonSerializer.Deserialize<Customer>(content, options);

                        logger?.LogInformation("Customer found");
                        return (true, result, null);
                    }
                    return (false, null, response.ReasonPhrase);
                }                
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
