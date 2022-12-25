using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace WebApplication1.Controller
{
    [Route("api/products/{productId}/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CustomersController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCustomersForProduct(Guid productId)
        {
            var product = _repository.Product.GetProduct(productId, false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                return NotFound();
            }

            var customers = _repository.Customer.GetAllCustomers(productId, trackChanges: false);
            var customersDto = _mapper.Map<IEnumerable<CustomerDto>>(customers);

            return Ok(customersDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerForProduct(Guid productId, Guid id)
        {
            var product = _repository.Product.GetProduct(productId, false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                return NotFound();
            }

            var customer = _repository.Customer.GetCustomer(productId, id, trackChanges: false);
            if (customer == null)
            {
                _logger.LogInfo($"Customer with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);

            return Ok(customerDto);
        }
    }
}
