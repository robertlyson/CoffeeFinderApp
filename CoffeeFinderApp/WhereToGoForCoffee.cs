using System.Linq;
using System.Threading.Tasks;
using Nest;

namespace CoffeeFinderApp
{
    public class WhereToGoForCoffee
    {
        private readonly IElasticClient _elasticClient;

        public WhereToGoForCoffee(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<CoffeeLocation> NearMe(double lat, double lng)
        {
            //TODO: exercise3, write query to find nearest coffe
            //TODO:            bool query with filter(goe distance)
            //TODO: exercise3, add sort
            var searchResponse = await _elasticClient.SearchAsync<CoffeeLocation>(s => s
                .Query(q => q.Bool(b => b
                    .Must(m => m.MatchAll())
                    .Filter(f => f.GeoDistance(gd => gd
                        .Location(lat, lng)
                        .Field(field => field.Location)))))
                .Sort(sort => sort
                    .GeoDistance(gd => gd
                        .Field(f => f.Location)
                        .Points(GeoLocation.TryCreate(lat, lng))
                        .Ascending()))
            );

            return searchResponse.Documents.FirstOrDefault();
        }

        public async Task<CoffeeLocation> NearMe(string brand, double lat, double lng)
        {
            //TODO: exercise4, search nearest by name
            var searchResponse = await _elasticClient.SearchAsync<CoffeeLocation>(s => s
                .Query(q => q.Bool(b => b
                    .Must(m => m.Match(match => match.Field(f => f.Name).Query(brand)))
                    .Filter(f => f.GeoDistance(gd => gd
                        .Location(lat, lng)
                        .Field(field => field.Location)))))
                .Sort(sort => sort
                    .GeoDistance(gd => gd
                        .Field(f => f.Location)
                        .Points(GeoLocation.TryCreate(lat, lng))
                        .Ascending())));

            return searchResponse.Documents.FirstOrDefault();
        }
    }
}