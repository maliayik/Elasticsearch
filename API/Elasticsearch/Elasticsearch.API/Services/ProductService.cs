using System.Collections.Immutable;
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

            return ResponseDto<ProductDto>.Success(responseProduct.CreateDto(), HttpStatusCode.Created);
        }

        public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
        {
            var productListDto= new List<ProductDto>();

            var products = await _productRepository.GetAllAsync();


            //her productun feature bilgisi olmayacağından dolayı null kontrolü yapılır.
            foreach (var x in products)
            {
                if (x.Feature is null)
                {
                    productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock,null));
                }
                else
                {
                    productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock,
                        new ProductFeatureDto(x.Feature.Width, x.Feature.Height, x.Feature.Color)));
                }

            }

            return ResponseDto<List<ProductDto>>.Success(productListDto, HttpStatusCode.OK);

        }
    }
}
