using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ActionFilters;
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
        public async Task<IActionResult> GetCustomersForProduct(Guid productId)
        {
            var product = await _repository.Product.GetProductAsync(productId, false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                return NotFound();
            }

            var customers = await _repository.Customer.GetAllCustomersAsync(productId, trackChanges: false);
            var customersDto = _mapper.Map<IEnumerable<CustomerDto>>(customers);

            return Ok(customersDto);
        }

        [HttpGet("{id}", Name = "CustomerById")]
        public async Task<IActionResult> GetCustomerForProduct(Guid id, Guid productId)
        {
            var product = await _repository.Product.GetProductAsync(productId, false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                return NotFound();
            }

            var customer = await _repository.Customer.GetCustomerAsync(productId, id, trackChanges: false);
            if (customer == null)
            {
                _logger.LogInfo($"Customer with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);

            return Ok(customerDto);
        }

        [HttpGet("collection/({ids})", Name = "CustomerCollection")]
        public async Task<IActionResult> GetCustomerCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids,
            Guid productId)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var customerEntities = await _repository.Customer.GetByIdsAsync(productId, ids, trackChanges: false);
            if (ids.Count() != customerEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var customersToReturn = _mapper.Map<IEnumerable<CustomerDto>>(customerEntities);
            return Ok(customersToReturn);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerForCreationDto customer)
        {
            var customerEntity = _mapper.Map<Customer>(customer);
            _repository.Customer.CreateCustomer(customerEntity);
            await _repository.SaveAsync();

            var customerToReturn = _mapper.Map<CustomerDto>(customerEntity);
            return CreatedAtRoute("CustomerById", new { id = customerToReturn.Id },
                customerToReturn);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCustomerCollection(
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
            await _repository.SaveAsync();

            var customerCollectionToReturn =
                _mapper.Map<IEnumerable<CustomerDto>>(customerEntities);
            var ids = string.Join(",", customerCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("CustomerCollection", new { ids }, customerCollectionToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerForProduct(Guid productId, Guid id)
        {
            var product = await _repository.Product.GetProductAsync(productId, trackChanges: false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                return NotFound();
            }

            var customer = await _repository.Customer.GetCustomerAsync(productId, id, trackChanges: false);
            if (customer == null)
            {
                _logger.LogInfo($"Customer with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _repository.Customer.DeleteCustomer(customer);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid productId, Guid id, 
            [FromBody] CompanyForUpdateDto customer)
        {
            if (customer == null)
            {
                _logger.LogError("CustomerForUpdateDto object sent from client is null.");
                return BadRequest("CustomerForUpdateDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for CustomerForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }

            var customerEntity = await _repository.Customer.GetCustomerAsync(productId, id, trackChanges: true);
            if (customerEntity == null)
            {
                _logger.LogInfo($"Customer with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _mapper.Map(customer, customerEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateCustomerForProduct(Guid productId, Guid id,
            [FromBody] JsonPatchDocument<CustomerForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var product = await _repository.Product.GetProductAsync(productId, trackChanges: false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                return NotFound();
            }

            var customerEntity = await _repository.Customer.GetCustomerAsync(productId, id, trackChanges: true);
            if (customerEntity == null)
            {
                _logger.LogInfo($"Customer with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var customerToPatch = _mapper.Map<CustomerForUpdateDto>(customerEntity);
            patchDoc.ApplyTo(customerToPatch, ModelState);
            TryValidateModel(customerToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(customerToPatch, customerEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
