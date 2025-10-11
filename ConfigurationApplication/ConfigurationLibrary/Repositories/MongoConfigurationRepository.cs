using MongoDB.Driver;
using ConfigurationLibrary.Models;
using ConfigurationLibrary.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigurationLibrary.Repositories
{
    public class MongoConfigurationRepository : IConfigurationRepository
    {
        private readonly IMongoCollection<ConfigurationItem> _collection;

        public MongoConfigurationRepository(string connectionString)
        {
            // Eğer Program.cs'te bir hata olursa ve connectionString boş gelirse yine de kontrol edelim.
            if (string.IsNullOrEmpty(connectionString))
            {
                // Bu durum oluşmamalı ama önlem olarak 'mongo' 
                connectionString = "mongodb://mongo:27017"; 
            }

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("ConfigurationDB");
            _collection = database.GetCollection<ConfigurationItem>("Configurations");
        }

        public async Task<List<ConfigurationItem>> GetActiveConfigurationsAsync(string applicationName)
        {
            return await _collection
                .Find(x => x.ApplicationName == applicationName && x.IsActive)
                .ToListAsync();
        }

        public async Task AddConfigurationAsync(ConfigurationItem item)
        {
            await _collection.InsertOneAsync(item);
        }

        public async Task<bool> UpdateConfigurationAsync(int id, ConfigurationItem updatedItem)
        {
            var filter = Builders<ConfigurationItem>.Filter.Eq(c => c.Id, id);

            var existingItem = await _collection.Find(filter).FirstOrDefaultAsync();

            if (existingItem == null)
            {
                return false;
            }
            
            updatedItem._id = existingItem._id;

            var result = await _collection.ReplaceOneAsync(filter, updatedItem); 
            
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteConfigurationAsync(int id)
        {
            var filter = Builders<ConfigurationItem>.Filter.Eq("Id", id);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

    }
}
