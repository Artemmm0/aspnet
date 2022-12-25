using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }
        IEmployeeRepository Employee { get; }
        IProductRepository Product { get; }
        ICustomerRepository Customer { get; }
        void Save();
    }
}
