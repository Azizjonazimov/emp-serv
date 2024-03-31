using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using FullStackApi.Models;

namespace FullStackApi.Data
{
    public class FullStackDbContext
    {
        private readonly IMongoDatabase _database;

        public FullStackDbContext(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("MongoDBConnectionString");
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("fullstackapi-web-database");

            // Initialize the Employees collection
            Employees = _database.GetCollection<Employee>("Employees");
        }

        // Define a property to access the Employees collection
        public IMongoCollection<Employee> Employees { get; }
    }
}