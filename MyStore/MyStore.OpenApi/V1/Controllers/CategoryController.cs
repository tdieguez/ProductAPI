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
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetAll()
        {
            var categories
                = await _dbContext
                    .Categories
                    .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<CategoryViewModel>>(categories));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryViewModel>> GetOne(long id)
        {
            var category
                = await _dbContext
                    .Categories
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            return Ok(_mapper.Map<CategoryViewModel>(category));
        }

        [HttpPost]
        public async Task<ActionResult<CategoryViewModel>> Create(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            category.CreatedAt = DateTimeOffset.Now;
            category.ModifiedAt = DateTimeOffset.Now;
            _dbContext.Add(category);
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<CategoryViewModel>(category));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(long id, CategoryDto categoryDto)
        {
            var category
                = await _dbContext
                    .Categories
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            _mapper.Map(categoryDto, category);
            category.ModifiedAt = DateTimeOffset.Now;
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<CategoryViewModel>(category));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdate(long id, JsonPatchDocument<CategoryDto> patchDocument)
        {
            var category
                = await _dbContext
                    .Categories
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);

            patchDocument.ApplyTo(categoryDto);

            var validationResult = await new CategoryValidator().ValidateAsync(categoryDto);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(validationResult.ToModelState());
            }

            _mapper.Map(categoryDto, category);
            category.ModifiedAt = DateTimeOffset.Now;
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<CategoryViewModel>(category));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var category
                = await _dbContext
                    .Categories
                    .FirstOrDefaultAsync(p => p.Id == id);

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