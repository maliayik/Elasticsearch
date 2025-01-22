using System.Net;
using Elasticsearch.API.DTOs;
using Elasticsearch.API.Repositories;

namespace Elasticsearch.API.Services
{
    public class ProductService(ProductRepository _productRepository)
    {
        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
        {
            var responseProduct = await _productRepository.SaveAsync(request.CreateProduct());

            if (responseProduct == null)
            {
                return ResponseDto<ProductDto>.Fail(new List<string> { "kayıt esnasında hata meydana geldi" }, HttpStatusCode.InternalServerError);
            }

            return ResponseDto<ProductDto>.Success(responseProduct.CreateDto(),HttpStatusCode.Created);
        }
    }
}
