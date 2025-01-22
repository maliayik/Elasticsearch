using Elasticsearch.API.Models;

namespace Elasticsearch.API.DTOs
{
    //record kullanmamızın sebebi sadece okunabilir olmasıdır, bu sayede DTO'larımızın propertysi immutable(değiştirilemez) olmasını sağlamış oluruz.
    public record ProductCreateDto(string Name, decimal Price, int Stock, ProductFeatureDto Feature)
    {
        //Dto mapleme işlemi mapper kullanmadığımızdan burada yaptık.

        public Product CreateProduct()
        {
            return new Product
            {
                Name = Name,
                Price = Price,
                Stock = Stock,
                Feature = new ProductFeature
                {
                    Width = Feature.Width,
                    Height = Feature.Height,
                    Color = (EColor)int.Parse(Feature.Color)
                }
            };
        }
    }
}
