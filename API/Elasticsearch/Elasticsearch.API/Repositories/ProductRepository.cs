using Elasticsearch.API.Models;
using Nest;

namespace Elasticsearch.API.Repositories
{
    public class ProductRepository
    {
        private readonly IElasticClient _elasticClient;

        public ProductRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<Product?> SaveAsync(Product newProduct)
        {
            newProduct.Created = DateTime.Now;

            //product indexine yeni bir doküman eklemek için kullanılır.Id değerini uygulamamız üzerinden üretiyoruz, yoksa elasticsearch id değeri üretir.
            var response = await _elasticClient.IndexAsync(newProduct,x=> x.Index("products").Id(Guid.NewGuid().ToString()));

            if (!response.IsValid) return null;

            newProduct.Id = response.Id;

            return newProduct;
        }
    }
}
