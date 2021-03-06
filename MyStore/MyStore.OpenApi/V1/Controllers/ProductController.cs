using System;
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
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _dbContext.Products.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(long id)
        {
            var productEntity = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (productEntity == null)
            {
                return NotFound();
            }

            return Ok(productEntity);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(ProductDto product)
        {
            var productEntity = _mapper.Map<Product>(product);
            productEntity.CreatedAt = DateTimeOffset.Now;
            productEntity.ModifiedAt = DateTimeOffset.Now;

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

            _mapper.Map(product, productEntity);
            productEntity.ModifiedAt = DateTimeOffset.Now;

            await _dbContext.SaveChangesAsync();

            return Ok(productEntity);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdate(long id, JsonPatchDocument<ProductDto> patchDocument)
        {
            var productEntity = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (productEntity == null)
            {
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDto>(productEntity);

            patchDocument.ApplyTo(productDto);

            var validationResult = await new ProductValidator().ValidateAsync(productDto);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(validationResult.ToModelState());
            }
            
            _mapper.Map(productDto, productEntity);
            productEntity.ModifiedAt = DateTimeOffset.Now;
 
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