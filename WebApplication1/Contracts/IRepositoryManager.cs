using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }
        IEmployeeRepository Employee { get; }
        IProductRepository Product { get; }
        ICustomerRepository Customer { get; }
        void Save();
        Task SaveAsync();
    }
}
