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
using WebApplication1.ModelBinders;

namespace WebApplication1.Controller
{
    [Route("api/companies/{companyId}/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ProductsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetProducts(Guid companyId)
        {
            var products = await _repository.Product.GetAllProductsAsync(companyId, trackChanges: false);

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(productsDto);
        }

        [HttpGet("{id}", Name = "ProductById")]
        [HttpHead]
        public async Task<IActionResult> GetProductById(Guid id, Guid companyId)
        {
            var product = await _repository.Product.GetProductAsync(companyId, id, trackChanges: false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        [HttpGet("collection/({ids})", Name = "ProductCollection")]
        [HttpHead]
        public async Task<IActionResult> GetProductCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids, 
            Guid companyId)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var productEntities = await _repository.Product.GetByIdsAsync(companyId, ids, trackChanges: false);
            if (ids.Count() != productEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var productsToReturn = _mapper.Map<IEnumerable<ProductDto>>(productEntities);
            return Ok(productsToReturn);
        }

        [HttpPost]
        [HttpHead]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreationDto product)
        {
            if (product == null)
            {
                _logger.LogError("ProductForCreationDto object sent from client is null.");
                return BadRequest("ProductForCreationDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for ProductForCreationDto object");
                return UnprocessableEntity(ModelState);
            }

            var productEntity = _mapper.Map<Product>(product);
            _repository.Product.CreateProduct(productEntity);
            await _repository.SaveAsync();

            var productToReturn = _mapper.Map<ProductDto>(productEntity);
            return CreatedAtRoute("ProductById", new { id = productToReturn.Id },
                productToReturn);
        }

        [HttpPost("collection")]
        [HttpHead]
        public async Task<IActionResult> CreateProductCollection(
            [FromBody] IEnumerable<ProductForCreationDto> productCollection)
        {
            if (productCollection == null)
            {
                _logger.LogError("Product collection sent from client is null.");
                return BadRequest("Product collection is null");
            }

            var productEntities = _mapper.Map<IEnumerable<Product>>(productCollection);
            foreach (var product in productEntities)
            {
                _repository.Product.CreateProduct(product);
            }
            await _repository.SaveAsync();

            var productCollectionToReturn =
                _mapper.Map<IEnumerable<ProductDto>>(productEntities);
            var ids = string.Join(",", productCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("ProductCollection", new { ids }, productCollectionToReturn);
        }

        [HttpDelete("{id}")]
        [HttpHead]
        public async Task<IActionResult> DeleteProductForCompany(Guid companyId, Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database");
                return NotFound();
            }

            var productForCompany = await _repository.Product.GetProductAsync(companyId, id, trackChanges: false);
            if (productForCompany == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _repository.Product.DeleteProduct(productForCompany);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        [HttpHead]
        public async Task<IActionResult> UpdateProductForCompany(Guid companyId, Guid id,
            [FromBody] ProductForUpdateDto product)
        {
            if (product == null)
            {
                _logger.LogError("ProductForUpdateDto object sent from client is null.");
                return BadRequest("ProductForUpdateDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for ProductForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }

            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var productEntity = await _repository.Product.GetProductAsync(companyId, id, trackChanges: true);
            if (productEntity == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _mapper.Map(product, productEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [HttpHead]
        public async Task<IActionResult> PartiallyUpdateProductForCompany(Guid companyId, Guid id,
            [FromBody] JsonPatchDocument<ProductForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var productEntity = await _repository.Product.GetProductAsync(companyId, id, trackChanges: true);
            if (productEntity == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var productToPatch = _mapper.Map<ProductForUpdateDto>(productEntity);
            patchDoc.ApplyTo(productToPatch, ModelState);
            TryValidateModel(productToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(productToPatch, productEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
