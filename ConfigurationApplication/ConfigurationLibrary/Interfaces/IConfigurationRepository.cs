using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigurationLibrary.Models;

namespace ConfigurationLibrary.Interfaces
{
    public interface IConfigurationRepository
    {
        // Okuma
        Task<List<ConfigurationItem>> GetActiveConfigurationsAsync(string applicationName);
        
        // Ekleme (kaydetme)
        Task AddConfigurationAsync(ConfigurationItem item);
        
        // GÜNCELLEME (Update) işlemi 
        // Başarılı olup olmadığını belirtmek için bool döndürür
        Task<bool> UpdateConfigurationAsync(int id, ConfigurationItem updatedItem);
        
        // SİLME (Delete) işlemi 
        // Başarılı olup olmadığını belirtmek için bool döndürür
        Task<bool> DeleteConfigurationAsync(int id);
    }
}



