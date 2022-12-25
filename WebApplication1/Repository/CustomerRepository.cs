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
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public void CreateCustomer(Customer customer)
        {
            Create(customer);
        }

        public void DeleteCustomer(Customer customer)
        {
            Delete(customer);
        }

        public IEnumerable<Customer> GetAllCustomers(Guid productId, bool trackChanges)
        {
            return FindByCondition(e => e.ProductId.Equals(productId), trackChanges)
                .OrderBy(e => e.Name);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync(Guid productId, bool trackChanges)
        {
            return await FindByCondition(e => e.ProductId.Equals(productId), trackChanges)
                .OrderBy(e => e.Name).ToListAsync();
        }

        public IEnumerable<Customer> GetByIds(Guid productId, IEnumerable<Guid> ids, bool trackChanges)
        {
            return FindByCondition(x => ids.Contains(x.Id) && x.ProductId.Equals(productId),
                trackChanges).ToList();
        }

        public async Task<IEnumerable<Customer>> GetByIdsAsync(Guid productId, IEnumerable<Guid> ids, bool trackChanges)
        {
            return await FindByCondition(x => ids.Contains(x.Id) && x.ProductId.Equals(productId),
                trackChanges).ToListAsync();
        }

        public Customer GetCustomer(Guid productId, Guid customerId, bool trackChanges)
        {
            return FindByCondition(e => e.ProductId.Equals(productId) && e.Id.Equals(customerId),
                trackChanges).SingleOrDefault();
        }

        public async Task<Customer> GetCustomerAsync(Guid productId, Guid customerId, bool trackChanges)
        {
            return await FindByCondition(e => e.ProductId.Equals(productId) && e.Id.Equals(customerId),
                trackChanges).SingleOrDefaultAsync();
        }
    }
}
