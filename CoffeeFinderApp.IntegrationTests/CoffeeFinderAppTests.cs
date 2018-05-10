using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using NUnit.Framework;
using static Nest.Infer;

namespace CoffeeFinderApp.IntegrationTests
{
    public class CoffeeFinderAppTests
    {
        private string IndexName => "devwarsztaty";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ElasticClient().DeleteIndex(IndexName);
        }

        [Test]
        public void LoadsGoogleLocationsFromFiles()
        {
            var loaded = LoadLocations().Count;

            Assert.IsTrue(loaded > 0, "Didn't load google locations from files properly");
        }

        [Test]
        public async Task CorrectVersionOfElasticsearchShouldBeInstalled()
        {
            var elasticClient = ElasticClient();

            var response = await elasticClient.RootNodeInfoAsync();
            var actual = response.IsValid;

            Assert.IsTrue(actual, $"Couldn't connect to elasticsearch {response.ServerError}");

            var version = response.Version.Number;

            Assert.AreEqual("6.2.4", version);
        }
        
        [Test]
        public async Task DataHasBeenIndexedIntoElasticsearch()
        {
            new IndexDataIntoElasticsearch(ElasticClient())
                .Index(LoadLocations());

            //TODO exercise1, make the data available for search(count in this case)
            await ElasticClient().RefreshAsync(Indices(IndexName));

            var response = await ElasticClient().CountAsync<CoffeeLocation>();

            var actual = response.Count;

            Assert.AreEqual(78, actual, $"Not all of documents have been indexed. Actual count: {actual}, should be 78");
        }
        
        [Test]
        public async Task LocationIsOfTypeGeoPoint()
        {
            var response = await ElasticClient()
                .GetMappingAsync<CoffeeLocation>();

            var actual = response.Indices[IndexName].Mappings[TypeName.From<CoffeeLocation>()]
                .Properties
                .FirstOrDefault(x => x.Key == "location")
                .Value
                .GetType();

            Assert.AreEqual(typeof(GeoPointProperty), actual);
        }
        
        [Test]
        public async Task FindCoffeeNearMe()
        {
            var actual = await new WhereToGoForCoffee(ElasticClient())
                .NearMe(50.048456, 19.961603);

            Assert.AreEqual("BAL", actual.Name);
        }

        [Test]
        public async Task FindMyFavCoffeeNearMe()
        {
            var actual = await new WhereToGoForCoffee(ElasticClient())
                .NearMe("costa", 50.048456, 19.961603);

            Assert.AreEqual("COSTA COFFEE", actual.Name);
        }

        public List<GoogleLocationDetails> LoadLocations()
        {
            var locations = new List<GoogleLocationDetails>(100);

            foreach (var file in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "locations"), "*.json"))
            {
                locations.AddRange(new GoogleLocations(file).Load());
            }

            return locations;
        }

        private ElasticClient ElasticClient()
        {
            var uri = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(uri);
            settings.DefaultIndex(IndexName);
            settings.EnableDebugMode();
            var elasticClient =
                new ElasticClient(settings);

            return elasticClient;
        }
    }
}
