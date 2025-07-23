using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, IBrandRepository, ITypeRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        async Task<Pagination<Product>> IProductRepository.GetProducts(CatalogSpecificationParams catalogSpecificationParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;
            if(!string.IsNullOrEmpty(catalogSpecificationParams.Search))
            {
                filter = filter & builder.Where(p => p.Name.ToLower().Contains(catalogSpecificationParams.Search.ToLower()));
            }
            if(!string.IsNullOrEmpty(catalogSpecificationParams.BrandId))
            {
                var brandFilter = builder.Eq(p => p.Brands.Id, catalogSpecificationParams.BrandId);
                filter &= brandFilter;
            }
            if (!string.IsNullOrEmpty(catalogSpecificationParams.TypeId))
            {
                var typeFilter = builder.Eq(p => p.Brands.Id, catalogSpecificationParams.BrandId);
                filter &= typeFilter;
            }
            var totalItems = await _context.Products.CountDocumentsAsync(filter);
            var data = await DataFilter(catalogSpecificationParams, filter);

            return new Pagination<Product>(
                catalogSpecificationParams.PageIndex, 
                catalogSpecificationParams.PageSize,
                (int)totalItems,
                data
            );
        }

        async Task<IEnumerable<Product>> IProductRepository.GetProductsByName(string name)
        {
            return await _context.Products
                .Find(p => p.Name.ToLower() == name.ToLower())
                .ToListAsync();
        }

        async Task<IEnumerable<Product>> IProductRepository.GetProductsByBrand(string brandName)
        {
            return await _context.Products
                .Find(p => p.Brands.Name.ToLower() == brandName.ToLower())
                .ToListAsync(); 
        }

        async Task<Product> IProductRepository.CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }

        async Task<bool> IProductRepository.DeleteProduct(string id)
        {
            var deletedProduct = await _context.Products
                .DeleteOneAsync(p => p.Id == id);

            return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
        }

        async Task<bool> IProductRepository.UpdateProduct(Product product)
        {
            var updatedProduct = await _context.Products
                .ReplaceOneAsync(p => p.Id == product.Id, product);

            return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
        }

        async Task<IEnumerable<ProductBrand>> IBrandRepository.GetAllBrands()
        {
            return await _context.Brands
                .Find(brand => true)
                .ToListAsync();
        }

        async Task<IEnumerable<ProductType>> ITypeRepository.GetAllTypes()
        {
            return await _context.Types
                .Find(type => true)
                .ToListAsync();
        }

        private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecificationParams catalogSpecificationParams, FilterDefinition<Product> filter)
        {
            var sortDefn = Builders<Product>.Sort.Ascending("Name");
            if (!string.IsNullOrEmpty(catalogSpecificationParams.Sort))
            {
                switch (catalogSpecificationParams.Sort)
                {
                    case "priceAsc":
                        sortDefn = Builders<Product>.Sort.Ascending(p => p.Price);
                        break;
                    case "priceDesc":
                        sortDefn = Builders<Product>.Sort.Descending(p => p.Price);
                        break;
                    default:
                        sortDefn = Builders<Product>.Sort.Ascending(p => p.Name);
                        break;
                }
            }
            return await _context.Products
                .Find(filter)
                .Sort(sortDefn)
                .Skip(catalogSpecificationParams.PageSize * (catalogSpecificationParams.PageIndex - 1))
                .Limit(catalogSpecificationParams.PageSize)
                .ToListAsync();
        }
    }
}
