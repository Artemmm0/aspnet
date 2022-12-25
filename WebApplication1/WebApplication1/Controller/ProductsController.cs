using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace WebApplication1.Controller
{
    [Route("api/[controller]")]
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
        public IActionResult GetProducts()
        {
            var products = _repository.Product.GetAllProducts(trackChanges: false);

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(productsDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(Guid id)
        {
            var product = _repository.Product.GetProduct(id, trackChanges: false);

            var productDto = _mapper.Map<ProductDto>(product);

            return Ok(productDto);

        }
    }
}
