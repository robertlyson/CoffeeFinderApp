using System;
using System.IO;
using Nest;

namespace CoffeeFinderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(@"C:\dev\devwarsztaty\elasticsearch\CoffeeFinder\locations\", "*.json");
            foreach (var file in files)
            {
                var googleLocationDetailses = new GoogleLocations(file).Load();
            }

            Console.WriteLine("Hello World!");
        }
    }

    public class CoffeeLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public GeoLocation Location { get; set; }
    }
}
