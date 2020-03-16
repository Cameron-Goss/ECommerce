using ECommerce.Api.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Interfaces
{
    public interface IOrdersService
    {
        Task<(bool isSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrderAsync(int customerId);
    }
}
