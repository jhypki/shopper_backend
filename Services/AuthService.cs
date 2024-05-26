using MongoDB.Driver;
using ShopperBackend.Models;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace ShopperBackend.Services;

public class AuthService
{
    private readonly IMongoCollection<User> _users;

    public AuthService(
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

    public async Task<User> GetAsync(string id) =>
        await _users.Find<User>(user => user.Id == id).FirstOrDefaultAsync();

    public async Task<User> GetByUsernameAsync(string username) =>
        await _users.Find<User>(user => user.Username == username).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser) =>
        await _users.InsertOneAsync(newUser);

    public async Task UpdateAsync(string id, User updatedUser) =>
        await _users.ReplaceOneAsync(user => user.Id == id, updatedUser);

    public async Task RemoveAsync(string id) =>
        await _users.DeleteOneAsync(user => user.Id == id);
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}