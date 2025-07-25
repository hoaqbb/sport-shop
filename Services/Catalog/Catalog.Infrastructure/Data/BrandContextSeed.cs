﻿using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public static class BrandContextSeed
    {
        public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
        {
            bool checkBrands = brandCollection.Find(b => true).Any();
            if(!checkBrands)
            {
                //var brandsData = File.ReadAllText("./Catalog/Catalog.Infrastructure/Data/SeedData/brands.json");
                var path = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "brands.json");
                var brandsData = File.ReadAllText(path);

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if(brands != null)
                {
                    foreach(var item in brands)
                    {
                        brandCollection.InsertOneAsync(item);
                    }
                }
            }
        }
    }
}
