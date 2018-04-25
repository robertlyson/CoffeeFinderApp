using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CoffeeFinderApp
{
    public class GoogleLocations
    {
        private readonly string _filePath;

        public GoogleLocations(string filePath)
        {
            _filePath = filePath;
        }

        public List<GoogleLocationDetails> Load()
        {
            var fileContent = File.ReadAllText(_filePath);

            var tmp = new List<GoogleLocationDetails>();

            try
            {
                var resultsToken = JObject.Parse(fileContent)["results"];
                tmp = resultsToken.Select(x => new GoogleLocationDetails
                {
                    Name = x["name"].ToObject<string>(),
                    Lat = x["geometry"]["location"]["lat"].ToObject<double>(),
                    Lng = x["geometry"]["location"]["lng"].ToObject<double>()
                }).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception($"Can't load google locations from {_filePath} file", exception);
            }

            return tmp;
        }
    }
}