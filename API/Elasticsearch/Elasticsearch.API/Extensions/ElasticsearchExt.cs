

using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace Elasticsearch.API.Extensions
{
    public static class ElasticsearchExt
    {
        public static void AddElastic(this IServiceCollection services, IConfiguration configuration)
        {
            var userName = configuration.GetSection("Elasticsearch")["Username"];
            var password = configuration.GetSection("Elasticsearch")["Password"];
            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elasticsearch")["Url"]!))
                .Authentication(new BasicAuthentication(userName!, password!));

            var client = new ElasticsearchClient(settings);

            services.AddSingleton(client);
        }
    }
}
