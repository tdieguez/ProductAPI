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
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly MyStoreDbContext _dbContext;
        private readonly IMapper _mapper;

        public CategoryController(MyStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _dbContext.Categories.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(long id)
        {
            var categoryEntity = await _dbContext.Categories.FirstOrDefaultAsync(p => p.Id == id);

            if (categoryEntity == null)
            {
                return NotFound();
            }

            return Ok(categoryEntity);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(CategoryDto category)
        {
            var categoryEntity = _mapper.Map<Category>(category);

            categoryEntity.CreatedAt = DateTimeOffset.Now;
            categoryEntity.ModifiedAt = DateTimeOffset.Now;
            _dbContext.Add(categoryEntity);
            await _dbContext.SaveChangesAsync();

            return Ok(categoryEntity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(long id, CategoryDto category)
        {
            var categoryEntity = _dbContext.Categories.FirstOrDefault(p => p.Id == id);

            if (categoryEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(category, categoryEntity);
            categoryEntity.ModifiedAt = DateTimeOffset.Now;

            await _dbContext.SaveChangesAsync();

            return Ok(categoryEntity);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdate(long id, JsonPatchDocument<CategoryDto> patchDocument)
        {
            var categoryEntity = await _dbContext.Categories.FirstOrDefaultAsync(p => p.Id == id);

            if (categoryEntity == null)
            {
                return NotFound();
            }

            var categoryDto = _mapper.Map<CategoryDto>(categoryEntity);

            patchDocument.ApplyTo(categoryDto);

            var validationResult = await new CategoryValidator().ValidateAsync(categoryDto);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(validationResult.ToModelState());
            }

            _mapper.Map(categoryDto, categoryEntity);
            categoryEntity.ModifiedAt = DateTimeOffset.Now;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(p => p.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            _dbContext.Remove(category);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}