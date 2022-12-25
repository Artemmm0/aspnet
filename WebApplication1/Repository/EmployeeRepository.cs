using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public IEnumerable<Employee> GetAllEmployees(bool trackChanges)
        {
            return FindAll(trackChanges).OrderBy(c => c.Name).ToList();
        }
    }
}
