using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }

        public IMongoCollection<ProductBrand> Brands { get; }

        public IMongoCollection<ProductType> Types { get; }

        public CatalogContext(IConfiguration configuration)
        {
            var mongoDbConfig = configuration.GetSection("DatabaseSettings");
            var client = new MongoClient(mongoDbConfig["ConnectionString"]);

            var database = client.GetDatabase(mongoDbConfig["DatabaseName"]);

            Brands = database.GetCollection<ProductBrand>(mongoDbConfig["BrandsCollection"]);
            Types = database.GetCollection<ProductType>(mongoDbConfig["TypesCollection"]);
            Products = database.GetCollection<Product>(mongoDbConfig["ProductsCollection"]);

            BrandContextSeed.SeedData(Brands);
            TypeContextSeed.SeedData(Types);
            CatalogContextSeed.SeedData(Products);
        }
    }
}
