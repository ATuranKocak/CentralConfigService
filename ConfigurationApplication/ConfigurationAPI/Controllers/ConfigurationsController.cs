using Microsoft.AspNetCore.Mvc;
using ConfigurationLibrary.Interfaces;
using ConfigurationLibrary.Models;

namespace ConfigurationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationsController : ControllerBase
    {
        private readonly IConfigurationRepository _repository;

        public ConfigurationsController(IConfigurationRepository repository)
        {
            _repository = repository;
        }

        // GET: Belirli bir uygulama için konfigürasyonları getirir.
        [HttpGet("{applicationName}")]
        public async Task<IActionResult> GetConfigurations(string applicationName)
        {
            var configs = await _repository.GetActiveConfigurationsAsync(applicationName);
            return Ok(configs);
        }

        // POST: Yeni bir konfigürasyon kaydı ekler.
        [HttpPost]
        public async Task<IActionResult> AddConfiguration([FromBody] ConfigurationItem item)
        {
            await _repository.AddConfigurationAsync(item);
            
            return CreatedAtAction(nameof(GetConfigurations), new { applicationName = item.ApplicationName }, item);
        }

        // PUT: Var olan bir kaydı günceller.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConfiguration(int id, [FromBody] ConfigurationItem updatedItem)
        {
            var result = await _repository.UpdateConfigurationAsync(id, updatedItem);

            if (!result)
                return NotFound(new { message = $"ID {id} kayıt bulunamadı." });

            return Ok(new { message = "Kayıt güncellendi", updatedItem });
        }

        // DELETE: Belirli bir kaydı siler.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfiguration(int id)
        {
            var result = await _repository.DeleteConfigurationAsync(id);

            if (!result)
                return NotFound(new { message = $"ID {id} kayıt bulunamadı." });

            return Ok(new { message = "Kayıt silindi" });
        }
    }
}

