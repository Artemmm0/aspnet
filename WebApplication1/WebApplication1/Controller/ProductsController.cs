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
        public IActionResult GetProducts(Guid companyId)
        {
            var products = _repository.Product.GetAllProducts(companyId, trackChanges: false);

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(productsDto);
        }

        [HttpGet("{id}", Name = "ProductById")]
        public IActionResult GetProductById(Guid id, Guid companyId)
        {
            var product = _repository.Product.GetProduct(companyId, id, trackChanges: false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        [HttpGet("collection/({ids})", Name = "ProductCollection")]
        public IActionResult GetProductCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids, 
            Guid companyId)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var productEntities = _repository.Product.GetByIds(companyId, ids, trackChanges: false);
            if (ids.Count() != productEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var productsToReturn = _mapper.Map<IEnumerable<ProductDto>>(productEntities);
            return Ok(productsToReturn);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductForCreationDto product)
        {
            if (product == null)
            {
                _logger.LogError("ProductForCreationDto object sent from client is null.");
                return BadRequest("ProductForCreationDto object is null");
            }

            var productEntity = _mapper.Map<Product>(product);
            _repository.Product.CreateProduct(productEntity);
            _repository.Save();

            var productToReturn = _mapper.Map<ProductDto>(productEntity);
            return CreatedAtRoute("ProductById", new { id = productToReturn.Id },
                productToReturn);
        }

        [HttpPost("collection")]
        public IActionResult CreateProductCollection(
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
            _repository.Save();

            var productCollectionToReturn =
                _mapper.Map<IEnumerable<ProductDto>>(productEntities);
            var ids = string.Join(",", productCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("ProductCollection", new { ids }, productCollectionToReturn);
        }
    }
}
