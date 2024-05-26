namespace ShopperBackend.Models;

public class ShopperDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string ProductsCollectionName { get; set; } = null!;
    public string UsersCollectionName { get; set; } = null!;
}