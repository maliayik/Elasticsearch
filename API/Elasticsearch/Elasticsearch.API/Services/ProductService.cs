﻿using System.Collections.Immutable;
using System.Net;
using Elastic.Clients.Elasticsearch;
using Elasticsearch.API.DTOs;
using Elasticsearch.API.Models;
using Elasticsearch.API.Repositories;

namespace Elasticsearch.API.Services
{
    public class ProductService(ProductRepository _productRepository,ILogger<ProductService> logger)
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
                    continue;
                }

                    productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock,
                        new ProductFeatureDto(x.Feature.Width, x.Feature.Height, x.Feature.Color.ToString())));
            }

            return ResponseDto<List<ProductDto>>.Success(productListDto, HttpStatusCode.OK);

        }

        public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
        {
            var hasProduct= await _productRepository.GetByIdAsync(id);

            if (hasProduct == null)
            {
                return ResponseDto<ProductDto>.Fail("Ürün Bulunamadı",HttpStatusCode.NotFound);
            }

            return ResponseDto<ProductDto>.Success(hasProduct.CreateDto(), HttpStatusCode.OK);
        }

        public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto updateProduct)
        {
            var isSuccess =await _productRepository.UpdateAsync(updateProduct);

            if (!isSuccess)
            {
                return ResponseDto<bool>.Fail(new List<string> { "güncelleme esnasında hata meydana geldi" }, HttpStatusCode.InternalServerError);
            }

            return ResponseDto<bool>.Success(true,HttpStatusCode.NoContent);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string id)
        {
            var deleteResponse=await _productRepository.DeleteAsync(id);

            if (!deleteResponse.IsValidResponse && deleteResponse.Result == Result.NotFound)
            {
                return ResponseDto<bool>.Fail(new List<string> { "silmeye çalıştığınız ürün bulunamamıştır" }, HttpStatusCode.NotFound);
            }

            if (!deleteResponse.IsValidResponse)
            {
                //I Logger sınıfından faydalanarak,Loglamamızı servis katmanında yaptık.
                deleteResponse.TryGetOriginalException(out Exception? exception);

                logger.LogError(exception,deleteResponse.ElasticsearchServerError.Error.ToString());

                return ResponseDto<bool>.Fail(new List<string> { "silme esnasında hata meydana geldi" }, HttpStatusCode.InternalServerError);
            }

           

            return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
        }
    }
}
