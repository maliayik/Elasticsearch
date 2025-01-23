using System.Collections.Immutable;
using Elasticsearch.API.DTOs;
using Elasticsearch.API.Models;
using Nest;

namespace Elasticsearch.API.Repositories
{
    public class ProductRepository
    {
        private readonly IElasticClient _elasticClient;
        private const string indexName = "products";

        public ProductRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<Product?> SaveAsync(Product newProduct)
        {
            newProduct.Created = DateTime.Now;

            //product indexine yeni bir doküman eklemek için kullanılır.Id değerini uygulamamız üzerinden üretiyoruz, yoksa elasticsearch id değeri üretir.
            var response = await _elasticClient.IndexAsync(newProduct, x => x.Index(indexName).Id(Guid.NewGuid().ToString()));

            if (!response.IsValid) return null;

            newProduct.Id = response.Id;

            return newProduct;
        }

        /// <summary>
        /// elastichsearch üzerindeki tüm datayı  değişiklik yapılamayacak türde döndürür.
        /// </summary>
        public async Task<ImmutableList<Product>> GetAllAsync()
        {
            var result = await _elasticClient.SearchAsync<Product>(s => s.Index(indexName)
                    .Query(q => q.MatchAll()));

            //resultumızın içerisinde id değeri görmemiz için source içindeki id'ye atıyoruz.
            result.Hits.ToList().ForEach(x => x.Source.Id = x.Id);

            return result.Documents.ToImmutableList();

        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            var response = await _elasticClient.GetAsync<Product>(id, x => x.Index(indexName));
            if (!response.IsValid) return null;

            response.Source.Id = response.Id;

            return response.Source;
        }

        public async Task<bool> UpdateAsync(ProductUpdateDto updateProduct)
        {
            var response = await _elasticClient.UpdateAsync<Product, ProductUpdateDto>(updateProduct.Id, u => u
                .Index(indexName)
                .Doc(updateProduct));

            return response.IsValid;
        }

        public async Task<bool> DeleteAsync(string id)
        {
           var response= await _elasticClient.DeleteAsync<Product>(id, x => x.Index(indexName));
           
           return response.IsValid;
        }
    }
}
