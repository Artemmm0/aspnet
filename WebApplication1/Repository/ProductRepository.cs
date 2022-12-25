using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<Product>> GetAllProductsAsync(Guid companyId, bool trackChanges)
        {
            return await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                .ToListAsync();
        }

        public IEnumerable<Product> GetByIds(Guid companyId, IEnumerable<Guid> ids, bool trackChanges)
        {
            return FindByCondition(x => ids.Contains(x.Id) && x.CompanyId.Equals(companyId),
                trackChanges).ToList();
        }

        public async Task<IEnumerable<Product>> GetByIdsAsync(Guid companyId, IEnumerable<Guid> ids, bool trackChanges)
        {
            return await FindByCondition(x => ids.Contains(x.Id) && x.CompanyId.Equals(companyId),
                trackChanges).ToListAsync();
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

        public async Task<Product> GetProductAsync(Guid id, bool trackChanges)
        {
            return await FindByCondition(e => e.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync();
        }

        public async Task<Product> GetProductAsync(Guid companyId, Guid id, bool trackChanges)
        {
            return await FindByCondition(e => e.Id.Equals(id) && e.CompanyId.Equals(companyId), trackChanges)
                .SingleOrDefaultAsync();
        }
    }
}
