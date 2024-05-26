using MongoDB.Driver;
using ShopperBackend.Models;
using Microsoft.Extensions.Options;

namespace ShopperBackend.Services;

public class UsersService
{
    private readonly IMongoCollection<User> _users;

    public UsersService(
        IOptions<ShopperDatabaseSettings> shopperDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            shopperDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            shopperDatabaseSettings.Value.DatabaseName);

        _users = mongoDatabase.GetCollection<User>(
            shopperDatabaseSettings.Value.UsersCollectionName);
    }

    public async Task<List<User>> GetAsync() =>
            await _users.Find(_ => true).ToListAsync();
    public async Task<User> GetById(string id) =>
        await _users.Find<User>(user => user.Id == id).FirstOrDefaultAsync();
    public async Task<User> GetByUsernameAsync(string username) =>
        await _users.Find<User>(user => user.Username == username).FirstOrDefaultAsync();


}