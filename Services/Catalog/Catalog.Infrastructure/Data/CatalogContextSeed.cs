using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public static class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool checkProducts = productCollection.Find(b => true).Any();
            if (!checkProducts)
            {
                //var productsData = File.ReadAllText("./Catalog/Catalog.Infrastructure/Data/SeedData/products.json");
                var path = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "products.json");
                var productsData = File.ReadAllText(path);

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products != null)
                {
                    foreach (var item in products)
                    {
                        productCollection.InsertOneAsync(item);
                    }
                }
            }
        }
    }
}
