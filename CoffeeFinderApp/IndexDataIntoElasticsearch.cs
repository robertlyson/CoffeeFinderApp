using System.Collections.Generic;
using Nest;

namespace CoffeeFinderApp
{
    public class IndexDataIntoElasticsearch
    {
        private readonly IElasticClient _elasticClient;

        public IndexDataIntoElasticsearch(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public void Index(List<GoogleLocationDetails> locations)
        {
            //TODO: exercise2, make sure Location is of type geo_point

            //TODO: exercise1, indexing
            //TODO: exercise1, can we speed up this process?
            for (var i = 0; i < locations.Count; i++)
            {
                var item = locations[i];
                _elasticClient.IndexDocument(new CoffeeLocation
                {
                    Id = i,
                    Name = item.Name,
                    Location = GeoLocation.TryCreate(item.Lat, item.Lng)
                });
            }
        }
    }
}