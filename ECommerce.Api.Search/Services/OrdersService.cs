using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<OrdersService> logger;

        public OrdersService(IHttpClientFactory httpClientFactory, ILogger<OrdersService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }
        public async Task<(bool isSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                using (var client = this.httpClientFactory.CreateClient("OrdersService"))
                {
                    logger?.LogInformation("Querying Orders Service");
                    var response = await client.GetAsync($"api/orders/{customerId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsByteArrayAsync();
                        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                        var result = JsonSerializer.Deserialize<IEnumerable<Order>>(content, options);

                        logger?.LogInformation($"{result.Count()} order(s) found");
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
