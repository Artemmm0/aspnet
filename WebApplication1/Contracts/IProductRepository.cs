using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts(Guid companyId, bool trackChanges);
        Product GetProduct(Guid companyId, Guid id, bool trackChanges);
        Product GetProduct(Guid id, bool trackChanges);
        void CreateProduct(Product product);

        IEnumerable<Product> GetByIds(Guid companyId, IEnumerable<Guid> ids, bool trackChanges);

        void DeleteProduct(Product product);
    }
}
