using Elasticsearch.API.DTOs;
using Elasticsearch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(ProductService productservice) : ControllerBase
    {
        [HttpPost]
        public async Task <IActionResult> Save(ProductCreateDto request)
        {
            return Ok(await productservice.SaveAsync(request));
        }
    }
}
