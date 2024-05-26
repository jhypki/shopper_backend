using ShopperBackend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ShopperBackend.Services;

public interface IProductsService
{
    Task<List<Product>> GetAsync();
    Task<Product> GetAsync(string id);
    Task CreateAsync(Product product);
    Task UpdateAsync(string id, Product product);
    Task RemoveAsync(string id);
}

public class ProductsService : IProductsService
{
    private readonly IMongoCollection<Product> _ProductsCollection;

    public ProductsService(
        IOptions<ShopperDatabaseSettings> ProductStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            ProductStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            ProductStoreDatabaseSettings.Value.DatabaseName);

        _ProductsCollection = mongoDatabase.GetCollection<Product>(
            ProductStoreDatabaseSettings.Value.ProductsCollectionName);
    }

    public async Task<List<Product>> GetAsync() =>
        await _ProductsCollection.Find(_ => true).ToListAsync();

    public async Task<Product> GetAsync(string id) =>
        await _ProductsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Product newProduct) =>
        await _ProductsCollection.InsertOneAsync(newProduct);

    public async Task UpdateAsync(string id, Product updatedProduct) =>
        await _ProductsCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

    public async Task RemoveAsync(string id) =>
        await _ProductsCollection.DeleteOneAsync(x => x.Id == id);
}