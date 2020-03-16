using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Interfaces
{
    public interface ISearchService
    {
        Task<(bool isSuccess, dynamic SearchResults)> SearchAsync(int customerId);
    }
}
