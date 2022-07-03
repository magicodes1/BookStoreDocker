using BookStoreDocker.Models;
using Nest;

namespace BookStoreDocker.Extensions;

public static class ElasticSearchConfiguration
{
    public static void AddElasticSearch(
       this IServiceCollection services, IConfiguration configuration
   )
    {
        var url = configuration["ELSConfig:Uri"];

        var settings = new ConnectionSettings(new Uri(url))
                        .DefaultIndex("abc");

        var client = new ElasticClient(settings);
        services.AddSingleton<IElasticClient>(client);

        CreateIndex(client);
    }

    private static void CreateIndex(ElasticClient client)
    {
        client.Indices.Create("book",x=>x.Map<Book>(m=>m.AutoMap()));
        client.Indices.Create("author",x=>x.Map<Author>(m=>m.AutoMap()));
    }
}