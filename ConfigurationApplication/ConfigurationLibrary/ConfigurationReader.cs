using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ConfigurationLibrary.Models;
using ConfigurationLibrary.Interfaces;
using ConfigurationLibrary.Repositories;

namespace ConfigurationLibrary
{
    public class ConfigurationReader
    {
        private readonly string _applicationName;
        private readonly string _connectionString;
        private readonly int _refreshTimerIntervalInMs;
        private readonly IConfigurationRepository _repository;

        private Dictionary<string, ConfigurationItem> _cache = new();
        private Timer _timer;

        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            _applicationName = applicationName;
            _connectionString = connectionString;
            _refreshTimerIntervalInMs = refreshTimerIntervalInMs;

            _repository = new MongoConfigurationRepository(_connectionString);

            // İlk yükleme
            LoadConfigurationsAsync().Wait();

            // Belirli aralıklarla konfigürasyonları yenile
            _timer = new Timer(async _ => await LoadConfigurationsAsync(), null, _refreshTimerIntervalInMs, _refreshTimerIntervalInMs);
        }

        private async Task LoadConfigurationsAsync()
        {
            try
            {
                var configs = await _repository.GetActiveConfigurationsAsync(_applicationName);
                var newCache = new Dictionary<string, ConfigurationItem>();

                foreach (var config in configs)
                {
                    newCache[config.Name] = config;
                }

                _cache = newCache;
                Console.WriteLine($"[{DateTime.Now:T}] Konfigürasyon güncellendi. ({configs.Count} kayıt)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] Konfigürasyon yüklenemedi, son geçerli kayıtlar kullanılacak: {ex.Message}");
            }
        }

        public T GetValue<T>(string key)
        {
            if (_cache.TryGetValue(key, out var config))
            {
                try
                {
                    object converted = Convert.ChangeType(config.Value, typeof(T));
                    return (T)converted;
                }
                catch
                {
                    throw new InvalidCastException($"Anahtar '{key}' için değer tipi '{config.Type}' bekleniyor ancak '{typeof(T).Name}' istendi.");
                }
            }

            throw new KeyNotFoundException($"Anahtar bulunamadı: {key}");
        }
    }
}
