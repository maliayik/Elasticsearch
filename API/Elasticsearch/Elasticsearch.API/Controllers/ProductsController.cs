using Elasticsearch.API.DTOs;
using Elasticsearch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.API.Controllers
{
    public class ProductsController(ProductService productservice) : BaseController
    {
        [HttpPost]
        public async Task <IActionResult> Save(ProductCreateDto request)
        {
            return CreateActionResult(await productservice.SaveAsync(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResult(await productservice.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return CreateActionResult(await productservice.GetByIdAsync(id));
        }
    }
}
