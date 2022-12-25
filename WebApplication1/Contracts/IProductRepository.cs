using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts(bool trackChanges);
        Product GetProduct(Guid id, bool trackChanges);
    }
}
