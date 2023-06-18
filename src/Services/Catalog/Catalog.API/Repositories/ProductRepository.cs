using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
  public readonly ICatalogContext _context;

  public ProductRepository(ICatalogContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }

  public async Task<IEnumerable<Product>> GetProducts()
  {
    return await _context.Products.Find(p => true).ToListAsync();
  }

  public async Task<Product> GetProduct(string id)
  {
    return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<Product>> GetProductByName(string name)
  {
    FilterDefinition<Product> nameFilter = Builders<Product>.Filter.Eq(p => p.Name, name);

    return await _context.Products.Find(nameFilter).ToListAsync();
  }

  public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
  {
    FilterDefinition<Product> categoryFilter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
    return await _context.Products.Find(categoryFilter).ToListAsync();
  }

  public async Task CreateProduct(Product product)
  {
    await _context.Products.InsertOneAsync(product);
  }

  public async Task<bool> DeleteProduct(string id)
  {
    //FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
    //DeleteResult delete = await _context.Products.DeleteOneAsync(filter);

    var deleteResult = await _context.Products.DeleteOneAsync(filter: filter => filter.Id == id);
    return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
  }

  public async Task<bool> UpdateProduct(Product product)
  {
    var updateResult = await _context.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

    return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
  }
}
