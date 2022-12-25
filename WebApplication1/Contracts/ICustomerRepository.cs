using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAllCustomers(Guid productId, bool trackChanges);
        Task<IEnumerable<Customer>> GetAllCustomersAsync(Guid productId, bool trackChanges);

        Customer GetCustomer(Guid productId, Guid customerId, bool trackChanges);
        Task<Customer> GetCustomerAsync(Guid productId, Guid customerId, bool trackChanges);

        void CreateCustomer(Customer customer);

        IEnumerable<Customer> GetByIds(Guid productId, IEnumerable<Guid> ids, bool trackChanges);
        Task<IEnumerable<Customer>> GetByIdsAsync(Guid productId, IEnumerable<Guid> ids, bool trackChanges);

        void DeleteCustomer(Customer customer);
    }
}
