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
            //TODO: mape sure Location is of type geo_point
            var createIndexResponse = _elasticClient.CreateIndex(_elasticClient.ConnectionSettings.DefaultIndex,
                i => i.Mappings(m => m.Map<CoffeeLocation>(mm => mm
                    .AutoMap()
                    .Properties(p => p.GeoPoint(gp => gp.Name(n => n.Location))))));

            //TODO: can we speed up this process?
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