using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyStore.OpenApi.Data;
using MyStore.OpenApi.Entities;
using MyStore.OpenApi.V1.Dtos;

namespace MyStore.OpenApi.V1.Controllers
{
    [ApiController()]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly MyStoreDbContext _dbContext;

        public ProductController(MyStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_dbContext.Products.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(long id)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<ProductDto> Create(ProductDto product)
        {
            var productEntity = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                CreatedAt = DateTimeOffset.Now,
                ModifiedAt = DateTimeOffset.Now
            };

            _dbContext.Add(productEntity);
            _dbContext.SaveChanges();

            return Ok(productEntity);
        }

        [HttpPut("{id}")]
        public ActionResult<ProductDto> Update(long id, ProductDto product)
        {
            var productEntity = _dbContext.Products.FirstOrDefault(p => p.Id == id);

            if (productEntity == null)
            {
                return NotFound();
            }

            productEntity.Name = product.Name;
            productEntity.Description = product.Description;
            productEntity.Price = product.Price;
            productEntity.Category = product.Category;
            productEntity.ModifiedAt = DateTimeOffset.Now;

            _dbContext.SaveChanges();

            return Ok(productEntity);
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdate(long id, JsonPatchDocument<ProductDto> patchDocument)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category
            };

            patchDocument.ApplyTo(productDto);

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Category = productDto.Category;

            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}