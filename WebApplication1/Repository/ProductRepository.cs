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

        public void CreateProduct(Product product)
        {
            Create(product);
        }

        public void DeleteProduct(Product product)
        {
            Delete(product);
        }

        public IEnumerable<Product> GetAllProducts(Guid companyId, bool trackChanges)
        {
            return FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                .ToList();
        }

        public IEnumerable<Product> GetByIds(Guid companyId, IEnumerable<Guid> ids, bool trackChanges)
        {
            return FindByCondition(x => ids.Contains(x.Id) && x.CompanyId.Equals(companyId),
                trackChanges).ToList();
        }

        public Product GetProduct(Guid companyId, Guid id, bool trackChanges)
        {
            return FindByCondition(e => e.Id.Equals(id) && e.CompanyId.Equals(companyId), trackChanges)
                .SingleOrDefault();
        }

        public Product GetProduct(Guid id, bool trackChanges)
        {
            return FindByCondition(e => e.Id.Equals(id), trackChanges)
                .SingleOrDefault();
        }
    }
}
