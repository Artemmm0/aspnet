using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public IEnumerable<Customer> GetAllCustomers(Guid productId, bool trackChanges)
        {
            return FindByCondition(e => e.ProductId.Equals(productId), trackChanges)
                .OrderBy(e => e.Name);
        }

        public Customer GetCustomer(Guid productId, Guid customerId, bool trackChanges)
        {
            return FindByCondition(e => e.ProductId.Equals(productId) && e.Id.Equals(customerId),
                trackChanges).SingleOrDefault();
        }
    }
}
