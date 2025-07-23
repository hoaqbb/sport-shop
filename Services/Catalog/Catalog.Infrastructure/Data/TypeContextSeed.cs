using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public static class TypeContextSeed
    {
        public static void SeedData(IMongoCollection<ProductType> typeCollection)
        {
            bool checkTypes = typeCollection.Find(b => true).Any();
            if (!checkTypes)
            {
                //var typesData = File.ReadAllText("./Catalog/Catalog.Infrastructure/Data/SeedData/types.json");
                var path = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "types.json");
                var typesData = File.ReadAllText(path);

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                if (types != null)
                {
                    foreach (var item in types)
                    {
                        typeCollection.InsertOneAsync(item);
                    }
                }
            }
        }
    }
}
