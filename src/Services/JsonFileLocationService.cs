﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using ContosoCrafts.WebSite.Models;

using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// Create class from Location.json file and retrieve all data from file.
    /// </summary>
    public class JsonFileLocationService
    {
        // Initialize class
        public JsonFileLocationService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }
        // Data middle tier
        public IWebHostEnvironment WebHostEnvironment { get; }
        // Get json file path 
        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "location.json"); }
        }
        // Read .json file and return all data fields through the model
        public IEnumerable<LocationModel> GetAllData()
        {
            using(var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<LocationModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        public void AddRating(string productId, int rating)
        {
            var products = GetAllData();

            if(products.First(x => x.location_id == productId).rating == null)
            {
                products.First(x => x.location_id == productId).rating = new int[] { rating };
            }
            else
            {
                var ratings = products.First(x => x.location_id == productId).rating.ToList();
                ratings.Add(rating);
                products.First(x => x.location_id == productId).rating = ratings.ToArray();
            }

            using(var outputStream = File.OpenWrite(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<LocationModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }), 
                    products
                );
            }
        }

        /// <summary>
        /// Find the data record
        /// Update the fields
        /// Save to the data store
        /// </summary>
        /// <param name="data"></param>
        public LocationModel UpdateData(LocationModel data)
        {
            var products = GetAllData();
            var productData = products.FirstOrDefault(x => x.location_id.Equals(data.location_id));
            if (productData == null)
            {
                return null;
            }

            productData.name = data.name;
            productData.type_id = data.type_id;
            productData.address = data.address;
            productData.img = data.img;

            SaveData(products);

            return productData;
        }

        /// <summary>
        /// Save All products data to storage
        /// </summary>
        private void SaveData(IEnumerable<LocationModel> products)
        {

            using (var outputStream = File.Create(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<LocationModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    products
                );
            }
        }

        /// <summary>
        /// Create a new product using default values
        /// After create the user can update to set values
        /// </summary>
        /// <returns></returns>
        public LocationModel CreateData()
        {
            var data = new LocationModel()
            {
                location_id = System.Guid.NewGuid().ToString(),
                name = "Enter Name",
                address = "Enter Address",
                type_id = "Enter Type ID",
                img = "",
            };

            // Get the current set, and append the new record to it
            var dataSet = GetAllData();
            dataSet = dataSet.Append(data);

            SaveData(dataSet);

            return data;
        }

        /// <summary>
        /// Remove the item from the system
        /// </summary>
        /// <returns></returns>
        public LocationModel DeleteData(string id)
        {
            // Get the current set, and append the new record to it
            var dataSet = GetAllData();
            var data = dataSet.FirstOrDefault(m => m.location_id.Equals(id));

            var newDataSet = GetAllData().Where(m => m.location_id.Equals(id) == false);
            
            SaveData(newDataSet);

            return data;
        }
        public IEnumerable<LocationModel> sortByRate()
        {
            // Get the current set, and append the new record to it
            var dataSet = GetAllData();
            // var data = dataSet.OrderBy(m => m.rating.Sum() / m.rating.Count());

            var data = (from t in dataSet
                        where t.rating != null
                        select t);

            var sortingData = data.OrderBy(d => d.rating.Sum() / d.rating.Count());
            return sortingData;
        }
        public IEnumerable<LocationModel> sortByLocation()
        {
            // Get the current set, and append the new record to it
            var dataSet = GetAllData();
            // var data = dataSet.OrderBy(m => m.rating.Sum() / m.rating.Count());


            var sortingData = dataSet.OrderByDescending(d => d.type_id);
            return sortingData;
        }


    }
}
