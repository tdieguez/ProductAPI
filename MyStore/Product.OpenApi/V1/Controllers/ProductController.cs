using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Product.OpenApi.V1.Controllers
{
    [ApiController()]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        //TODO: remove static collection once the EF Core is implemented
        private static ICollection<Entities.Product> _productCollection = Entities.Product.GetCollection();

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
        public IActionResult Create()
        {
            return Ok("Test");
        }

        [HttpPut]
        public IActionResult Update()
        {
            return Ok("Test");
        }

        [HttpPatch]
        public IActionResult UpdatePartial()
        {
            return Ok("Test");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete()
        {
            return Ok("Test");
        }
    }
}