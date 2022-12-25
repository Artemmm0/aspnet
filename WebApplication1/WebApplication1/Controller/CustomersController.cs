using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.ModelBinders;

namespace WebApplication1.Controller
{
    [Route("api/companies/{companyId}/products/{productId}/customers")]
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

        [HttpGet("{id}", Name = "CustomerById")]
        public IActionResult GetCustomerForProduct(Guid id, Guid productId)
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

        [HttpGet("collection/({ids})", Name = "CustomerCollection")]
        public IActionResult GetCustomerCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids,
            Guid productId)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var customerEntities = _repository.Customer.GetByIds(productId, ids, trackChanges: false);
            if (ids.Count() != customerEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var customersToReturn = _mapper.Map<IEnumerable<CustomerDto>>(customerEntities);
            return Ok(customersToReturn);
        }

        [HttpPost]
        public IActionResult CreateCustomer([FromBody] CustomerForCreationDto customer)
        {
            if (customer == null)
            {
                _logger.LogError("CustomerForCreationDto object sent from client is null.");
                return BadRequest("CustomerForCreationDto object is null");
            }

            var customerEntity = _mapper.Map<Customer>(customer);
            _repository.Customer.CreateCustomer(customerEntity);
            _repository.Save();

            var customerToReturn = _mapper.Map<CustomerDto>(customerEntity);
            return CreatedAtRoute("CustomerById", new { id = customerToReturn.Id },
                customerToReturn);
        }

        [HttpPost("collection")]
        public IActionResult CreateCustomerCollection(
            [FromBody] IEnumerable<CustomerForCreationDto> customerCollection)
        {
            if (customerCollection == null)
            {
                _logger.LogError("Customer collection sent from client is null.");
                return BadRequest("Customer collection is null");
            }

            var customerEntities = _mapper.Map<IEnumerable<Customer>>(customerCollection);
            foreach (var customer in customerEntities)
            {
                _repository.Customer.CreateCustomer(customer);
            }
            _repository.Save();

            var customerCollectionToReturn =
                _mapper.Map<IEnumerable<CustomerDto>>(customerEntities);
            var ids = string.Join(",", customerCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("CustomerCollection", new { ids }, customerCollectionToReturn);
        }

    }
}
