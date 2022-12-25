using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public IEnumerable<Product> GetAllProducts(bool trackChanges)
        {
            return FindAll(trackChanges).OrderBy(c => c.Name).ToList();
        }

        public Product GetProduct(Guid id, bool trackChanges)
        {
            return FindByCondition(e => e.Id.Equals(id), trackChanges)
                .SingleOrDefault();
        }
    }
}
