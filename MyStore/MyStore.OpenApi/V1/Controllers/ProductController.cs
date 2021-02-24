using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyStore.OpenApi.Entities;
using MyStore.OpenApi.V1.Dtos;

namespace MyStore.OpenApi.V1.Controllers
{
    [ApiController()]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        //TODO: remove static collection once the EF Core is implemented
        private static ICollection<Product> _productCollection = Entities.Product.GetCollection();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_productCollection);
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(long id)
        {
            var product = _productCollection.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<ProductDto> Create(ProductDto product)
        {
            var productEntity = new Entities.Product
            {
                Id = (_productCollection.Max(p => p.Id) + 1),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                CreatedAt = DateTimeOffset.Now,
                ModifiedAt = DateTimeOffset.Now
            };

            _productCollection.Add(productEntity);

            return Ok(productEntity);
        }

        [HttpPut("{id}")]
        public ActionResult<ProductDto> Update(long id, ProductDto product)
        {
            var productEntity = _productCollection.FirstOrDefault(p => p.Id == id);

            if (productEntity == null)
            {
                return NotFound();
            }

            productEntity.Name = product.Name;
            productEntity.Description = product.Description;
            productEntity.Price = product.Price;
            productEntity.Category = product.Category;
            productEntity.ModifiedAt = DateTimeOffset.Now;

            return Ok(productEntity);
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdate(long id, JsonPatchDocument<ProductDto> patchDocument)
        {
            var product = _productCollection.FirstOrDefault(p => p.Id == id);

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

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var product = _productCollection.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _productCollection.Remove(product);

            return NoContent();
        }
    }
}