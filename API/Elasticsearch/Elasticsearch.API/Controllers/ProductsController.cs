using Elasticsearch.API.DTOs;
using Elasticsearch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.API.Controllers
{
    public class ProductsController(ProductService productservice) : BaseController
    {
        /// <summary>
        /// Elastic search'e kayıt işlemi yapılır.
        /// </summary>
        [HttpPost]
        public async Task <IActionResult> Save(ProductCreateDto request)
        {
            return CreateActionResult(await productservice.SaveAsync(request));
        }

        /// <summary>
        /// Tüm kayıtları getirir.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResult(await productservice.GetAllAsync());
        }

        /// <summary>
        /// id'ye göre get işlemi yapılır.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return CreateActionResult(await productservice.GetByIdAsync(id));
        }

        /// <summary>
        /// Product datasını güncelleme işlemini yapar.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto updateProduct)
        {
            return CreateActionResult(await productservice.UpdateAsync(updateProduct));
        }

        /// <summary>
        /// id'ye göre silme işlemi yapılır.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            return CreateActionResult(await productservice.DeleteAsync(id));
        }
    }
}
