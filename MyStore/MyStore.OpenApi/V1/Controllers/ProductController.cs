using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStore.OpenApi.Data;
using MyStore.OpenApi.Entities;
using MyStore.OpenApi.Extensions;
using MyStore.OpenApi.V1.Dtos;
using MyStore.OpenApi.V1.Validators;
using MyStore.OpenApi.V1.ViewModels;

namespace MyStore.OpenApi.V1.Controllers
{
    [ApiController()]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly MyStoreDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductController(MyStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetAll()
        {
            var products
                = await _dbContext
                    .Products
                    .Include(p => p.Category)
                    .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<ProductViewModel>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> GetOne(long id)
        {
            var productEntity
                = await _dbContext
                    .Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (productEntity == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(_mapper.Map<ProductViewModel>(productEntity));
        }

        [HttpPost]
        public async Task<ActionResult<ProductViewModel>> Create(ProductDto product)
        {
            var productEntity = _mapper.Map<Product>(product);

            productEntity.CreatedAt = DateTimeOffset.Now;
            productEntity.ModifiedAt = DateTimeOffset.Now;

            var productCategory
                = await _dbContext
                    .Categories
                    .FirstOrDefaultAsync(c => c.Id == productEntity.CategoryId);

            if (productCategory != null)
            {
                productEntity.Category = productCategory;
            }

            _dbContext.Add(productEntity);
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<ProductViewModel>(productEntity));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductViewModel>> Update(long id, ProductDto productDto)
        {
            var product
                = await _dbContext
                    .Products
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            _mapper.Map(productDto, product);

            var category
                = await _dbContext
                    .Categories
                    .FirstOrDefaultAsync(c => c.Id == product.Category.Id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            product.Category = category;
            product.ModifiedAt = DateTimeOffset.Now;
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<ProductViewModel>(product));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ProductViewModel>> PartiallyUpdate(long id, JsonPatchDocument<ProductDto> patchDocument)
        {
            var product
                = await _dbContext
                    .Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var productDto = _mapper.Map<ProductDto>(product);

            patchDocument.ApplyTo(productDto);

            var validationResult = await new ProductValidator().ValidateAsync(productDto);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(validationResult.ToModelState());
            }

            _mapper.Map(productDto, product);

            var category
                = await _dbContext
                    .Categories
                    .FirstOrDefaultAsync(c => c.Id == product.Category.Id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            product.Category = category;
            product.ModifiedAt = DateTimeOffset.Now;
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<ProductViewModel>(product));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var product
                = await _dbContext
                    .Products
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}