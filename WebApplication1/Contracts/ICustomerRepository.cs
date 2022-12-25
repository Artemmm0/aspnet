﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAllCustomers(Guid productId, bool trackChanges);
        Customer GetCustomer(Guid productId, Guid customerId, bool trackChanges);
    }
}
