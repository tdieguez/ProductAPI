using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStore.OpenApi.Data;
using MyStore.OpenApi.Entities;
using MyStore.OpenApi.Extensions;
using MyStore.OpenApi.V1.Dtos;
using MyStore.OpenApi.V1.Validators;

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
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _dbContext.Products.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(long id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(ProductDto product)
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
            await _dbContext.SaveChangesAsync();

            return Ok(productEntity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(long id, ProductDto product)
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

            await _dbContext.SaveChangesAsync();

            return Ok(productEntity);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdate(long id, JsonPatchDocument<ProductDto> patchDocument)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

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

            var validationResult = await new ProductValidator().ValidateAsync(productDto);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(validationResult.ToModelState());
            }

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Category = productDto.Category;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}