using System;
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

            throw new NotImplementedException("exercise3");
        }

        public async Task<CoffeeLocation> NearMe(string brand, double lat, double lng)
        {
            //TODO: exercise4, search nearest by name

            throw new NotImplementedException("exercise4");
        }
    }
}